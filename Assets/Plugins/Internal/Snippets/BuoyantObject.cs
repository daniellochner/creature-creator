// Credit: https://www.youtube.com/watch?v=iasDPyC0QOg

using Pinwheel.Poseidon;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(Rigidbody))]
    public class BuoyantObject : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Transform[] floatingPoints;
        [SerializeField] private float floatingPower;
        [Space]
        [SerializeField] private float underwaterAngularDrag;
        [SerializeField] private float underwaterDrag;
        [SerializeField] private float airAngularDrag;
        [SerializeField] private float airDrag;
        [Header("Debug")]
        [SerializeField, ReadOnly] private bool isUnderwater;
        [SerializeField, ReadOnly] private int pointsUnderwater;

        private Rigidbody rb;
        private PWater water;
        #endregion

        #region Properties
        private bool IsUnderwater
        {
            get => isUnderwater;
            set
            {
                isUnderwater = value;

                rb.drag = isUnderwater ? underwaterDrag : airDrag;
                rb.angularDrag = isUnderwater ? underwaterAngularDrag : airAngularDrag;
            }
        }
        #endregion

        #region Methods
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            if (water != null)
            {
                HandleBuoyancy();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                water = other.GetComponent<PWater>();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                water = null;
            }
        }
        
        private void HandleBuoyancy()
        {
            pointsUnderwater = 0;

            foreach (Transform point in floatingPoints)
            {
                Vector3 localPointPos = water.transform.InverseTransformPoint(point.position);
                localPointPos.y = 0;
                Vector3 worldWaterPos = water.transform.TransformPoint(water.GetLocalVertexPosition(localPointPos, true));

                float difference = point.position.y - worldWaterPos.y;

                if (difference < 0)
                {
                    rb.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), point.position, ForceMode.Force);
                    if (!IsUnderwater)
                    {
                        IsUnderwater = true;
                    }
                    pointsUnderwater++;
                }
            }
            
            if (isUnderwater && pointsUnderwater == 0)
            {
                IsUnderwater = false;
            }
        }
        #endregion
    }
}