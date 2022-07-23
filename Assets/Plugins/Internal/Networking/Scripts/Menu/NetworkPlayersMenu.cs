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
        [SerializeField] private PlayerNameUI playerNameUIPrefab;
        [SerializeField] private TextMeshProUGUI connectionText;
        [SerializeField] private CodeField worldCodeField;

        private Dictionary<ulong, PlayerNameUI> playerNames = new Dictionary<ulong, PlayerNameUI>();
        #endregion

        #region Methods
        private void Update()
        {
            HandleMenuState();
        }

        public void Setup()
        {
            StartCoroutine(UpdateConnectionRoutine(2));

            worldCodeField.Setup(LobbyHelper.Instance.JoinedLobby.Id, onDeselect: delegate (string input)
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
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Open();
            }
            else if (Input.GetKeyUp(KeyCode.Tab) && !worldCodeField.IsVisible)
            {
                Close();
            }
        }

        private IEnumerator UpdateConnectionRoutine(float updateRate)
        {
            while (true)
            {
                yield return new WaitForSeconds(1f / updateRate);

                if (NetworkManager.Singleton.IsHost)
                {
                    connectionText.text = $"Connection: Host";
                }
                else
                {
                    connectionText.text = $"Connection: Client ({Mathf.RoundToInt(NetworkStatsManager.Instance.LastRTT * 1000f)} ms)";
                }
            }
        }
        #endregion
    }
}