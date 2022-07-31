using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(Animator))]
    public class CreatureUnderwater : MonoBehaviour
    {
        private bool isUnderwater;

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

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                IsUnderwater = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                IsUnderwater = false;
            }
        }
    }
}