using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkConnectionManager : NetworkSingleton<NetworkConnectionManager>
    {
        #region Properties
        public static bool IsConnected => NetworkManager.Singleton && NetworkManager.Singleton.IsListening;
        #endregion

        #region Methods
        [ClientRpc]
        private void ForceDisconnectClientRpc(string reason, ClientRpcParams clientRpcParams)
        {
            ForceDisconnect(reason);
        }
        [ClientRpc]
        private void ForceDisconnectClientRpc(string reason)
        {
            if (!IsHost)
            {
                ForceDisconnect(reason);
            }
        }
        public void ForceDisconnect(string reason)
        {
            Leave(() => InformationDialog.Inform(LocalizationUtility.Localize("disconnect_title"), reason));
        }

        public void Leave(Action onLeave = null)
        {
            StartCoroutine(LeaveRoutine(onLeave));
        }
        private IEnumerator LeaveRoutine(Action onLeave)
        {
            // Disconnect all connected players before the host leaves the game.
            if (IsHost)
            {
                ForceDisconnectClientRpc(LocalizationUtility.Localize("disconnect_message_host-left-game"));
                while (NetworkManager.Singleton.ConnectedClients.Count > 1)
                {
                    yield return null;
                }
                LobbyHelper.Instance.DeleteActiveLobbies();
            }

            NetworkShutdownManager.Instance.Shutdown();
            SceneManager.LoadScene("MainMenu");

            onLeave?.Invoke();
        }

        public void Kick(ulong clientId, string playerId, string reason = default)
        {
            if (string.IsNullOrEmpty(reason))
            {
                reason = LocalizationUtility.Localize("disconnect_message_kicked");
            }

            ForceDisconnectClientRpc(reason, NetworkUtils.SendTo(clientId));

            List<string> kickedPlayers = new List<string>(LobbyHelper.Instance.JoinedLobby.TryGetValue("kickedPlayers", "").Split(","));
            kickedPlayers.Add(playerId);
            UpdateLobbyOptions options = new UpdateLobbyOptions()
            {
                Data = new Dictionary<string, DataObject>()
                {
                    { "kickedPlayers", new DataObject(DataObject.VisibilityOptions.Public, string.Join(",", kickedPlayers)) },
                }
            };

            options.HostId = AuthenticationService.Instance.PlayerId;
            LobbyService.Instance.UpdateLobbyAsync(LobbyHelper.Instance.JoinedLobby.Id, options);
        }
        #endregion
    }
}