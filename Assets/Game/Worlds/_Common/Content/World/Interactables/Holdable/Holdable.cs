// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(Follower))]
    public class Holdable : CreatureInteractable
    {
        private Collider col;
        private Rigidbody rb;
        private Follower follower;

        private void Awake()
        {
            col = GetComponent<Collider>();
            rb = GetComponent<Rigidbody>();
            follower = GetComponent<Follower>();
        }

        public override bool CanInteract(Interactor interactor)
        {
            return base.CanInteract(interactor) && Player.Instance.PickUp.enabled && interactor.GetComponent<CreatureAnimator>().Arms.Count > 0;
        }

        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);
            Player.Instance.PickUp.PickUp(this);
        }

        public void PickUp(Transform hand)
        {
            follower.SetFollow(hand, true);
            rb.isKinematic = true;
            col.enabled = false;
        }
        public void Drop()
        {
            follower.follow = null;
            rb.isKinematic = false;
            col.enabled = true;
        }
    }
}