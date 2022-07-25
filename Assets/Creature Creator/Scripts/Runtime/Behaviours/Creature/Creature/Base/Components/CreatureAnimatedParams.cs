using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(KinematicVelocity))]
    [RequireComponent(typeof(CreatureAnimator))]
    [RequireComponent(typeof(Animator))]
    public class CreatureAnimatedParams : MonoBehaviour
    {
        [SerializeField] private float baseMovementSpeed;
        [SerializeField] private float baseTurnSpeed;
        [SerializeField] private float contactDistance;

        public CreatureAnimator CreatureAnimator { get; private set; }
        public KinematicVelocity Velocity { get; private set; }
        public Animator Animator { get; private set; }

        public bool IsGrounded
        {
            get; private set;
        }

        private void Awake()
        {
            CreatureAnimator = GetComponent<CreatureAnimator>();
            Velocity = GetComponent<KinematicVelocity>();
            Animator = GetComponent<Animator>();
        }
        private void FixedUpdate()
        {
            if (!CreatureAnimator.IsAnimated) return;

            IsGrounded = Physics.Raycast(transform.position + transform.up, -transform.up, 1f + contactDistance);
            Animator.SetBool("IsGrounded", IsGrounded);

            float l = Mathf.Clamp01(Vector3.ProjectOnPlane(Velocity.Linear, transform.up).magnitude / baseMovementSpeed);
            float a = Mathf.Clamp01(Mathf.Abs(Velocity.Angular.y) / baseTurnSpeed);
            Animator.SetFloat("%LSpeed", l);
            Animator.SetFloat("%ASpeed", a);
        }
    }
}