using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkConnectionManager : NetworkSingleton<NetworkConnectionManager>
    {
        #region Properties
        public static bool IsConnected => NetworkManager.Singleton.IsListening;
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

        public void Kick(ulong clientId, string reason = default)
        {
            ForceDisconnectClientRpc(reason, NetworkUtils.SendTo(clientId));
        }
        #endregion
    }
}