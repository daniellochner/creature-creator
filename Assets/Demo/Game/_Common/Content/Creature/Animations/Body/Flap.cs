// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Flap : CreatureAnimation
    {
        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (WingAnimator wing in Creature.Wings)
            {
                wing.Flap();
            }
        }
        #endregion
    }
}