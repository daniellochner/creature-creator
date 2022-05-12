using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkConnectionManager : NetworkSingleton<NetworkConnectionManager>
    {
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
            Leave(() => InformationDialog.Inform("Disconnected!", reason));
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
                ForceDisconnectClientRpc("The host left the game.");
                while (NetworkManager.Singleton.ConnectedClients.Count > 1)
                {
                    yield return null;
                }
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