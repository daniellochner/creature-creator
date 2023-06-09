using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;

namespace DanielLochner.Assets
{
    public static class NetworkUtils
    {
        public static ClientRpcParams SendTo(params ulong[] clientIds)
        {
            return new ClientRpcParams() { Send = new ClientRpcSendParams() { TargetClientIds = clientIds } };
        }

        public static ClientRpcParams DontSendTo(params ulong[] clientIds)
        {
            List<ulong> targetClientIds = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);
            foreach (ulong clientId in clientIds)
            {
                targetClientIds.Remove(clientId);
            }
            return new ClientRpcParams() { Send = new ClientRpcSendParams() { TargetClientIds = targetClientIds } };
        }

        public static bool IsPlayer(ulong clientId)
        {
            return clientId == NetworkManager.Singleton.LocalClientId;
        }
        public static T TryGetValue<T>(this Lobby lobby, string key, T fallback = default)
        {
            if (lobby.Data.ContainsKey(key))
            {
                return (T)Convert.ChangeType(lobby.Data[key].Value, typeof(T));
            }
            return fallback;
        }
    }
}