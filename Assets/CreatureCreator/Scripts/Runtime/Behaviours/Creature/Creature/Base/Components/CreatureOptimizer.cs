using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor), typeof(MB3_MeshBaker))]
    public class CreatureOptimizer : MonoBehaviour
    {
        #region Fields
        [SerializeField, Button("Optimize")] private bool optimize;

        private List<BodyPartConstructor> bodyPartsToAdd = new List<BodyPartConstructor>();
        #endregion

        #region Properties
        private CreatureConstructor Constructor { get; set; }
        private MB3_MeshBaker Baker { get; set; }

        public SkinnedMeshRenderer OptimizedCreature { get; private set; }

        public bool IsOptimized { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Baker = GetComponent<MB3_MeshBaker>();
        }
        private void LateUpdate()
        {
            if (OptimizedCreature == null) return;

            int j = Constructor.Bones.Count;
            foreach (BodyPartConstructor bodyPart in bodyPartsToAdd)
            {
                if (bodyPart.SkinnedMeshRenderer == null) continue;
                for (int i = 0; i < bodyPart.SkinnedMeshRenderer.sharedMesh.blendShapeCount; i++, j++)
                {
                    OptimizedCreature.SetBlendShapeWeight(j, bodyPart.SkinnedMeshRenderer.GetBlendShapeWeight(i));
                }
            }
        }

        public void Optimize()
        {
            if (SettingsManager.Data.OptimizeCreatures && !IsOptimized)
            {
                Baker.ClearMesh();

                foreach (BodyPartConstructor bpc in Constructor.BodyParts)
                {
                    if (bpc.IsVisible)
                    {
                        AddBodyPart(bpc);
                    }
                    if (bpc.Flipped.IsVisible)
                    {
                        AddBodyPart(bpc.Flipped);
                    }
                }
                FlipLimbs();

                GameObject[] objsToCombine = new GameObject[bodyPartsToAdd.Count + 1];
                objsToCombine[0] = Constructor.SkinnedMeshRenderer.gameObject;
                for (int i = 0; i < bodyPartsToAdd.Count; i++)
                {
                    objsToCombine[i + 1] = bodyPartsToAdd[i].Renderer.gameObject;
                }
                Baker.AddDeleteGameObjects(objsToCombine, null, true);

                Baker.Apply();
                FlipLimbs();

                OptimizedCreature = Baker.meshCombiner.resultSceneObject.GetComponentInChildren<SkinnedMeshRenderer>();

                for (int i = 0; i < Constructor.Bones.Count; i++)
                {
                    OptimizedCreature.SetBlendShapeWeight(i, Constructor.SkinnedMeshRenderer.GetBlendShapeWeight(i));
                }

                IsOptimized = true;
            }
        }
        public void Unoptimize()
        {
            if (IsOptimized)
            {
                Constructor.SkinnedMeshRenderer.enabled = true;
                foreach (BodyPartConstructor bodyPart in bodyPartsToAdd)
                {
                    bodyPart.Renderer.enabled = true;
                }
                bodyPartsToAdd.Clear();

                DestroyImmediate(Baker.meshCombiner.resultSceneObject.gameObject);

                IsOptimized = false;
            }
        }

        private void AddBodyPart(BodyPartConstructor bpc)
        {
            if (bpc.SkinnedMeshRenderer != null)
            {
                bpc.SkinnedMeshRenderer.sharedMesh = Instantiate(bpc.SkinnedMeshRenderer.sharedMesh);
            }

            Material[] mats = bpc.Renderer.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].name += $"_{bpc.name}";
            }
            bpc.Renderer.sharedMaterials = mats;

            bodyPartsToAdd.Add(bpc);
        }
        private void FlipLimbs()
        {
            foreach (BodyPartConstructor bodyPart in bodyPartsToAdd)
            {
                if (bodyPart is LimbConstructor && bodyPart.IsFlipped)
                {
                    bodyPart.Model.localScale = new Vector3(-bodyPart.Model.localScale.x, bodyPart.Model.localScale.y, bodyPart.Model.localScale.z);
                }
            }
        }
        #endregion
    }
}