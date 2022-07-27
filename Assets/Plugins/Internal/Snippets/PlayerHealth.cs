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

        public virtual void TakeDamage(float damage)
        {
            if (IsDead) return;
            
            Health -= damage;
            OnTakeDamage?.Invoke(damage);
        }
        public virtual void Die()
        {
            OnDie?.Invoke();
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

            if (Health <= 0f)
            {
                Die();
            }
        }

        [ContextMenu("Take Random Damage")]
        private void TakeRandomDamage()
        {
            TakeDamage(UnityEngine.Random.Range(0f, maxHealth));
        }
        #endregion
    }
}