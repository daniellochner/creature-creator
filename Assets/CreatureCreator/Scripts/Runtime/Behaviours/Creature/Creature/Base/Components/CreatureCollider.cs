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
        [SerializeField] private Mesh baseMesh;
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
            meshCollider.sharedMesh = new Mesh
            {
                name = "Creature"
            };
        }

        public void UpdateCollider()
        {
            Transform tmp = new GameObject("_TMP").transform;

            List<CombineInstance> combine = new List<CombineInstance>();
            for (int i = 0; i < Constructor.Bones.Count; i++)
            {
                Transform bone = new GameObject($"Bone.{i}").transform;
                bone.SetZeroParent(Constructor.Bones[i]);

                float width = 2f * Constructor.BoneSettings.Radius * Mathf.Lerp(1, 4, Constructor.GetWeight(i) / 100f);
                bone.localScale = new Vector3(width, width, Constructor.BoneSettings.Length);

                if (Constructor.Legs.Count > 0)
                {
                    tmp.SetZeroParent(transform);
                }
                else
                {
                    tmp.SetZeroParent(Constructor.Body);
                    tmp.localPosition = new Vector3(0f, Constructor.SkinnedMeshRenderer.localBounds.center.y - (Constructor.SkinnedMeshRenderer.localBounds.size.y / 2f), 0f);
                }
                bone.SetParent(tmp, true);
                tmp.SetZeroParent(Dynamic.Transform);

                combine.Add(new CombineInstance()
                {
                    mesh = cubeMesh,
                    transform = bone.localToWorldMatrix
                });
            }

            if (Constructor.Legs.Count > 0)
            {
                Transform legs = new GameObject("Legs").transform;
                legs.SetZeroParent(tmp);
                legs.localScale *= Constructor.Dimensions.Body.Width;

                combine.Add(new CombineInstance()
                {
                    mesh = baseMesh,
                    transform = legs.localToWorldMatrix
                });
            }

            meshCollider.sharedMesh.CombineMeshes(combine.ToArray());

            Destroy(tmp.gameObject);
        }
        #endregion
    }
}