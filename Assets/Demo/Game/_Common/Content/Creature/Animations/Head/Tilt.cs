// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Tilt : CreatureAnimation
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
            },
            duration);
        }
        #endregion
    }
}