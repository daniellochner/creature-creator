using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(PlayerHealth))]
    public class NetworkPlayerHealth : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private NetworkVariable<float> health;
        #endregion

        #region Properties
        private PlayerHealth Health { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Health = GetComponent<PlayerHealth>();
        }

        private void Start()
        {
            if (IsOwner)
            {
                Health.OnHealthChanged += SetHealthServerRpc;
            }
            else
            {
                health.OnValueChanged += UpdateHealth;
            }
        }

        [ServerRpc]
        private void SetHealthServerRpc(float h)
        {
            health.Value = h;
        }
        private void UpdateHealth(float oldH, float newH)
        {
            Health.Health = newH;
        }
        #endregion
    }
}