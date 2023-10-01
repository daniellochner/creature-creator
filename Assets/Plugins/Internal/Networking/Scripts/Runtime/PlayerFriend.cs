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
        private PlayerDataContainer DataContainer { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            MinimapIcon = GetComponent<MinimapIcon>();
            DataContainer = GetComponent<PlayerDataContainer>();
        }

        public void Setup()
        {
            SetFriend(FriendsManager.Instance.IsFriended(DataContainer.PlayerData.Value.playerId));

            NetworkPlayersManager.Instance.OnConfirmFriendRequest += OnConfirmFriendRequest;
        }

        public void SetFriend(bool isFriend)
        {
            MinimapIcon.MinimapIconUI.SetColour(isFriend ? friendColour : nonFriendColour);
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