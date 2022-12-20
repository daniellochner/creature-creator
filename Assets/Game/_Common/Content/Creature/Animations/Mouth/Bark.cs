// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Bark : CreatureAnimation
    {
        #region Fields
        [SerializeField] private float duration;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Creature.InvokeOverTime(delegate (float p)
            {
                foreach (MouthAnimator mouth in Creature.Mouths)
                {
                    mouth.SetOpen(0.5f + 0.5f * Mathf.Sin(p * Mathf.PI));
                }
            },
            duration);
        }
        #endregion
    }
}