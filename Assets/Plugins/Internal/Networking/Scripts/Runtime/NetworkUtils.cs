using Unity.Netcode;

namespace DanielLochner.Assets
{
    public static class NetworkUtils
    {
        public static ClientRpcParams SendTo(params ulong[] clientIds)
        {
            return new ClientRpcParams() { Send = new ClientRpcSendParams() { TargetClientIds = clientIds } };
        }
        public static bool IsPlayer(ulong clientId)
        {
            return clientId == NetworkManager.Singleton.LocalClientId;
        }
    }
}