// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor), typeof(MeshCollider))]
    public class CreatureCollider : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Mesh capsuleMesh;
        private MeshCollider meshCollider;

        private Mesh tmpMesh;
        #endregion

        #region Properties
        public CreatureConstructor Constructor { get; private set; }

        public Collider Hitbox => meshCollider;
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }
        private void OnEnable()
        {
            meshCollider.enabled = true;
        }
        private void OnDisable()
        {
            meshCollider.enabled = false;
        }

        private void Initialize()
        {
            Constructor = GetComponent<CreatureConstructor>();

            meshCollider = GetComponent<MeshCollider>();
            meshCollider.sharedMesh = new Mesh();
            tmpMesh = new Mesh();
        }

        public void UpdateCollider()
        {
            // Calculate bounds
            Constructor.SkinnedMeshRenderer.BakeMesh(tmpMesh);

            MinMax minMaxX = new MinMax(Mathf.Infinity, Mathf.NegativeInfinity);
            MinMax minMaxY = new MinMax(Mathf.Infinity, Mathf.NegativeInfinity);
            MinMax minMaxZ = new MinMax(Mathf.Infinity, Mathf.NegativeInfinity);
            foreach (Vector3 vertex in tmpMesh.vertices)
            {
                if (vertex.x < minMaxX.min)
                {
                    minMaxX.min = vertex.x;
                }
                if (vertex.x > minMaxX.max)
                {
                    minMaxX.max = vertex.x;
                }

                if (vertex.y < minMaxY.min)
                {
                    minMaxY.min = vertex.y;
                }
                if (vertex.y > minMaxY.max)
                {
                    minMaxY.max = vertex.y;
                }

                if (vertex.z < minMaxZ.min)
                {
                    minMaxZ.min = vertex.z;
                }
                if (vertex.z > minMaxZ.max)
                {
                    minMaxZ.max = vertex.z;
                }
            }

            Vector3 center = new Vector3(minMaxX.Average, minMaxY.Average, minMaxZ.Average);
            Vector3 size = new Vector3(minMaxX.Range, minMaxY.Range, minMaxZ.Range);
            Constructor.SkinnedMeshRenderer.localBounds = new UnityEngine.Bounds(center, size);

            UnityEngine.Bounds bounds = Constructor.SkinnedMeshRenderer.localBounds;

            // Setup meshes
            Transform tmp = new GameObject().transform;
            tmp.SetParent(Dynamic.Transform, false);

            MeshFilter capsule = new GameObject("_TMP").AddComponent<MeshFilter>();
            capsule.sharedMesh = capsuleMesh;
            capsule.transform.SetZeroParent(Dynamic.Transform);
            capsule.transform.localPosition = Vector3.up * (bounds.size.y / 2f);
            capsule.transform.localScale = new Vector3(bounds.size.x, bounds.size.y / 2f, bounds.size.x);
 
            if (Constructor.Legs.Count > 0)
            {
                tmp.localPosition = Constructor.Body.localPosition;
            }
            else
            {
                capsule.transform.position += Vector3.up * (bounds.size.y / 2f - bounds.center.y);
                tmp.localPosition = Vector3.up * (bounds.size.y / 2f - bounds.center.y);
            }

            // Combine
            List<CombineInstance> combine = new List<CombineInstance>()
            {
                new CombineInstance(){ mesh = tmpMesh, transform = tmp.localToWorldMatrix }
            };
            if (Constructor.Legs.Count > 0)
            {
                combine.Add(new CombineInstance() { mesh = capsule.mesh, transform = capsule.transform.localToWorldMatrix });
            }
            meshCollider.sharedMesh.CombineMeshes(combine.ToArray());
            tmpMesh.Clear();
        }
        #endregion
    }
}