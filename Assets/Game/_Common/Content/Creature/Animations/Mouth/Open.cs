// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Open : CreatureAnimation
    {
        #region Fields
        [SerializeField] private float duration;
        [SerializeField] private bool reverse;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Creature.InvokeOverTime(delegate (float p)
            {
                float t = 0.5f * (1f + (reverse ? (1f - p) : p));
                foreach (MouthAnimator mouth in Creature.Mouths)
                {
                    mouth.SetOpen(t);
                }
            },
            duration);
        }
        #endregion
    }
}