using System;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class PlayerHealth : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private float maxHealth = 100f;
        [Space]
        [SerializeField] private NetworkVariable<float> health = new NetworkVariable<float>(100);
        #endregion

        #region Properties
        public float Health
        {
            get => health.Value;
            set
            {
                if (IsServer)
                {
                    health.Value = Mathf.Clamp(value, 0f, maxHealth);

                    if (Health <= 0f)
                    {
                        Die();
                    }
                }
                else
                {
                    SetHealthServerRpc(value);
                }
            }
        }
        public float HealthPercentage
        {
            get => Mathf.InverseLerp(0f, maxHealth, Health);
            set => Health = Mathf.Lerp(0f, maxHealth, value);
        }

        public Action<float> OnHealthChanged { get; set; }
        public Action<float> OnTakeDamage { get; set; }
        public Action OnDie { get; set; }

        public bool IsDead
        {
            get
            {
                return Health <= 0;
            }
        }
        #endregion

        #region Methods
        private void Start()
        {
            health.OnValueChanged += UpdateHealth;
            health.SetDirty(true);
        }

        [ServerRpc]
        private void SetHealthServerRpc(float health)
        {
            Health = health;
        }
        private void UpdateHealth(float oldHealth, float newHealth)
        {
            Health = newHealth;
            OnHealthChanged?.Invoke(Health);
        }

        public void TakeDamage(float damage)
        {
            TakeDamageServerRpc(damage);
        }
        [ServerRpc(RequireOwnership = false)]
        private void TakeDamageServerRpc(float damage)
        {
            if (IsDead) return;
            Health -= damage;
            TakeDamageClientRpc(damage);
        }
        [ClientRpc]
        private void TakeDamageClientRpc(float damage)
        {
            OnTakeDamage?.Invoke(damage);
        }

        public void Die()
        {
            DieServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void DieServerRpc()
        {
            if (IsDead) return;
            Health = 0f;
            DieClientRpc();
        }
        [ClientRpc]
        private void DieClientRpc()
        {
            OnDie?.Invoke();
        }

        [ContextMenu("Take Random Damage")]
        private void TakeRandomDamage()
        {
            TakeDamage(UnityEngine.Random.Range(0f, maxHealth));
        }
        #endregion
    }
}