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
        private void Start()
        {
            if (IsServer)
            {
                Health.OnDie += OnDie;
            }
        }

        private void OnDie(DamageReason reason, string inflicter)
        {
            SendDeathMsg(OwnerClientId);
        }

        private void SendDeathMsg(ulong clientId)
        {
            SendDeathMsgClientRpc(Random.Range(0, deathMessages.Length), NetworkHostManager.Instance.Players[clientId].username);
        }
        [ClientRpc]
        private void SendDeathMsgClientRpc(int message, string name)
        {
            NotificationsManager.Notify(LocalizationUtility.Localize(deathMessages[message], name));
        }
    }
}