using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureRideable : CreatureInteractable
    {
        #region Properties
        private CreatureRider Rider { get; set; }
        private PlayerDataContainer DataContainer { get; set; }
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();

            Rider = GetComponent<CreatureRider>();
            DataContainer = GetComponent<PlayerDataContainer>();
        }

        public override bool CanInteract(Interactor interactor)
        {
            return base.CanInteract(interactor) && !EditorManager.Instance.IsEditing && !Player.Instance.Rider.IsRiding && !Player.Instance.Rider.IsBase && MinigameManager.Instance.CurrentMinigame == null && FriendsManager.Instance.IsFriended(DataContainer.PlayerData.Value.playerId);
        }
        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);

            Player.Instance.Rider.Ride(Rider);
        }
        #endregion
    }
}