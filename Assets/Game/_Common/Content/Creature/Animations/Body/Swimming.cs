using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Swimming : CreatureAnimation
    {
        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            FreezeLegs();
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            FreezeLegs();
        }
        private void FreezeLegs()
        {
            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.Anchor.SetPositionAndRotation(Creature.Constructor.transform.L2WSpace(leg.DefaultFootLocalPos), Quaternion.identity);
            }
        }
        #endregion
    }
}