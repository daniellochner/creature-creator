// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Smell")]
    public class Smell : Ability
    {
        #region Fields
        [SerializeField] private float senseFactor;
        #endregion

        #region Methods
        public override void OnAdd()
        {
            base.OnAdd();
            foreach (VisibilityObject obj in VisibilityManager.Instance.Objects)
            {
                obj.Radius *= senseFactor;
            }
        }
        public override void OnRemove()
        {
            base.OnRemove();
            foreach (VisibilityObject obj in VisibilityManager.Instance.Objects)
            {
                obj.Radius /= senseFactor;
            }
        }
        #endregion
    }
}