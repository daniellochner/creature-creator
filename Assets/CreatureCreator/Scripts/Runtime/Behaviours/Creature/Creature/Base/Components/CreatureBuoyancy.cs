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
            Refresh();
            buoyantObject.enabled = true;
        }
        private void OnDisable()
        {
            buoyantObject.enabled = false;
        }

        public void Refresh()
        {
            buoyantObject.floatingPower = rb.mass * factor;
        }
    }
}