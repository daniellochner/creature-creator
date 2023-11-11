using Unity.Netcode;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class PlayerNameUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Color playerColour;
        [SerializeField] private Color nonPlayerColour;
        [SerializeField] private Color friendColour;
        [Space]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private GameObject hostGO;
        [SerializeField] private Button friendButton;
        [SerializeField] private Button kickButton;

        private PlayerData playerData;
        #endregion

        #region Methods
        public void Setup(PlayerData playerData)
        {
            this.playerData = playerData;

            SetName(playerData.username);
            if (NetworkUtils.IsPlayer(playerData.clientId))
            {
                SetColour(playerColour);
            }
            else
            {
                SetColour(nonPlayerColour);
                SetFriend(FriendsManager.Instance.IsFriended(playerData.playerId));
            }

            if (NetworkManager.ServerClientId == playerData.clientId)
            {
                hostGO.SetActive(true);
            }
            else if (NetworkManager.Singleton.IsHost)
            {
                kickButton.gameObject.SetActive(true);
            }
        }

        public void SetName(string name)
        {
            nameText.text = name.NoParse();
        }
        public void SetColour(Color colour)
        {
            backgroundImage.color = colour;
        }
        public void SetFriend(bool isFriend)
        {
            nameText.color = isFriend ? friendColour : Color.white;
            friendButton.enabled = !isFriend;
        }

        public void SendFriendRequest()
        {
            if (!FriendsManager.Instance.IsFriended(playerData.playerId))
            {
                ConfirmationDialog.Confirm(LocalizationUtility.Localize("friends_request_title"), LocalizationUtility.Localize("friends_request_message", playerData.username), onYes: delegate
                {
                    FriendsManager.Instance.SendFriendRequest(playerData.playerId);
                    NetworkPlayersManager.Instance.SendFriendRequest(playerData);
                });
            }
        }
        public void Kick()
        {
            InputDialog.Input(LocalizationUtility.Localize("kick_title", playerData.username), placeholder: LocalizationUtility.Localize("kick_input"), onSubmit: delegate (string reason)
            {
                NetworkConnectionManager.Instance.Kick(playerData.clientId, playerData.playerId, reason);
            });
        }
        #endregion
    }
}