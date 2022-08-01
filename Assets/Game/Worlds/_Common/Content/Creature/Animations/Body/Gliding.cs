using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Gliding : CreatureAnimation
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetParent(Creature.Constructor.Root);
                leg.Anchor.SetPositionAndRotation(Creature.Constructor.transform.L2WSpace(leg.DefaultFootLocalPos), Quaternion.identity);
            }
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetParent(Dynamic.Transform);
            }
        }
        //public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    foreach (LegAnimator leg in Creature.Legs)
        //    {
        //        leg.Anchor.SetPositionAndRotation(Creature.Constructor.transform.L2WSpace(leg.DefaultFootLocalPos), Quaternion.identity);
        //    }
        //}
    }
}