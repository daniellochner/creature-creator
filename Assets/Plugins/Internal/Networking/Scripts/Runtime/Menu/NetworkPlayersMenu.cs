using Unity.Netcode;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

namespace DanielLochner.Assets
{
    public class NetworkPlayersMenu : MenuSingleton<NetworkPlayersMenu>
    {
        #region Fields
        [SerializeField] private Keybind keybind;
        [SerializeField] private PlayerNameUI playerNameUIPrefab;
        [SerializeField] private TextMeshProUGUI connectionText;
        [SerializeField] private CodeField worldCodeField;

        private Dictionary<ulong, PlayerNameUI> playerNames = new Dictionary<ulong, PlayerNameUI>();
        #endregion

        #region Properties
        public int NumPlayers => playerNames.Count;

        public Keybind Keybind
        {
            get => keybind;
            set => keybind = value;
        }
        #endregion

        #region Methods
        private void Update()
        {
            HandleMenuState();
            HandleConnection();
        }

        public void Setup(string code)
        {
            worldCodeField.Setup(code, onDeselect: delegate (string input)
            {
                if (IsOpen) Close();
            });
        }

        public void AddPlayer(PlayerData playerData)
        {
            PlayerNameUI playerNameUI = Instantiate(playerNameUIPrefab, transform);
            playerNameUI.Setup(playerData);

            playerNames.Add(playerData.clientId, playerNameUI);
        }
        public void RemovePlayer(ulong clientId)
        {
            Destroy(playerNames[clientId].gameObject);
            playerNames.Remove(clientId);
        }
        public void SetName(ulong clientId, string name)
        {
            if (string.IsNullOrEmpty(name) || !playerNames.ContainsKey(clientId)) return;
            playerNames[clientId].SetName(name);
        }

        private void HandleMenuState()
        {
            if (InputUtility.GetKeyDown(keybind))
            {
                Open();
            }
            else if (InputUtility.GetKeyUp(keybind) && !worldCodeField.IsVisible)
            {
                Close();
            }
        }
        private void HandleConnection()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                connectionText.text = LocalizationUtility.Localize("network_menu_host");
            }
            else
            {
                connectionText.text = LocalizationUtility.Localize("network_menu_client", Mathf.RoundToInt(NetworkStatsManager.Instance.LastRTT * 1000f));
            }
        }
        #endregion
    }
}