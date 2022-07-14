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
                NetworkHostManager.Instance.OnPlayerAdd += PlayerJoinClientRpc;
                NetworkHostManager.Instance.OnPlayerRemove += PlayerLeaveClientRpc;
            }
            HandleExistingPlayers();

            NetworkCreaturesMenu.Instance.Setup();
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
            foreach (PlayerData playerData in NetworkHostManager.Instance.Players.Values)
            {
                NetworkCreaturePlayer creature = NetworkManager.Singleton.ConnectedClients[playerData.clientId].PlayerObject.GetComponent<NetworkCreaturePlayer>();
                string creatureData = JsonUtility.ToJson(creature.Source.Constructor.Data);
                creature.LoadPlayerClientRpc(playerData, creatureData, NetworkUtils.SendTo(clientId));
            }

            NetworkCreatureNonPlayer[] npcs = FindObjectsOfType<NetworkCreatureNonPlayer>();
            foreach (NetworkCreatureNonPlayer npc in npcs)
            {
                npc.ReconstructAndShowClientRpc(JsonUtility.ToJson(npc.Source.Constructor.Data), NetworkUtils.SendTo(clientId));
            }
        }
        public void HandleExistingPlayers()
        {
            HandleExistingPlayersServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        #endregion
    }
}