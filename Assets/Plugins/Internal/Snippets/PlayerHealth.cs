using System;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class PlayerHealth : NetworkBehaviour
    {
        #region Fields
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
                    health.Value = Mathf.Clamp(value, 0f, MaxHealth);
                }
                else
                {
                    SetHealthServerRpc(value);
                }
            }
        }
        public float HealthPercentage
        {
            get => Mathf.InverseLerp(0f, MaxHealth, Health);
            set => Health = Mathf.Lerp(0f, MaxHealth, value);
        }

        public virtual float MaxHealth => 100f;

        public Action<float> OnHealthChanged { get; set; }
        public Action<float, Vector3> OnTakeDamage { get; set; }
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
        protected virtual void Start()
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
            OnHealthChanged?.Invoke(Health);
        }

        public void TakeDamage(float damage, Vector3 force = default)
        {
            TakeDamageServerRpc(damage, force);
        }
        [ServerRpc(RequireOwnership = false)]
        private void TakeDamageServerRpc(float damage, Vector3 force)
        {
            if (IsDead) return;
            Health -= damage;
            TakeDamageClientRpc(damage, force);

            if (Health <= 0f)
            {
                Die();
            }
        }
        [ClientRpc]
        private void TakeDamageClientRpc(float damage, Vector3 force)
        {
            OnTakeDamage?.Invoke(damage, force);
        }

        public void Die()
        {
            DieServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void DieServerRpc()
        {
            transform.parent = null;
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
            TakeDamage(UnityEngine.Random.Range(0f, MaxHealth));
        }
        #endregion
    }
}