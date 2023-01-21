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
        [SerializeField] private TextMeshProUGUI mapText;
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

            WorldMP world = new WorldMP(lobby);
            mapText.text = $"{world.MapName} ({FormatMode(world.CreativeMode)})";

            infoButton.onClick.AddListener(delegate
            {
                InformationDialog.Inform(world.WorldName, 
                    $"<b>{LocalizationUtility.Localize("mainmenu_multiplayer_world_version")}:</b> {world.Version}<br>" +
                    $"<b>{LocalizationUtility.Localize("mainmenu_multiplayer_world_map")}:</b> {world.MapName}<br>" + 
                    $"<b>{LocalizationUtility.Localize("mainmenu_multiplayer_world_mode")}:</b> {FormatMode(world.CreativeMode)}<br>" +
                    $"<b>{LocalizationUtility.Localize("mainmenu_multiplayer_world_pvp")}:</b> {FormatEnabled(world.EnablePVP)}<br>" +
                    $"<b>{LocalizationUtility.Localize("mainmenu_multiplayer_world_npcs")}:</b> {FormatEnabled(world.SpawnNPC)}<br>" +
                    $"<b>{LocalizationUtility.Localize("mainmenu_multiplayer_world_pve")}:</b> {FormatEnabled(world.EnablePVE)}<br>" +
                    $"<b>{LocalizationUtility.Localize("mainmenu_multiplayer_world_profanity")}:</b> {FormatAllowed(world.AllowProfanity)}"
                    );
            });

            padlockIcon.SetActive(world.IsPasswordProtected);
        }

        private string FormatMode(bool isCreativeMode)
        {
            return isCreativeMode ? LocalizationUtility.Localize("mainmenu_multiplayer_world_creative") : LocalizationUtility.Localize("mainmenu_multiplayer_world_adventure");
        }
        private string FormatEnabled(bool isEnabled)
        {
            return isEnabled ? LocalizationUtility.Localize("mainmenu_multiplayer_world_enabled") : LocalizationUtility.Localize("mainmenu_multiplayer_world_disabled");
        }
        private string FormatAllowed(bool isAllowed)
        {
            return isAllowed ? LocalizationUtility.Localize("mainmenu_multiplayer_world_allowed") : LocalizationUtility.Localize("mainmenu_multiplayer_world_forbidden");
        }
        #endregion
    }
}