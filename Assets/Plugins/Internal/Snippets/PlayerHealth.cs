using System;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class PlayerHealth : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private MinMax minMaxHealth = new MinMax(0f, 100f);
        [Space]
        [SerializeField] private NetworkVariable<float> health = new NetworkVariable<float>(100);
        #endregion

        #region Properties
        public MinMax MinMaxHealth => minMaxHealth;

        public float Health
        {
            get => health.Value;
            set
            {
                if (IsServer)
                {
                    health.Value = Mathf.Clamp(value, minMaxHealth.min, minMaxHealth.max);
                }
                else
                {
                    SetHealthServerRpc(value);
                }
            }
        }
        public float HealthPercentage
        {
            get => Mathf.InverseLerp(minMaxHealth.min, minMaxHealth.max, Health);
            set => Health = Mathf.Lerp(minMaxHealth.min, minMaxHealth.max, value);
        }

        public Action<float> OnHealthChanged { get; set; }
        public Action<float> OnTakeDamage { get; set; }
        public Action OnDie { get; set; }
        public Action OnRespawn { get; set; }

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

            if (Health <= minMaxHealth.min)
            {
                Die();
            }
        }
        public virtual void Die()
        {
            if (IsDead) return;
            OnDie?.Invoke();
        }
        public virtual void Respawn()
        {
            Health = minMaxHealth.max;
            OnRespawn?.Invoke();
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

        [ContextMenu("Take Damage")]
        private void TakeRandomDamage()
        {
            TakeDamage(minMaxHealth.Random);
        }
        #endregion
    }
}