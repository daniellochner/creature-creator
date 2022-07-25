using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureAnimator))]
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatedParams : MonoBehaviour
    {
        [SerializeField] private float minSlitherLength;

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
                Animator.SetBool("Body_Fly", HasWings);
                Animator.SetBool("Body_Slither", !HasLegs && !HasWings && (CreatureAnimator.Constructor.Dimensions.body.length >= minSlitherLength));
                Animator.SetBool("Body_Walk", HasLegs && !HasWings);
            };
        }
    }
}