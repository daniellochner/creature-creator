using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Swimming : CreatureAnimation
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetPositionAndRotation(Creature.Constructor.transform.L2WSpace(leg.DefaultFootLocalPos), Quaternion.identity);
            }
        }
    }
}