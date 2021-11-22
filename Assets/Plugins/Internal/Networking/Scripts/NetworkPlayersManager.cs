using Unity.Netcode;
using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace DanielLochner.Assets
{
    /// <summary>
    /// Used by host/server to manage players in place of NetworkManager.Singleton.ConnectedClients.
    /// </summary>
    public class NetworkPlayersManager : MonoBehaviourSingleton<NetworkPlayersManager>
    {
        #region Fields
        [SerializeField] private int maxPayloadSize = 1024;
        [SerializeField] private int maxPlayers = 16;
        #endregion

        #region Properties
        public Dictionary<ulong, PlayerData> Players { get; private set; } = new Dictionary<ulong, PlayerData>();

        public Action<PlayerData> OnPlayerAdd { get; set; }
        public Action<PlayerData> OnPlayerRemove { get; set; }
        #endregion

        #region Methods
        private void Start()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ApproveConnection;
            NetworkManager.Singleton.OnClientDisconnectCallback += Remove;

            NetworkShutdownManager.Instance.OnShutdown += Clear;
        }

        private void Add(PlayerData playerData)
        {
            Players.Add(playerData.clientId, playerData);
            OnPlayerAdd?.Invoke(playerData);
        }
        private void Remove(ulong clientId)
        {
            OnPlayerRemove?.Invoke(Players[clientId]);
            Players.Remove(clientId);
        }
        private void Clear()
        {
            OnPlayerAdd = OnPlayerRemove = null;
            Players.Clear();
        }

        private void ApproveConnection(byte[] data, ulong clientId, NetworkManager.ConnectionApprovedDelegate connectionApproved)
        {
            if (data.Length > maxPayloadSize)
            {
                connectionApproved(false, null, false, null, null);
                return;
            }

            if (Players.Count >= maxPlayers)
            {
                connectionApproved(false, null, false, null, null);
                return;
            }

            PlayerData playerData = JsonUtility.FromJson<PlayerData>(Encoding.UTF8.GetString(data));
            playerData.clientId = clientId;
            Add(playerData);

            connectionApproved(true, null, true, null, null);
        }
        #endregion
    }
}