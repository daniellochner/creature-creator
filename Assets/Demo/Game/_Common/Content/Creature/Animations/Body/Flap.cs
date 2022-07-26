// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Flap : CreatureAnimation
    {
        #region Fields
        [SerializeField] private CreatureEffector.Sound[] flapSounds;
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (WingAnimator wing in Creature.Wings)
            {
                wing.Flap();
            }
            if (PerformLogic)
            {
                Creature.Effector.PlaySound(flapSounds);
            }
        }
        #endregion
    }
}