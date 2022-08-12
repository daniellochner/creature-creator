using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Gliding : CreatureAnimation
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
            }

            Walking walking = animator.GetBehaviour<Walking>();
            if (walking != null)
            {
                walking.StopMovingLegs();
            }
        }
    }
}