using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureAnimator))]
    public class PlayerAnimator : MonoBehaviour
    {
        private CreatureAnimator Animator { get; set; }

        private void Awake()
        {
            Animator = GetComponent<CreatureAnimator>();
        }

        private void Start()
        {
            Animator.OnBuild += delegate
            {
                Animator.SetBool("HasArms", Animator.Arms.Count != 0);
                Animator.SetBool("HasLegs", Animator.Legs.Count != 0);
                Animator.SetBool("HasWings", Animator.Wings.Count != 0);
                Animator.SetBool("HasMouths", Animator.Mouths.Count != 0);
            };
        }
    }
}