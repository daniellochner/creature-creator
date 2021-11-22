// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
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
        [SerializeField] private RawImage backgroundImage;

        private PlayerData playerData;
        #endregion

        #region Methods
        public void Setup(PlayerData playerData)
        {
            backgroundImage.color = (NetworkManager.Singleton.LocalClientId == playerData.clientId) ? playerColour : nonPlayerColour;
            nameText.text = playerData.username.ToString();

            if (NetworkManager.Singleton.ServerClientId == playerData.clientId)
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

        public void SetCreatureName(string creatureName)
        {
            nameText.text = $"{playerData.username} ({creatureName})";
        }
        #endregion
    }
}