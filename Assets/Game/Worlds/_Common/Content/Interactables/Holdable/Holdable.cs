// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(Holder))]
    public class Holdable : CreatureInteractable
    {
        #region Fields
        private Holder holder;
        #endregion

        #region Properties
        public HoldableDummy Dummy => holder.Dummy;

        public bool IsHeld => holder.IsHeld;
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            holder = GetComponent<Holder>();
        }
        public override bool CanInteract(Interactor interactor)
        {
            return base.CanInteract(interactor) && !EditorManager.Instance.IsEditing && Player.Instance.Holder.enabled && interactor.GetComponent<CreatureAnimator>().Arms.Count > 0;
        }
        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);
            Player.Instance.Holder.TryHold(holder);
        }
        
        public void Hold(LimbConstructor arm)
        {
            holder.Hand.Value = arm.name;
        }
        public void Drop()
        {
            holder.Hand.Value = "";
        }
        #endregion
    }
}