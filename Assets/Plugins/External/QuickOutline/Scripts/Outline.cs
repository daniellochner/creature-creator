//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Outline : MonoBehaviour
{
    #region Fields
    [SerializeField] private Mode outlineMode;
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField, Range(0f, 10f)] private float outlineWidth = 2f;

    [Header("Optional")]
    [SerializeField, Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")] private bool precomputeOutline = false;
    [SerializeField] private bool includeSubMeshes = false;

    [SerializeField, HideInInspector] private List<Mesh> bakeKeys = new List<Mesh>();
    [SerializeField, HideInInspector] private List<ListVector3> bakeValues = new List<ListVector3>();

    private Renderer[] renderers;
    private Material outlineMaskMaterial;
    private Material outlineFillMaterial;
    private bool needsUpdate;

    private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();
    #endregion

    #region Properties
    public Mode OutlineMode
    {
        get
        {
            return outlineMode;
        }
        set
        {
            outlineMode = value;
            needsUpdate = true;
        }
    }
    public Color OutlineColor
    {
        get
        {
            return outlineColor;
        }
        set
        {
            outlineColor = value;
            needsUpdate = true;
        }
    }
    public float OutlineWidth
    {
        get
        {
            return outlineWidth;
        }
        set
        {
            outlineWidth = value;
            needsUpdate = true;
        }
    }
    public bool IncludeSubMeshes
    {
        get { return includeSubMeshes; }
        set { includeSubMeshes = value; }
    }
    #endregion

    #region Methods
    private void Awake()
    {
        if (includeSubMeshes)
        {
            foreach (MeshFilter meshFilter in GetComponentsInChildren<MeshFilter>())
            {
                Mesh mesh = meshFilter.mesh;
                if (mesh.subMeshCount > 1)
                {
                    mesh.SetTriangles(mesh.triangles, ++mesh.subMeshCount - 1);
                }
            }
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                Mesh mesh = skinnedMeshRenderer.sharedMesh;
                if (mesh.subMeshCount > 1 && mesh.GetTriangles(mesh.subMeshCount - 1).Length != mesh.triangles.Length / 2) // Necessary to prevent a memory leak (since we are modifying the sharedMesh).
                {
                    mesh.SetTriangles(mesh.triangles, ++mesh.subMeshCount - 1);
                }
            }
        }

        // Cache renderers
        renderers = GetComponentsInChildren<Renderer>();

        // Instantiate outline materials
        outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMask"));
        outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"));

        outlineMaskMaterial.name = "OutlineMask (Instance)";
        outlineFillMaterial.name = "OutlineFill (Instance)";

        // Retrieve or generate smooth normals
        LoadSmoothNormals();

        // Apply material properties immediately
        needsUpdate = true;
    }
    private void OnValidate()
    {
        // Update material properties
        needsUpdate = true;

        // Clear cache when baking is disabled or corrupted
        if (!precomputeOutline && bakeKeys.Count != 0 || bakeKeys.Count != bakeValues.Count)
        {
            bakeKeys.Clear();
            bakeValues.Clear();
        }

        // Generate smooth normals when baking is enabled
        if (precomputeOutline && bakeKeys.Count == 0)
        {
            Bake();
        }
    }
    private void Update()
    {
        if (needsUpdate)
        {
            needsUpdate = false;
            UpdateMaterialProperties();
        }
    }
    private void OnDisable()
    {
        foreach (var renderer in renderers)
        {
            // Remove outline shaders
            var materials = renderer.sharedMaterials.ToList();

            materials.Remove(outlineMaskMaterial);
            materials.Remove(outlineFillMaterial);

            renderer.materials = materials.ToArray();
        }
    }
    private void OnEnable()
    {
        foreach (var renderer in renderers)
        {
            // Append outline shaders
            var materials = renderer.sharedMaterials.ToList();

            materials.Add(outlineMaskMaterial);
            materials.Add(outlineFillMaterial);

            renderer.materials = materials.ToArray();
        }
    }
    private void OnDestroy()
    {
        // Destroy material instances
        Destroy(outlineMaskMaterial);
        Destroy(outlineFillMaterial);
    }

    private void UpdateMaterialProperties()
    {
        // Apply properties according to mode
        outlineFillMaterial.SetColor("_OutlineColor", outlineColor);

        switch (outlineMode)
        {
            case Mode.OutlineAll:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;
            case Mode.OutlineVisible:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;
            case Mode.OutlineHidden:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;
            case Mode.OutlineAndSilhouette:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;
            case Mode.SilhouetteOnly:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                outlineFillMaterial.SetFloat("_OutlineWidth", 0f);
                break;
        }
    }
    private void Bake()
    {
        // Generate smooth normals for each mesh
        var bakedMeshes = new HashSet<Mesh>();

        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            // Skip duplicates
            if (!bakedMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }

            // Serialize smooth normals
            var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

            bakeKeys.Add(meshFilter.sharedMesh);
            bakeValues.Add(new ListVector3() { data = smoothNormals });
        }
    }
    private void LoadSmoothNormals()
    {
        // Retrieve or generate smooth normals
        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            if (meshFilter.mesh == null) continue;

            // Skip if smooth normals have already been adopted
            if (!registeredMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }

            // Retrieve or generate smooth normals
            var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
            var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

            // Store smooth normals in UV3
            meshFilter.sharedMesh.SetUVs(3, smoothNormals);
        }

        // Clear UV3 on skinned mesh renderers
        foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (skinnedMeshRenderer.sharedMesh == null) continue;

            if (registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
            {
                skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
            }
        }
    }
    private List<Vector3> SmoothNormals(Mesh mesh)
    {
        // Group vertices by location
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

        // Copy normals to a new list
        var smoothNormals = new List<Vector3>(mesh.normals);

        // Average normals for grouped vertices
        foreach (var group in groups)
        {
            // Skip single vertices
            if (group.Count() == 1)
            {
                continue;
            }

            // Calculate the average normal
            var smoothNormal = Vector3.zero;

            var meshNormals = mesh.normals; // Cache mesh normals.
            foreach (var pair in group)
            {
                smoothNormal += meshNormals[pair.Value];
            }

            smoothNormal.Normalize();

            // Assign smooth normal to each vertex
            foreach (var pair in group)
            {
                smoothNormals[pair.Value] = smoothNormal;
            }
        }
        return smoothNormals;
    }
    #endregion

    #region Enumerators
    public enum Mode
    {
        OutlineAll,
        OutlineVisible,
        OutlineHidden,
        OutlineAndSilhouette,
        SilhouetteOnly
    }
    #endregion

    #region Inner Classes
    [Serializable]
    private class ListVector3
    {
        public List<Vector3> data;
    }
    #endregion
}