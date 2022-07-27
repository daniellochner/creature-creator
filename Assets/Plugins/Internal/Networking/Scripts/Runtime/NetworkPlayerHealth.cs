using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(PlayerHealth))]
    public class NetworkPlayerHealth : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private NetworkVariable<float> health = new NetworkVariable<float>(100);
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
            if (IsServer)
            {
                Health.OnHealthChanged += delegate (float h)
                {
                    health.Value = h;
                };
            }
            else
            {
                health.OnValueChanged += UpdateHealth;
            }
        }
        
        private void UpdateHealth(float oldH, float newH)
        {
            Health.Health = newH;
        }
        #endregion
    }
}