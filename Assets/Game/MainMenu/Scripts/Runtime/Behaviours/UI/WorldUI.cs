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
                    $"<b>{LocalizationUtility.Localize("world_version")}:</b> {world.Version}<br>" +
                    $"<b>{LocalizationUtility.Localize("world_map")}:</b> {world.MapName}<br>" + 
                    $"<b>{LocalizationUtility.Localize("world_mode")}:</b> {FormatMode(world.CreativeMode)}<br>" +
                    $"<b>{LocalizationUtility.Localize("world_pvp")}:</b> {FormatEnabled(world.EnablePVP)}<br>" +
                    $"<b>{LocalizationUtility.Localize("world_npcs")}:</b> {FormatEnabled(world.SpawnNPC)}<br>" +
                    $"<b>{LocalizationUtility.Localize("world_pve")}:</b> {FormatEnabled(world.EnablePVE)}<br>" +
                    $"<b>{LocalizationUtility.Localize("world_profanity")}:</b> {FormatAllowed(world.AllowProfanity)}"
                    );
            });

            padlockIcon.SetActive(world.IsPasswordProtected);
        }

        private string FormatMode(bool isCreativeMode)
        {
            return isCreativeMode ? LocalizationUtility.Localize("world_creative") : LocalizationUtility.Localize("world_adventure");
        }
        private string FormatEnabled(bool isEnabled)
        {
            return isEnabled ? LocalizationUtility.Localize("world_enabled") : LocalizationUtility.Localize("world_disabled");
        }
        private string FormatAllowed(bool isAllowed)
        {
            return isAllowed ? LocalizationUtility.Localize("world_allowed") : LocalizationUtility.Localize("world_forbidden");
        }
        #endregion
    }
}