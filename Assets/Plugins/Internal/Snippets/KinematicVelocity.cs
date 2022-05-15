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
        private void FixedUpdate()
        {
            if (sourcePosition)
            {
                if (prevPosition != null)
                {
                    Vector3 deltaPosition = sourcePosition.position - (Vector3)prevPosition;
                    Linear = deltaPosition / Time.fixedDeltaTime;
                }
                prevPosition = sourcePosition.position;
            }
            if (sourceRotation)
            {
                if (prevRotation != null)
                {
                    Quaternion deltaRotation = sourceRotation.rotation * Quaternion.Inverse((Quaternion)prevRotation);
                    deltaRotation.ToAngleAxis(out float angle, out var axis);
                    Angular = (angle / Time.fixedDeltaTime) * axis;
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