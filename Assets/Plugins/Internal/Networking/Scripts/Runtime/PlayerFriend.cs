using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class PlayerFriend : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Color friendColour;
        #endregion

        #region Properties
        private PlayerRecolour Recolour { get; set; }
        private PlayerDataContainer DataContainer { get; set; }
		
		private PlayerNameUI NameUI => NetworkPlayersMenu.Instance?.GetPlayerNameUI(OwnerClientId);
        #endregion

        #region Methods
        private void Awake()
        {
            Recolour = GetComponent<PlayerRecolour>();
            DataContainer = GetComponent<PlayerDataContainer>();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            if (NetworkPlayersManager.Instance)
            {
                NetworkPlayersManager.Instance.OnConfirmFriendRequest -= OnConfirmFriendRequest;
            }
        }

        public void Setup()
        {
            if (FriendsManager.Instance.IsFriended(DataContainer.Data.playerId))
            {
                SetAsFriend();
            }
            else
            {
                NetworkPlayersManager.Instance.OnConfirmFriendRequest += OnConfirmFriendRequest;
            }
        }

        public void SetAsFriend()
        {
            Recolour.SetColour(friendColour);

            NameUI?.SetFriend(true);
        }

        private void OnConfirmFriendRequest(PlayerData playerData)
        {
            if (OwnerClientId == playerData.clientId)
            {
                SetAsFriend();
            }
        }
        #endregion
    }
}