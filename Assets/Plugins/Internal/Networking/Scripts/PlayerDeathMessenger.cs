using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerDeathMessenger : NetworkBehaviour
    {
        [SerializeField] private string[] deathMessages;

        public PlayerHealth Health { get; set; }

        private void Awake()
        {
            Health = GetComponent<PlayerHealth>();
        }

        public void Setup()
        {
            if (IsOwner)
            {
                Health.OnDie += () => SendDeathMsgServerRpc(OwnerClientId);
            }
        }

        [ServerRpc]
        private void SendDeathMsgServerRpc(ulong clientId)
        {
            SendDeathMsgClientRpc(Random.Range(0, deathMessages.Length), NetworkHostManager.Instance.Players[clientId].username);
        }
        [ClientRpc]
        private void SendDeathMsgClientRpc(int message, string name)
        {
            NotificationsManager.Notify(deathMessages[message].Replace("{name}", name));
        }
    }
}