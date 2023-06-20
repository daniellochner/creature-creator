using System;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Held : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private HoldableDummy dummyPrefab;

        private NetworkTransform networkTransform;
        private Rigidbody rigidBody;
        private Outline outline;

        private Vector3 startPosition;
        private Quaternion startRotation;
        #endregion

        #region Properties
        public NetworkVariable<HeldData> Hand { get; set; } = new NetworkVariable<HeldData>();

        public Holdable Holdable { get; private set; }
        public HoldableDummy Dummy { get; private set; }

        public bool IsHeld => !Hand.Value.armGUID.IsEmpty;
        #endregion

        #region Methods
        private void Awake()
        {
            networkTransform = GetComponent<NetworkTransform>();
            rigidBody = GetComponent<Rigidbody>();
            outline = GetComponent<Outline>();

            Holdable = GetComponent<Holdable>();
        }
        private void Start()
        {
            startPosition = transform.position;
            startRotation = transform.rotation;

            Hand.OnValueChanged += OnHandChanged;

            if (IsHeld)
            {
                OnHandChanged(default, Hand.Value);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsServer && collision.collider.CompareTag("WorldBorder"))
            {
                networkTransform.Teleport(startPosition, startRotation, transform.localScale);
                rigidBody.velocity = rigidBody.angularVelocity = Vector3.zero;
            }
        }

        public void OnHandChanged(HeldData oHD, HeldData nHD)
        {
            bool isHeld = !nHD.armGUID.IsEmpty;
            if (isHeld)
            {
                if (outline != null)
                {
                    outline.enabled = false;
                }

                Dummy = Instantiate(dummyPrefab);
                Dummy.Setup(this, nHD);
            }
            else
            {
                if (IsServer)
                {
                    GetComponent<NetworkTransform>().Teleport(Dummy.transform.position, Dummy.transform.rotation, Dummy.transform.localScale);
                }
                Destroy(Dummy.gameObject);
            }
            gameObject.SetActive(!isHeld);
        }
        #endregion

        #region Nested
        public struct HeldData : INetworkSerializable, IEquatable<HeldData>
        {
            public ulong networkObjectId;
            public FixedString64Bytes armGUID;

            public bool Equals(HeldData other)
            {
                return networkObjectId.Equals(other.networkObjectId) && armGUID.Equals(other.armGUID);
            }

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref networkObjectId);
                serializer.SerializeValue(ref armGUID);
            }
        }
        #endregion
    }
}