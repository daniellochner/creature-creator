using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class KinematicVelocity : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool useFixedUpdate;
        [SerializeField] private bool useThisTransform = true;
        [SerializeField, DrawIf("useThisTransform", false)] private Transform source;
        [SerializeField, DrawIf("source", null)] private Transform sourcePosition;
        [SerializeField, DrawIf("source", null)] private Transform sourceRotation;
        [SerializeField] private float posThreshold;
        [SerializeField] private float rotThreshold;
        [SerializeField] private float alpha = 0.75f;

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
                //linear = (alpha * value) + ((1f - alpha) * linear);
                OnLinearChanged?.Invoke(linear);
            }
        }
        public Vector3 Angular
        {
            get => angular;
            set
            {
                angular = value;
                //angular = (alpha * value) + ((1f - alpha) * angular);
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
        private void OnDisable()
        {
            Reset();
        }
        private void Awake()
        {
            angular = linear = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (useFixedUpdate){
                Calculate(Time.fixedDeltaTime);
            }
        }
        private void Update()
        {
            if (!useFixedUpdate){
                Calculate(Time.deltaTime);
            }
        }

        public void Calculate(float deltaTime)
        {
            if (sourcePosition)
            {
                if (prevPosition != null)
                {
                    Vector3 deltaPosition = sourcePosition.localPosition - (Vector3)prevPosition;
                    if (deltaPosition.sqrMagnitude > posThreshold * posThreshold)
                    {
                        Linear = deltaPosition / deltaTime;
                    }
                    else
                    {
                        Linear = Vector3.zero;
                    }
                }
                prevPosition = sourcePosition.localPosition;
            }
            if (sourceRotation)
            {
                if (prevRotation != null)
                {
                    Quaternion deltaRotation = sourceRotation.localRotation * Quaternion.Inverse((Quaternion)prevRotation);
                    deltaRotation.ToAngleAxis(out float deltaAngle, out var axis);
                    if (deltaAngle > rotThreshold)
                    {
                        Angular = (deltaAngle / deltaTime) * axis;
                    }
                    else
                    {
                        Angular = Vector3.zero;
                    }
                }
                prevRotation = sourceRotation.localRotation;
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