using UnityEngine;

namespace DanielLochner.Assets
{
    public class PlayerFriend : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Color friendColour;
        [SerializeField] private Color nonFriendColour;
        #endregion

        #region Properties
        private MinimapIcon MinimapIcon { get; set; }
        private PlayerNamer Namer { get; set; }
        private PlayerDeathMessenger DeathMessenger { get; set; }
        private PlayerDataContainer DataContainer { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            MinimapIcon = GetComponent<MinimapIcon>();
            Namer = GetComponent<PlayerNamer>();
            DeathMessenger = GetComponent<PlayerDeathMessenger>();
            DataContainer = GetComponent<PlayerDataContainer>();
        }
        private void OnDestroy()
        {
            if (NetworkPlayersManager.Instance)
            {
                NetworkPlayersManager.Instance.OnConfirmFriendRequest -= OnConfirmFriendRequest;
            }
        }

        public void Setup()
        {
            if (FriendsManager.Instance.IsFriended(DataContainer.PlayerData.Value.playerId))
            {
                SetFriend(true);
            }
            else
            {
                NetworkPlayersManager.Instance.OnConfirmFriendRequest += OnConfirmFriendRequest;
            }
        }

        public void SetFriend(bool isFriend)
        {
            Color colour = isFriend ? friendColour : nonFriendColour;
            MinimapIcon.MinimapIconUI.SetColour(colour);

            if (isFriend)
            {
                Namer.SetColour(friendColour);
                DeathMessenger.SetColour(friendColour);
            }
        }

        private void OnConfirmFriendRequest(PlayerData playerData)
        {
            if (DataContainer.PlayerData.Value.clientId == playerData.clientId)
            {
                SetFriend(true);
            }
        }
        #endregion
    }
}