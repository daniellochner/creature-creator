using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureAnimator))]
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        private CreatureAnimator CreatureAnimator { get; set; }
        private Animator Animator { get; set; }

        private bool HasArms => CreatureAnimator.Arms.Count != 0;
        private bool HasLegs => CreatureAnimator.Legs.Count != 0;
        private bool HasWings => CreatureAnimator.Wings.Count != 0;
        private bool HasMouths => CreatureAnimator.Mouths.Count != 0;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            CreatureAnimator = GetComponent<CreatureAnimator>();
        }

        private void Start()
        {
            CreatureAnimator.OnBuild += delegate
            {
                Animator.SetBool("HasWings", HasWings);
                //Animator.SetBool("HasWings", HasWings);
                //Animator.SetBool("HasLegs", HasLegs && !HasWings);
            };
        }
    }
}