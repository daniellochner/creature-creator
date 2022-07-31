// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MouthAnimator : BodyPartAnimator
    {
        #region Fields
        [SerializeField] private int oIndex = -1;
        [SerializeField] private int cIndex = -1;
        #endregion

        #region Methods
        public void SetOpen(float amount)
        {
            if (oIndex != -1)
            {
                float oWeight = (amount > 0.5f) ? Mathf.InverseLerp(0.5f, 1.0f, amount) * 100f : 0f;
                BodyPartConstructor.SkinnedMeshRenderer.SetBlendShapeWeight(oIndex, oWeight);
            }
            if (cIndex != -1)
            {
                float cWeight = (amount < 0.5f) ? Mathf.InverseLerp(0.0f, 0.5f, 0.5f - amount) * 100f : 0f;
                BodyPartConstructor.SkinnedMeshRenderer.SetBlendShapeWeight(cIndex, cWeight);
            }
        }
        #endregion
    }
}