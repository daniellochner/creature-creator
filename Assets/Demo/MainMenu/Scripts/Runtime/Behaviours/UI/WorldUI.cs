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
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private GameObject padlockIcon;
        [SerializeField] private Button joinButton;
        #endregion

        #region Properties
        public Button JoinButton => joinButton;
        #endregion

        #region Methods
        public void Setup(MultiplayerUI multiplayerUI, Lobby lobby)
        {
            int players = lobby.Players.Count;
            int maxPlayers = lobby.MaxPlayers;
            string mapName = lobby.Data["mapName"].Value;
            string version = lobby.Data["version"].Value;
            string joinCode = lobby.Data["joinCode"].Value;
            bool isPasswordProtected = !string.IsNullOrEmpty(lobby.Data["passwordHash"].Value);
            bool pvp = bool.Parse(lobby.Data["pvp"].Value);
            bool pve = bool.Parse(lobby.Data["pve"].Value);
            bool npc = bool.Parse(lobby.Data["npc"].Value);

            playersText.text = $"{players}/{maxPlayers}";
            nameText.text = lobby.Name;
            descriptionText.text = $"{mapName} ({version}) | {FormatTag("PVP", pvp)}, {FormatTag("PVE", pve)}, {FormatTag("NPCs", npc)}";
            padlockIcon.SetActive(isPasswordProtected);
            joinButton.onClick.AddListener(delegate 
            {
                if (isPasswordProtected)
                {
                    InputDialog.Input("Password Required", "Enter the password...", submitEvent: delegate (string password)
                    {
                        multiplayerUI.Join(lobby.LobbyCode, password);
                    });
                }
                else
                {
                    multiplayerUI.Join(lobby.LobbyCode);
                }
            });
        }

        private string FormatTag(string tag, bool enabled)
        {
            return $"{(enabled ? "<s>" : "")}{tag}{(enabled ? "</s>" : "")}";
        }
        #endregion
    }
}