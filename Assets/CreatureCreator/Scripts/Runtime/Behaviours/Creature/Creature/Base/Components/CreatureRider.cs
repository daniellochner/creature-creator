// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using UnityEngine;
using System;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureRider : NetworkBehaviour
    {
        #region Fields
        private List<CreatureRider> riders = new List<CreatureRider>();

        private ClientNetworkTransform clientNetworkTransform;
        private CreatureRider baseRider;
        #endregion

        #region Properties
        public CreatureConstructor Constructor { get; set; }
        public CreatureCollider Collider { get; set; }
        public CreatureAnimator Animator { get; set; }

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

            clientNetworkTransform = GetComponent<ClientNetworkTransform>();
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
                if (baseRider != null)
                {
                    SetPositionAndRotation(baseRider, Base.Value);
                }
                else
                if (IsServer)
                {
                    Dismount(); // Handle case where base rider disconnects
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
                foreach (CreatureRider rider in new List<CreatureRider>(riders))
                {
                    rider.Dismount();
                }
            }
            else
            if (IsRiding)
            {
                CreatureRider baseRider = GetRider(Base.Value.reference);
                if (baseRider != null)
                {
                    foreach (CreatureRider rider in baseRider.riders)
                    {
                        if (rider.Base.Value.height > Base.Value.height)
                        {
                            rider.Base.Value = new BaseData(rider.Base.Value, -Constructor.Dimensions.Height);
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

                SetPositionAndRotation(oldBaseRider, oldBase);

                Physics.IgnoreCollision(oldBaseRider.Collider.Hitbox, Collider.Hitbox, false);
            }

            if (isRiding)
            {
                baseRider = GetRider(newBase.reference);

                SetPositionAndRotation(baseRider, newBase);

                Physics.IgnoreCollision(baseRider.Collider.Hitbox, Collider.Hitbox, true);
            }
            else
            {
                baseRider = null;
            }

            if (IsLocalPlayer)
            {
                clientNetworkTransform.Teleport(transform.position, transform.rotation, transform.localScale);
                Constructor.Rigidbody.isKinematic = isRiding;
            }

            clientNetworkTransform.enabled = !isRiding;
            Animator.enabled = !isRiding && Constructor.Body.gameObject.activeSelf;
        }
        
        #region Helper
        private void SetPositionAndRotation(CreatureRider baseRider, BaseData baseData)
        {
            transform.position = baseRider.transform.position + (baseData.height * baseRider.transform.up);
            transform.rotation = baseRider.transform.rotation;
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
            public BaseData(BaseData data, float offset)
            {
                this.reference = data.reference;
                this.height = data.height + offset;
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