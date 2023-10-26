// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using UnityEngine;
using System;
using System.Collections;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureRider : NetworkBehaviour
    {
        #region Fields
        private List<CreatureRider> riders = new List<CreatureRider>();

        private ClientNetworkTransform clientNetworkTransform;
        #endregion

        #region Properties
        public CreatureConstructor Constructor { get; set; }
        public CreatureCollider Collider { get; set; }
        public CreatureAnimator Animator { get; set; }
        public CreatureMover Mover { get; set; }

        public NetworkVariable<BaseData> Base { get; set; } = new NetworkVariable<BaseData>();

        public bool IsRiding => Base.Value != null;
        public bool IsBase => riders.Count > 0;
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Collider = GetComponent<CreatureCollider>();
            Animator = GetComponent<CreatureAnimator>();
            Mover = GetComponent<CreatureMover>();

            clientNetworkTransform = GetComponent<ClientNetworkTransform>();
        }
        private void OnEnable()
        {
            NetworkManager.OnClientDisconnectCallback += OnClientDisconnected;
        }
        private void OnDisable()
        {
            NetworkManager.OnClientDisconnectCallback -= OnClientDisconnected;
        }
        private void Start()
        {
            Base.OnValueChanged += OnBaseChanged;

            if (Base.Value != null)
            {
                OnBaseChanged(null, Base.Value);
            }
        }
        private void Update()
        {
            if (IsLocalPlayer && InputUtility.GetKeyDown(KeybindingsManager.Data.Dismount))
            {
                Dismount();
            }
        }
        private void LateUpdate()
        {
            if (IsRiding)
            {
                if (Base.Value.reference.TryGet(out NetworkObject baseNetworkObj))
                {
                    SetPositionAndRotation(baseNetworkObj.transform, Base.Value.height);
                }
                else
                if (IsServer)
                {
                    Base.Value = null; // Handle case where base rider disconnects
                }
            }
        }

        public void Ride(CreatureRider rider)
        {
            RideServerRpc(new NetworkObjectReference(rider.NetworkObject));
        }
        [ServerRpc(RequireOwnership = false)]
        private void RideServerRpc(NetworkObjectReference baseNetObjRef)
        {
            if (IsRiding || IsBase) return;

            CreatureRider baseRider = GetRider(baseNetObjRef);

            // Base
            if (baseRider.Base.Value != null)
            {
                baseNetObjRef = baseRider.Base.Value.reference;
                baseRider = GetRider(baseNetObjRef);
            }

            // Height
            float height = baseRider.Constructor.Dimensions.Height;
            foreach (CreatureRider rider in baseRider.riders)
            {
                if (rider == null) continue;

                height += rider.Constructor.Dimensions.Height;
            }
            baseRider.riders.Add(this);

            Base.Value = new BaseData(baseNetObjRef, height);
        }

        public void Dismount()
        {
            DismountServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void DismountServerRpc()
        {
            if (IsBase)
            {
                foreach (CreatureRider rider in riders)
                {
                    if (rider == null) continue;

                    rider.Base.Value = null;
                }
                riders.Clear();
            }
            else
            if (IsRiding)
            {
                CreatureRider baseRider = GetRider(Base.Value.reference);
                if (baseRider != null)
                {
                    foreach (CreatureRider rider in baseRider.riders)
                    {
                        if (rider == null) continue;

                        if (rider.Base.Value.height > Base.Value.height)
                        {
                            rider.Base.Value.height -= Constructor.Dimensions.Height;
                        }
                    }
                    baseRider.riders.Remove(this);
                }

                Base.Value = null;
            }
        }

        private void OnBaseChanged(BaseData oldBase, BaseData newBase)
        {
            bool isRiding  = newBase != null;
            bool wasRiding = oldBase != null;

            if (wasRiding)
            {
                CreatureRider oldBaseRider = GetRider(oldBase.reference);

                SetPositionAndRotation(oldBaseRider.transform, oldBase.height);

                Physics.IgnoreCollision(oldBaseRider.Collider.Hitbox, Collider.Hitbox, false);
            }

            if (isRiding)
            {
                CreatureRider newBaseRider = GetRider(newBase.reference);

                SetPositionAndRotation(newBaseRider.transform, newBase.height);

                Physics.IgnoreCollision(newBaseRider.Collider.Hitbox, Collider.Hitbox, true);
            }

            if (IsLocalPlayer)
            {
                clientNetworkTransform.Teleport(transform.position, transform.rotation, transform.localScale);
                Constructor.Rigidbody.isKinematic = isRiding;
                Mover?.StopMoving();
            }

            clientNetworkTransform.enabled = !isRiding;
            Animator.enabled = !isRiding && Constructor.Body.gameObject.activeSelf;
        }
        private void OnClientDisconnected(ulong clientId)
        {
            if (IsServer && IsBase)
            {
                int numRemoved = riders.RemoveAll(x => x == null);
                if (numRemoved > 0)
                {
                    float height = Constructor.Dimensions.Height;
                    foreach (CreatureRider rider in riders)
                    {
                        rider.Base.Value.height = height;
                        height += rider.Constructor.Dimensions.Height;
                    }
                }
            }
        }

        #region Helper
        private void SetPositionAndRotation(Transform baseT, float height)
        {
            transform.position = baseT.position + (height * baseT.up);
            transform.rotation = baseT.rotation;
        }

        private CreatureRider GetRider(NetworkObjectReference reference)
        {
            if (reference.TryGet(out NetworkObject networkObject))
            {
                return networkObject.GetComponent<CreatureRider>();
            }
            return null;
        }
        #endregion
        #endregion

        #region Nested
        public class BaseData : INetworkSerializable, IEquatable<BaseData>
        {
            public NetworkObjectReference reference;
            public float height;

            public BaseData()
            {
            }
            public BaseData(NetworkObjectReference reference, float height)
            {
                this.reference = reference;
                this.height = height;
            }

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref reference);
                serializer.SerializeValue(ref height);
            }

            public bool Equals(BaseData other)
            {
                return (other.reference.NetworkObjectId == reference.NetworkObjectId) && (other.height == height);
            }
        }
        #endregion
    }
}