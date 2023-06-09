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
                    health.Value = value;
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
        public Action<float, Vector3, DamageReason> OnTakeDamage { get; set; }
        public Action<DamageReason> OnDie { get; set; }

        public bool IsDead
        {
            get
            {
                return Health <= 0;
            }
        }
        public virtual bool CanTakeDamage => !IsDead;
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

        public void TakeDamage(float damage, Vector3 force = default, DamageReason reason = DamageReason.Misc)
        {
            TakeDamageServerRpc(damage, force, reason);
        }
        [ServerRpc(RequireOwnership = false)]
        private void TakeDamageServerRpc(float damage, Vector3 force, DamageReason reason)
        {
            if (CanTakeDamage)
            {
                Health -= damage;
                TakeDamageClientRpc(damage, force, reason);

                if (Health <= 0f)
                {
                    Die(reason);
                }
            }
        }
        [ClientRpc]
        private void TakeDamageClientRpc(float damage, Vector3 force, DamageReason reason)
        {
            OnTakeDamage?.Invoke(damage, force, reason);
        }

        public void Die(DamageReason reason)
        {
            DieServerRpc(reason);
        }
        [ServerRpc(RequireOwnership = false)]
        private void DieServerRpc(DamageReason reason)
        {
            transform.parent = null;
            DieClientRpc(reason);
        }
        [ClientRpc]
        private void DieClientRpc(DamageReason reason)
        {
            OnDie?.Invoke(reason);
        }

        [ContextMenu("Take Random Damage")]
        private void TakeRandomDamage()
        {
            TakeDamage(UnityEngine.Random.Range(0f, MaxHealth));
        }
        #endregion
    }
}