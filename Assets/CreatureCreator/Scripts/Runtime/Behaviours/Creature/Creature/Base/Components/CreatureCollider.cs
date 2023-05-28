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
        [SerializeField] private Mesh cubeMesh;
        private MeshCollider meshCollider;
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
        }

        public void UpdateCollider()
        {
            Transform tmp = new GameObject("_TMP").transform;

            List<CombineInstance> combine = new List<CombineInstance>();
            foreach (Transform bone in Constructor.Bones)
            {
                MeshFilter meshFilter = new GameObject("Bone").AddComponent<MeshFilter>();
                meshFilter.sharedMesh = cubeMesh;
                meshFilter.transform.SetZeroParent(bone);

                float width = Constructor.Dimensions.Body.Width;

                meshFilter.transform.localScale = new Vector3(width, width, Constructor.BoneSettings.Length);

if (Constructor.Legs.Count > 0)
{
                tmp.SetZeroParent(transform);

}
else
{
                tmp.SetZeroParent(Constructor.Body);

}
                tmp.localPosition = -Vector3.up * Constructor.BodyAlignedOffset;


                meshFilter.transform.SetParent(tmp, true);

                tmp.SetZeroParent(Dynamic.Transform);

                combine.Add(new CombineInstance()
                {
                    mesh = cubeMesh,
                    transform = meshFilter.transform.localToWorldMatrix
                });
            }

            if (Constructor.Legs.Count > 0)
            {
                MeshFilter meshFilter = new GameObject("Legs").AddComponent<MeshFilter>();
                meshFilter.sharedMesh = cubeMesh;
                meshFilter.transform.SetZeroParent(Dynamic.Transform);
                meshFilter.transform.localScale *= Constructor.Dimensions.Body.Width;
                meshFilter.transform.localPosition = Vector3.up * Constructor.Dimensions.Body.Width;

                combine.Add(new CombineInstance()
                {
                    mesh = cubeMesh,
                    transform = meshFilter.transform.localToWorldMatrix
                });
            }

            meshCollider.sharedMesh.CombineMeshes(combine.ToArray());




            //  if (useComplexMesh)
            // {
            //     MeshFilter capsule = new GameObject("_TMP").AddComponent<MeshFilter>();
            //     capsule.sharedMesh = capsuleMesh;
            //     capsule.transform.SetZeroParent(Dynamic.Transform);
            //     capsule.transform.localPosition = Vector3.up * (Constructor.Dimensions.Body.Height / 2f);
            //     capsule.transform.localScale = new Vector3(Constructor.Dimensions.Body.Width, Constructor.Dimensions.Body.Height / 2f, Constructor.Dimensions.Body.Width);
    
            //     Transform tmp = new GameObject().transform;
            //     tmp.SetParent(Dynamic.Transform, false);
            //     if (Constructor.Legs.Count > 0)
            //     {
            //         tmp.localPosition = Constructor.Body.localPosition;
            //     }
            //     else
            //     {
            //         tmp.localPosition = Vector3.up * Constructor.BodyAlignedOffset;
            //     }

            //     Mesh tmpMesh = new Mesh();
            //     Constructor.SkinnedMeshRenderer.BakeMesh(tmpMesh);
            //     List<CombineInstance> combine = new List<CombineInstance>()
            //     {
            //         new CombineInstance(){ mesh = tmpMesh, transform = tmp.localToWorldMatrix }
            //     };
            //     if (Constructor.Legs.Count > 0)
            //     {
            //         combine.Add(new CombineInstance() { mesh = capsule.mesh, transform = capsule.transform.localToWorldMatrix });
            //     }
            //     meshCollider.sharedMesh.CombineMeshes(combine.ToArray());
            //     Destroy(tmpMesh);
            // }
        }
        #endregion
    }
}