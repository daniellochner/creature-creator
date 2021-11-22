// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkCreaturesManager : NetworkSingleton<NetworkCreaturesManager>
    {
        #region Methods
        public void Setup()
        {
            if (IsHost)
            {
                NetworkPlayersManager.Instance.OnPlayerAdd += PlayerJoinClientRpc;
                NetworkPlayersManager.Instance.OnPlayerRemove += PlayerLeaveClientRpc;
            }
            HandleExistingPlayers();
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
            foreach (PlayerData playerData in NetworkPlayersManager.Instance.Players.Values)
            {
                NetworkCreature networkedCreature = NetworkManager.Singleton.ConnectedClients[playerData.clientId].PlayerObject.GetComponent<NetworkCreature>();
                if (!networkedCreature.IsHidden.Value)
                {
                    networkedCreature.LoadPlayerClientRpc(playerData, JsonUtility.ToJson(networkedCreature.PlayerCreature.Constructor.Data), NetworkUtils.SendTo(clientId));
                }
            }
        }
        public void HandleExistingPlayers()
        {
            HandleExistingPlayersServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        #endregion
    }
}