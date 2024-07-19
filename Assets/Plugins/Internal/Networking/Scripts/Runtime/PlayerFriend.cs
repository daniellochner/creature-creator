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
        private PlayerDataContainer DataContainer { get; set; }

        private MinimapIcon MinimapIcon { get; set; }
        private PlayerNamer Namer { get; set; }
        private PlayerDeathMessenger DeathMessenger { get; set; }
		
		private PlayerNameUI NameUI => NetworkPlayersMenu.Instance?.GetPlayerNameUI(OwnerClientId);
        #endregion

        #region Methods
        private void Awake()
        {
            DataContainer = GetComponent<PlayerDataContainer>();

            MinimapIcon = GetComponent<MinimapIcon>();
            Namer = GetComponent<PlayerNamer>();
            DeathMessenger = GetComponent<PlayerDeathMessenger>();
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
            MinimapIcon.MinimapIconUI?.SetColour(friendColour.Alpha(0.8f));

            Namer?.SetColour(friendColour);
            DeathMessenger?.SetColour(friendColour);

            NameUI?.SetFriend(true);
        }

        private void OnConfirmFriendRequest(PlayerData playerData)
        {
            if (playerData != null && OwnerClientId == playerData.clientId)
            {
                SetAsFriend();
            }
        }
        #endregion
    }
}