// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Falling : CreatureAnimation
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetParent(Creature.Constructor.Root);
            }
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetParent(Dynamic.Transform);

                if (Creature.Grounded.IsGrounded)
                {
                    // step
                }
            }

            Walking walking = animator.GetBehaviour<Walking>();
            if (walking != null)
            {
                walking.StopMovingLegs();
            }
        }
    }
}