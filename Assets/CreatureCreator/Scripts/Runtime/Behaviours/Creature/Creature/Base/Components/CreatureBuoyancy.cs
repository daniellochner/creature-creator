using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureBuoyancy : MonoBehaviour
    {
        [SerializeField] private float factor;

        private BuoyantObject buoyantObject;
        private Rigidbody rb;

        private void Awake()
        {
            buoyantObject = GetComponent<BuoyantObject>();
            rb = GetComponent<Rigidbody>();
        }

        public void OnEnable()
        {
            buoyantObject.floatingPower = rb.mass * factor;
            buoyantObject.enabled = true;
        }
        private void OnDisable()
        {
            buoyantObject.enabled = false;
        }
    }
}