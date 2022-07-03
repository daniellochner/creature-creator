using UnityEngine;

namespace DanielLochner.Assets
{
    public class KinematicVelocity : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool useThisTransform = true;
        [SerializeField, DrawIf("useThisTransform", false)] private Transform source;
        [SerializeField, DrawIf("source", null)] private Transform sourcePosition;
        [SerializeField, DrawIf("source", null)] private Transform sourceRotation;
        [SerializeField] private float posThreshold;
        [SerializeField] private float rotThreshold;

        private Vector3? prevPosition = null;
        private Quaternion? prevRotation = null;
        #endregion

        #region Properties
        public Vector3 Linear
        {
            get; private set;
        }
        public Vector3 Angular
        {
            get; private set;
        }
        #endregion

        #region Methods
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (useThisTransform) { source = transform; }

            if (source != null) { sourcePosition = sourceRotation = source; }
        }
#endif
        private void Update()
        {
            if (sourcePosition)
            {
                if (prevPosition != null)
                {
                    Vector3 deltaPosition = sourcePosition.position - (Vector3)prevPosition;
                    if (deltaPosition.sqrMagnitude > posThreshold * posThreshold)
                    {
                        Linear = deltaPosition / Time.fixedDeltaTime;
                    }
                    else
                    {
                        Linear = Vector3.zero;
                    }
                }
                prevPosition = sourcePosition.position;
            }
            if (sourceRotation)
            {
                if (prevRotation != null)
                {
                    Quaternion deltaRotation = sourceRotation.rotation * Quaternion.Inverse((Quaternion)prevRotation);
                    deltaRotation.ToAngleAxis(out float deltaAngle, out var axis);
                    if (deltaAngle > rotThreshold)
                    {
                        Angular = (deltaAngle / Time.fixedDeltaTime) * axis;
                    }
                    else
                    {
                        Angular = Vector3.zero;
                    }
                }
                prevRotation = sourceRotation.rotation;
            }
        }
        public void Reset()
        {
            prevPosition = null;
            prevRotation = null;

            Linear = Angular = Vector3.zero;
        }
        #endregion
    }
}