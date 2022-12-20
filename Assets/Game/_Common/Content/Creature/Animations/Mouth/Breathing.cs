using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Breathing : CreatureAnimation
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (MouthAnimator mouth in Creature.Mouths)
            {
                mouth.SetOpen(animator.GetFloat("Mouth_Breathe"));
            }
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (MouthAnimator mouth in Creature.Mouths)
            {
                mouth.SetOpen(0.5f);
            }
        }
    }
}