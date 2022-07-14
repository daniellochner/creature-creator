using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureAnimator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private float minSlitherLength;

        private CreatureAnimator Animator { get; set; }

        private bool HasArms => Animator.Arms.Count != 0;
        private bool HasLegs => Animator.Legs.Count != 0;
        private bool HasWings => Animator.Wings.Count != 0;
        private bool HasMouths => Animator.Mouths.Count != 0;

        private void Awake()
        {
            Animator = GetComponent<CreatureAnimator>();
        }

        private void Start()
        {
            Animator.OnBuild += delegate
            {
                Animator.SetBool("Body_Fly", HasWings);
                Animator.SetBool("Body_Slither", !HasLegs && !HasWings && (Animator.Constructor.Dimensions.body.length >= minSlitherLength));
                Animator.SetBool("Body_Walk", HasLegs && !HasWings);
            };
        }
    }
}