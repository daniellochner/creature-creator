using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureGlider : MonoBehaviour
    {
        [SerializeField] private float airDrag;
        private Rigidbody rb;
        private CreatureAnimator creatureAnimator;

        private void Awake()
        {
            creatureAnimator = GetComponent<CreatureAnimator>();
            rb = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            rb.drag = (creatureAnimator.Wings.Count > 0 && !creatureAnimator.IsGrounded) ? airDrag : 0f;
        }
    }
}