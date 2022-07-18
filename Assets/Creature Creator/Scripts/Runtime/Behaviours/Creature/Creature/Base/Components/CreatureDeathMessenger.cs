using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureHealth))]
    public class CreatureDeathMessenger : NetworkBehaviour
    {
        private readonly string[] DEATH_MESSAGES = new string[]
        {
            "{username} died.",
            "{username} kicked the bucket.",
            "{username} passed away.",
            "{username} expired.",
            "{username} bit the dust.",
            "{username} perished.",
        };

        public CreatureHealth Health { get; set; }

        private void Awake()
        {
            Health = GetComponent<CreatureHealth>();
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
            string message = DEATH_MESSAGES[Random.Range(0, DEATH_MESSAGES.Length)];
            SendDeathMsgClientRpc(message.Replace("{username}", NetworkHostManager.Instance.Players[clientId].username));
        }
        [ClientRpc]
        private void SendDeathMsgClientRpc(string message)
        {
            NotificationsManager.Notify(message);
        }
    }
}