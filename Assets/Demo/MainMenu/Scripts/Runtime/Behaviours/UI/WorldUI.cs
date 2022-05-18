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
            playersText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
            nameText.text = lobby.Name;
            joinButton.onClick.AddListener(delegate 
            {
                multiplayerUI.Join(lobby.Id);
            });

            World world = new World(lobby);
            descriptionText.text = $"{world.MapName} ({world.Version}) | {FormatTag("PVP", world.AllowPVP)}, {FormatTag("PVE", world.AllowPVE)}, {FormatTag("NPCs", world.SpawnNPC)}";
            padlockIcon.SetActive(world.IsPasswordProtected);
        }

        private string FormatTag(string tag, bool enabled)
        {
            return $"{(enabled ? "" : "<s>")}{tag}{(enabled ? "" : "</s>")}";
        }
        #endregion
    }
}