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
            // Bug: MLAPI cannot be shutdown from within an RPC (https://github.com/Unity-Technologies/com.unity.multiplayer.mlapi/issues/942).
            // Temporary workaround: Invoke "NetworkManager.Shutdown()" at the end of the frame (once the rpc queue has been cleared).
            InvokeUtility.InvokeAtEndOfFrame(this, delegate
            {
                NetworkShutdownManager.Instance.Shutdown();
                SceneManager.LoadScene("MainMenu");
                InformationDialog.Inform("Disconnected!", reason);
            });
        }

        public void Leave()
        {
            StartCoroutine(LeaveRoutine());
        }
        private IEnumerator LeaveRoutine()
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

            LoadingManager.LoadScene("MainMenu", () => NetworkShutdownManager.Instance.Shutdown());
        }

        public void Kick(ulong clientId, string reason = default)
        {
            ForceDisconnectClientRpc(reason, NetworkUtils.SendTo(clientId));
        }
        #endregion
    }
}