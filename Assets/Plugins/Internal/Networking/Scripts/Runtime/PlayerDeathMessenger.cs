using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerDeathMessenger : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private string[] deathMessages;
        [SerializeField] private Color colour = Color.white;
        #endregion

        #region Properties
        public PlayerHealth Health { get; set; }
        #endregion

        #region Methods
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
            NotificationsManager.Notify(LocalizationUtility.Localize(deathMessages[message], name).ToColour(colour));
        }

        public void SetColour(Color colour)
        {
            this.colour = colour;
        }
        #endregion
    }
}