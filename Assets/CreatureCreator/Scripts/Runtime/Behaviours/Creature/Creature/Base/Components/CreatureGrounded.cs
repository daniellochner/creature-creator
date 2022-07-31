using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureAnimator))]
    public class CreatureGrounded : MonoBehaviour
    {
        [SerializeField] private float contactDistance;

        public CreatureAnimator Animator { get; private set; }

        public bool IsGrounded
        {
            get; private set;
        }

        private void Awake()
        {
            Animator = GetComponent<CreatureAnimator>();
        }
        private void FixedUpdate()
        {
            IsGrounded = Physics.Raycast(transform.position + transform.up, -transform.up, 1f + contactDistance);
            Animator.Animator.SetBool("IsGrounded", IsGrounded);
        }
    }
}