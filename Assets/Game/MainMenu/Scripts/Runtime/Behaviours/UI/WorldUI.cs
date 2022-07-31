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
        [SerializeField] private GameObject padlockIcon;
        [SerializeField] private Button infoButton;
        [SerializeField] private Button joinButton;
        #endregion

        #region Properties
        public Button JoinButton => joinButton;

        public int Players { get; private set; }
        #endregion

        #region Methods
        public void Setup(MultiplayerUI multiplayerUI, Lobby lobby)
        {
            Players = lobby.Players.Count;

            playersText.text = $"{Players}/{lobby.MaxPlayers}";
            nameText.text = lobby.Name;
            joinButton.onClick.AddListener(delegate 
            {
                multiplayerUI.Join(lobby.Id);
            });

            World world = new World(lobby);

            infoButton.onClick.AddListener(delegate
            {
                InformationDialog.Inform(world.WorldName, 
                    $"<b>Version:</b> {world.Version}<br>" +
                    $"<b>Map:</b> {world.MapName}<br>" + 
                    $"<b>PVP:</b> {FormatEnabled(world.EnablePVP)}<br>" +
                    $"<b>PVE:</b> {FormatEnabled(world.EnablePVE)}<br>" +
                    $"<b>NPCs:</b> {FormatEnabled(world.SpawnNPC)}<br>" +
                    $"<b>Profanity:</b> {FormatAllowed(world.AllowProfanity)}"
                    );
            });

            padlockIcon.SetActive(world.IsPasswordProtected);
        }

        private string FormatEnabled(bool isEnabled)
        {
            return isEnabled ? "Enabled" : "Disabled";
        }
        private string FormatAllowed(bool isAllowed)
        {
            return isAllowed ? "Allowed" : "Forbidden";
        }
        #endregion
    }
}