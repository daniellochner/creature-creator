// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureAnimator))]
    public class CreatureHolder : NetworkBehaviour
    {
        #region Fields
        private Dictionary<ArmAnimator, Held> held = new Dictionary<ArmAnimator, Held>();
        #endregion

        #region Properties
        public CreatureAnimator Animator { get; private set; }

        public Dictionary<ArmAnimator, Held> Held => held;

        public NetworkVariable<bool> IsHolding { get; set; } = new NetworkVariable<bool>(false); 
        #endregion

        #region Methods
        private void Awake()
        {
            Animator = GetComponent<CreatureAnimator>();
        }

        public void TryHold(Held holder)
        {
            HoldServerRpc(holder.NetworkObject);
        }
        [ServerRpc(RequireOwnership = false)]
        private void HoldServerRpc(NetworkObjectReference networkObjectRef)
        {
            if (networkObjectRef.TryGet(out NetworkObject networkObject))
            {
                Held holder = networkObject.GetComponent<Held>();

                if (!holder.IsHeld)
                {
                    ArmAnimator nearestArm = null;
                    float minDistance = Mathf.Infinity;

                    foreach (ArmAnimator arm in Animator.Arms)
                    {
                        if (held.ContainsKey(arm)) continue;

                        float d = Vector3.Distance(arm.LimbConstructor.Extremity.position, holder.transform.position);
                        if (d < minDistance)
                        {
                            minDistance = d;
                            nearestArm = arm;
                        }
                    }

                    if (nearestArm != null)
                    {
                        holder.Hand.Value = new Held.HeldData()
                        {
                            networkObjectId = NetworkObjectId,
                            armGUID = nearestArm.name
                        };
                        held[nearestArm] = holder;
                    }

                    IsHolding.Value = true;
                }
            }
        }

        public void DropAll()
        {
            DropAllServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void DropAllServerRpc()
        {
            foreach (Held h in held.Values)
            {
                if (h == null) continue;
                h.Hand.Value = default;
            }
            held.Clear();

            IsHolding.Value = false;
        }
        #endregion
    }
}