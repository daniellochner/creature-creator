// Network Transform
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkTransform : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private int sendTickRate = 20;
        [SerializeField] private bool useInterpolation = true;

        [Header("Source/Target")]
        [SerializeField] private bool useThisTransform = false;
        [SerializeField, DrawIf("useThisTransform", false)] private Transform source;
        [SerializeField, DrawIf("source", null)] private Transform sourcePosition;
        [SerializeField, DrawIf("source", null)] private Transform sourceRotation;
        [SerializeField, DrawIf("useThisTransform", false)] private Transform target;
        [SerializeField, DrawIf("target", null)] private Transform targetPosition;
        [SerializeField, DrawIf("target", null)] private Transform targetRotation;

        [Header("Network Variables")]
        [SerializeField, ReadOnly] private NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();
        [SerializeField, ReadOnly] private NetworkVariable<Quaternion> rotation = new NetworkVariable<Quaternion>();

        private NetworkObject networkObject;
        #endregion

        #region Properties
        public Transform SourcePosition => sourcePosition;
        public Transform TargetPosition => targetPosition;
        public Transform SourceRotation => sourceRotation;
        public Transform TargetRotation => targetRotation;

        public bool Capture { get; set; } = true;
        #endregion

        #region Methods
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (useThisTransform) { source = target = transform; }

            if (source != null) { sourcePosition = sourceRotation = source; }
            if (target != null) { targetPosition = targetRotation = target; }
        }
#endif
        public override void OnNetworkSpawn()
        {
            networkObject = GetComponent<NetworkObject>();

            if (IsOwner)
            {
                InvokeRepeating("CapturePositionAndRotation", 0, 1f / sendTickRate);
            }
            else
            {
                position.OnValueChanged += UpdatePosition;
                rotation.OnValueChanged += UpdateRotation;

                targetPosition.position = position.Value;
                targetRotation.rotation = rotation.Value;
            }
        }

        [ServerRpc]
        private void CapturePositionAndRotationServerRpc(Vector3 p, Quaternion r)
        {
            position.Value = p;
            rotation.Value = r;
        }
        private void CapturePositionAndRotation()
        {
            if (Capture)
            {
                CapturePositionAndRotationServerRpc(sourcePosition.position, sourceRotation.rotation);
            }
        }
        
        private void UpdatePosition(Vector3 previous, Vector3 target)
        {
            if (useInterpolation)
            {
                StartCoroutine(LerpRoutine(previous, target, 1f / sendTickRate, LerpPosition));
            }
            else
            {
                targetPosition.position = target;
            }
        }
        private void UpdateRotation(Quaternion previous, Quaternion target)
        {
            if (useInterpolation)
            {
                StartCoroutine(LerpRoutine(previous, target, 1f / sendTickRate, LerpRotation));
            }
            else
            {
                targetRotation.rotation = target;
            }
        }

        private void LerpPosition(Vector3 previous, Vector3 target, float progress)
        {
            targetPosition.position = Vector3.Lerp(previous, target, progress);
        }
        private void LerpRotation(Quaternion previous, Quaternion target, float progress)
        {
            targetRotation.rotation = Quaternion.Slerp(previous, target, progress);
        }

        private IEnumerator LerpRoutine<T>(T previous, T target, float timeToInterpolate, UnityAction<T, T, float> interpolation)
        {
            float timeElapsed = 0f;
            while (timeElapsed < timeToInterpolate)
            {
                interpolation(previous, target, timeElapsed / timeToInterpolate);
                timeElapsed += Time.deltaTime;

                yield return null;
            }
        }
        #endregion
    }
}