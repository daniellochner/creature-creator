// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkCreaturesManager : NetworkSingleton<NetworkCreaturesManager>
    {
        #region Fields
        [SerializeField] private NetworkCreaturesMenu networkMenuPrefab;
        #endregion

        #region Methods
        public void Setup()
        {
            if (IsServer)
            {
                NetworkHostManager.Instance.OnPlayerAdd += PlayerJoinClientRpc;
                NetworkHostManager.Instance.OnPlayerRemove += PlayerLeaveClientRpc;
            }
            
            Instantiate(networkMenuPrefab, Dynamic.OverlayCanvas).Setup();
            HandleExistingPlayers();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();

            if (NetworkHostManager.Instance)
            {
                NetworkHostManager.Instance.OnPlayerAdd -= PlayerJoinClientRpc;
                NetworkHostManager.Instance.OnPlayerRemove -= PlayerLeaveClientRpc;
            }
        }

        [ClientRpc]
        private void PlayerJoinClientRpc(PlayerData playerData)
        {
            if (NetworkUtils.IsPlayer(playerData.clientId)) return;
            OnPlayerJoin(playerData);
        }
        [ClientRpc]
        private void PlayerLeaveClientRpc(PlayerData playerData)
        {
            if (NetworkUtils.IsPlayer(playerData.clientId)) return;
            OnPlayerLeave(playerData);
        }
        private void OnPlayerJoin(PlayerData playerData)
        {
            NotificationsManager.Notify($"{playerData.username} has joined the game.");
            NetworkCreaturesMenu.Instance.AddPlayer(playerData);
        }
        private void OnPlayerLeave(PlayerData playerData)
        {
            NotificationsManager.Notify($"{playerData.username} has left the game.");
            NetworkCreaturesMenu.Instance.RemovePlayer(playerData.clientId);
        }

        [ServerRpc(RequireOwnership = false)]
        public void HandleExistingPlayersServerRpc(ulong clientId)
        {
            foreach (PlayerData data in NetworkHostManager.Instance.Players.Values)
            {
                HandleExistingPlayersClientRpc(data, NetworkUtils.SendTo(clientId));
            }
        }
        [ClientRpc]
        public void HandleExistingPlayersClientRpc(PlayerData data, ClientRpcParams clientRpcParams)
        {
            NetworkCreaturesMenu.Instance.AddPlayer(data);
        }
        public void HandleExistingPlayers()
        {
            HandleExistingPlayersServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        #endregion
    }
}