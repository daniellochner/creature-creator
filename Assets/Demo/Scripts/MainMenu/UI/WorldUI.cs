using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class WorldUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI playersText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI mapVersionText;
        [SerializeField] private GameObject padlockIcon;
        [SerializeField] private Button joinButton;
        #endregion

        #region Methods
        public void Setup(MultiplayerUI multiplayerUI, Lobby lobby)
        {
            int players = lobby.Players.Count;
            int maxPlayers = lobby.MaxPlayers;
            string mapName = lobby.Data["map"].Value;
            string version = lobby.Data["version"].Value;
            string joinCode = lobby.Data["joinCode"].Value;
            bool isPasswordProtected = bool.Parse(lobby.Data["isPasswordProtected"].Value);

            playersText.text = $"{players}/{maxPlayers}";
            nameText.text = lobby.Name;
            mapVersionText.text = $"{mapName} ({version})";
            padlockIcon.SetActive(isPasswordProtected);
            joinButton.onClick.AddListener(delegate 
            {
                if (isPasswordProtected)
                {
                    InputDialog.Input("Password Required", "Enter the password...", submitEvent: delegate (string password)
                    {
                        multiplayerUI.Join(lobby.LobbyCode, joinCode, password);
                    });
                }
                else
                {
                    multiplayerUI.Join(lobby.LobbyCode, joinCode);
                }
            });
        }
        #endregion
    }
}