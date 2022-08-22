// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(Follower))]
    public class Holdable : CreatureInteractable
    {
        #region Fields
        private Follower follower;
        private Collider col;
        private Rigidbody rb;
        #endregion

        #region Methods
        private void Awake()
        {
            col = GetComponent<Collider>();
            rb = GetComponent<Rigidbody>();
            follower = GetComponent<Follower>();
        }

        public override bool CanInteract(Interactor interactor)
        {
            return base.CanInteract(interactor) && !EditorManager.Instance.IsEditing && Player.Instance.Holder.enabled && interactor.GetComponent<CreatureAnimator>().Arms.Count > 0;
        }
        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);
            Player.Instance.Holder.Hold(this);
        }

        public void PickUp(Transform hand)
        {
            follower.SetFollow(hand, true);
            rb.isKinematic = true;
            ToggleColliderClientRpc(false);
        }
        public void Drop()
        {
            follower.follow = null;
            rb.isKinematic = false;
            ToggleColliderClientRpc(true);
        }

        [ClientRpc]
        public void ToggleColliderClientRpc(bool isEnabled)
        {
            col.enabled = isEnabled;
        }
        #endregion
    }
}