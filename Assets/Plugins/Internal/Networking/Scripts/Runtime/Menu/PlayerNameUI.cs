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
        [Space]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button kickButton;
        [SerializeField] private GameObject hostGO;
        [SerializeField] private Image backgroundImage;

        private PlayerData playerData;
        #endregion

        #region Methods
        public void Setup(PlayerData playerData)
        {
            SetName(playerData.username);
            SetColour((NetworkManager.Singleton.LocalClientId == playerData.clientId) ? playerColour : nonPlayerColour);

            if (NetworkManager.ServerClientId == playerData.clientId)
            {
                hostGO.SetActive(true);
            }
            else if (NetworkManager.Singleton.IsHost)
            {
                kickButton.gameObject.SetActive(true);
                kickButton.onClick.AddListener(() => NetworkConnectionManager.Instance.Kick(playerData.clientId, "You were kicked by the host."));
            }

            this.playerData = playerData;
        }

        public void SetName(string name)
        {
            nameText.text = name;
        }
        public void SetColour(Color colour)
        {
            backgroundImage.color = colour;
        }
        #endregion
    }
}