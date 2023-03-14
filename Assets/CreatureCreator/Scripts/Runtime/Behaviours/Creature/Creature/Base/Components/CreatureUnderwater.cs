using Pinwheel.Poseidon;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureUnderwater : MonoBehaviour
    {
        private bool isUnderwater;
        private Water water;

        public bool IsOnRaft { get; set; }

        public bool IsUnderwater
        {
            get => isUnderwater;
            set
            {
                isUnderwater = value;
                Animator.SetBool("IsUnderwater", isUnderwater);
            }
        }
        public Animator Animator { get; private set; }
        public CreatureConstructor Constructor { get; private set; }
        public CreatureCollider Collider { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Constructor = GetComponent<CreatureConstructor>();
            Collider = GetComponent<CreatureCollider>();
        }
        private void OnDisable()
        {
            IsOnRaft = false;
        }
        private void FixedUpdate()
        {
            if (water != null)
            {
                IsUnderwater = water.Collider.bounds.Contains(Constructor.Body.position);
            }
            else
            {
                IsUnderwater = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Raft"))
            {
                IsOnRaft = true;
            }

            if (other.CompareTag("Water"))
            {
                water = other.GetComponent<Water>();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (water != null)
            {
                if (IsOnRaft && other.CompareTag("Raft"))
                {
                    IsOnRaft = false;
                    water.OnTriggerEnter(Collider.Capsule);
                }

                if (other.CompareTag("Water"))
                {
                    water = null;
                }
            }

            if (other.CompareTag("Raft"))
            {
                IsOnRaft = false;
            }
        }
    }
}