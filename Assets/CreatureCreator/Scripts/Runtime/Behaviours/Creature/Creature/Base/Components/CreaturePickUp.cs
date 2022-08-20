// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureAnimator))]
    public class CreaturePickUp : NetworkBehaviour
    {
        private Dictionary<ArmAnimator, Holdable> held = new Dictionary<ArmAnimator, Holdable>();

        public CreatureAnimator Animator { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<CreatureAnimator>();
        }
        
        private void OnDisable()
        {
            Debug.Log("DISABLED?");
            DropAll();
        }

        public void PickUp(Holdable holdable)
        {
            PickUpServerRpc(holdable.NetworkObject);
        }
        [ServerRpc(RequireOwnership = false)]
        private void PickUpServerRpc(NetworkObjectReference networkObjectRef)
        {
            Holdable holdable = null;
            if (networkObjectRef.TryGet(out NetworkObject networkObject))
            {
                holdable = networkObject.GetComponent<Holdable>();
            }
            else
            {
                return;
            }
            
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

            Debug.Log(nearestArm);
            
            if (nearestArm != null)
            {
                holdable.PickUp(nearestArm.LimbConstructor.Extremity);
                held[nearestArm] = holdable;
            }
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
        }
    }
}