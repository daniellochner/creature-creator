// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(Held))]
    public class Holdable : CreatureInteractable
    {
        #region Properties
        public Held Held { get; private set; }
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            Held = GetComponent<Held>();
        }
        public override bool CanInteract(Interactor interactor)
        {
            return base.CanInteract(interactor) && Held.CanHold.Value && !Held.IsHeld && !EditorManager.Instance.IsEditing && Player.Instance.Holder.enabled && Player.Instance.Animator.Arms.Count > 0;
        }
        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);
            Player.Instance.Holder.TryHold(Held);
        }
        #endregion
    }
}