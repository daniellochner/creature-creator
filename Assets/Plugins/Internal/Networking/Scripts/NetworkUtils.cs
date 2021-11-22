using Unity.Netcode;

namespace DanielLochner.Assets
{
    public static class NetworkUtils
    {
        public static ClientRpcParams SendTo(ulong clientId)
        {
            return new ClientRpcParams() { Send = new ClientRpcSendParams() { TargetClientIds = new[] { clientId } } };
        }
        public static bool IsPlayer(ulong clientId)
        {
            return clientId == NetworkManager.Singleton.LocalClientId;
        }
    }
}