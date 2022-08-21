using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class TimeoutPreventer : NetworkBehaviour
    {
        private IEnumerator Start()
        {
            if (IsServer)
            {
                while (enabled)
                {
                    yield return new WaitForSeconds(1f);
                    PreventTimeoutServerRpc();

                    NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(NetworkManager.Singleton.LocalClientId);
                }
            }
        }
        [ServerRpc(RequireOwnership = false)]
        private void PreventTimeoutServerRpc()
        {
        }
    }
}