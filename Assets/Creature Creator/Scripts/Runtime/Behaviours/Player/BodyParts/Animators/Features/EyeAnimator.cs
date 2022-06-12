// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class EyeAnimator : BodyPartAnimator
    {
        #region Fields
        [SerializeField] private int index = -1;
        #endregion

        #region Methods
        public void SetClose(float amount)
        {
            if (index != -1)
            {
                BodyPartConstructor.SkinnedMeshRenderer.SetBlendShapeWeight(index, amount * 100f);
            }
        }
        #endregion
    }
}