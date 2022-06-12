using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Breathing : SceneLinkedSMB<CreatureAnimator>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (MouthAnimator mouth in m_MonoBehaviour.Mouths)
            {
                mouth.SetOpen(animator.GetFloat("Mouth_Breathe"));
            }
        }
    }
}