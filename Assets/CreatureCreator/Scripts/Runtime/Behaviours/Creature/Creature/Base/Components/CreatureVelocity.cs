using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(Animator))]
    public class CreatureVelocity : KinematicVelocity
    {
        [SerializeField] private float baseMovementSpeed;
        [SerializeField] private float baseTurnSpeed;

        public Animator Animator { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        private void Start()
        {
            OnLinearChanged += delegate (Vector3 linear)
            {
                float l = Mathf.Clamp01(Vector3.ProjectOnPlane(Linear, transform.up).magnitude / baseMovementSpeed);
                Animator.SetFloat("%LSpeed", l);
            };
            OnAngularChanged += delegate (Vector3 angular)
            {
                float a = Mathf.Clamp01(Mathf.Abs(Angular.y) / baseTurnSpeed);
                Animator.SetFloat("%ASpeed", a);
            };
        }
    }
}