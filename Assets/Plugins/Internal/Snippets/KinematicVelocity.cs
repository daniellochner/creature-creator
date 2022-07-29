using System;
using System.Collections;
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
        [SerializeField] private float updateTime;

        private Vector3? prevPosition = null;
        private Quaternion? prevRotation = null;
        private Vector3 angular, linear;
        #endregion

        #region Properties
        public Vector3 Linear
        {
            get => linear;
            set
            {
                linear = value;
                OnLinearChanged?.Invoke(linear);
            }
        }
        public Vector3 Angular
        {
            get => angular;
            set
            {
                angular = value;
                OnAngularChanged?.Invoke(angular);
            }
        }

        public Action<Vector3> OnLinearChanged { get; set; }
        public Action<Vector3> OnAngularChanged { get; set; }
        #endregion

        #region Methods
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (useThisTransform) { source = transform; }

            if (source != null) { sourcePosition = sourceRotation = source; }
        }
#endif
        private void OnEnable()
        {
            InvokeRepeating("Calculate", 0f, updateTime);
        }

        public void Calculate()
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