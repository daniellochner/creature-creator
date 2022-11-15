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
        private Dictionary<ArmAnimator, Holdable> held = new Dictionary<ArmAnimator, Holdable>();
        #endregion

        #region Properties
        public CreatureAnimator Animator { get; private set; }

        public Dictionary<ArmAnimator, Holdable> Held => held;

        public bool IsHolding { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Animator = GetComponent<CreatureAnimator>();
        }

        public void TryHold(Holdable holdable)
        {
            HoldServerRpc(holdable.NetworkObject);
        }
        [ServerRpc(RequireOwnership = false)]
        private void HoldServerRpc(NetworkObjectReference networkObjectRef)
        {
            Holdable holdable = null;
            if (networkObjectRef.TryGet(out NetworkObject networkObject))
            {
                holdable = networkObject.GetComponent<Holdable>();

                if (!holdable.IsHeld)
                {
                    ArmAnimator nearestArm = null;
                    float minDistance = Mathf.Infinity;

                    foreach (ArmAnimator arm in Animator.Arms)
                    {
                        if (held.ContainsKey(arm)) continue;

                        float d = Vector3.Distance(arm.LimbConstructor.Extremity.position, holdable.transform.position);
                        if (d < minDistance)
                        {
                            minDistance = d;
                            nearestArm = arm;
                        }
                    }

                    if (nearestArm != null)
                    {
                        holdable.Hold(nearestArm.LimbConstructor);
                        held[nearestArm] = holdable;
                    }

                    HoldClientRpc();
                }
            }
        }
        [ClientRpc]
        private void HoldClientRpc()
        {
            IsHolding = true;
        }

        public void DropAll()
        {
            DropAllServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void DropAllServerRpc()
        {
            foreach (Holdable h in held.Values)
            {
                if (h == null) continue;
                h.Drop();
            }
            held.Clear();

            DropClientRpc();
        }
        [ClientRpc]
        private void DropClientRpc()
        {
            IsHolding = false;
        }
        #endregion
    }
}