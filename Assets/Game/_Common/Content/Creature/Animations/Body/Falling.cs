// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using static DanielLochner.Assets.FootstepEffects;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Falling : CreatureAnimation
    {
        #region Fields
        [SerializeField] private float maxFallSpeed;
        #endregion

        #region Methods
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

                float i = Mathf.Clamp01(Vector3.Project(Creature.Velocity.Linear, -Creature.transform.up).magnitude / maxFallSpeed);
                leg.Step(StepType.JumpEnd, i);
            }

            Walking walking = animator.GetBehaviour<Walking>();
            if (walking != null)
            {
                walking.StopMovingLegs();
            }
        }
        #endregion
    }
}