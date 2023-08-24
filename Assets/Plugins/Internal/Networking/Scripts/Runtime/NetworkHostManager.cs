using Unity.Netcode;
using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Authentication;

namespace DanielLochner.Assets
{
    public class NetworkHostManager : MonoBehaviourSingleton<NetworkHostManager>
    {
        #region Fields
        [SerializeField] private int maxPayloadSize = 1024;
        [SerializeField] private int maxPlayers = 16;

        [Header("Debug")]
        [SerializeField, ReadOnly] private string password;
        #endregion

        #region Properties
        public Dictionary<ulong, PlayerData> Players { get; private set; } = new Dictionary<ulong, PlayerData>();

        public Action<PlayerData> OnPlayerAdd { get; set; }
        public Action<PlayerData> OnPlayerRemove { get; set; }

        public string Password
        {
            get => password;
            set => password = value;
        }

        public Vector3 SpawnPosition { get; set; } = Vector3.zero;
        public Quaternion SpawnRotation { get; set; } = Quaternion.identity;

        public Action<ulong> OnApproveConnection { get; set; }
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
            // Remove disconnecting player from joined lobby immediately...
            try
            {
                var lobbyId = LobbyHelper.Instance.JoinedLobby.Id;
                var playerId = Players[clientId].playerId;
                LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            OnPlayerRemove?.Invoke(Players[clientId]);
            Players.Remove(clientId);
        }
        private void Clear()
        {
            OnPlayerAdd = OnPlayerRemove = null;
            Players.Clear();
        }
        
        private void ApproveConnection(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            byte[] data = request.Payload;
            ulong clientId = request.ClientNetworkId;

            if (data.Length > maxPayloadSize)
            {
                response.Approved = false;
                return;
            }

            if (Players.Count >= maxPlayers)
            {
                response.Approved = false;
                return;
            }

            ConnectionData connectionData = JsonUtility.FromJson<ConnectionData>(Encoding.UTF8.GetString(data));
            if (connectionData.password != Password)
            {
                response.Approved = false;
                return;
            }

            PlayerData playerData = new PlayerData()
            {
                playerId = connectionData.playerId,
                clientId = clientId,
                username = connectionData.username
            };
            Add(playerData);

            response.CreatePlayerObject = NetworkManager.Singleton.NetworkConfig.PlayerPrefab != null;
            response.Position = SpawnPosition;
            response.Rotation = SpawnRotation;
            response.Approved = true;
            OnApproveConnection?.Invoke(clientId);
        }
        #endregion
    }
}