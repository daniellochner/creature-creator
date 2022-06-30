// Credit: https://www.youtube.com/watch?v=iasDPyC0QOg

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
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                HandleBuoyancy(other.transform.position.y);
            }
        }

        private void HandleBuoyancy(float waterHeight)
        {
            pointsUnderwater = 0;

            foreach (Transform point in floatingPoints)
            {
                float difference = point.position.y - waterHeight;

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