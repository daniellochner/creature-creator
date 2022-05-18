using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class PlayerHealth : MonoBehaviour
    {
        #region Fields
        [SerializeField] private MinMax minMaxHealth = new MinMax(0f, 100f);
        [Space]
        [SerializeField, ReadOnly] private float health;
        #endregion

        #region Properties
        public MinMax MinMaxHealth => minMaxHealth;

        public float Health
        {
            get => health;
            set
            {
                health = Mathf.Clamp(value, minMaxHealth.min, minMaxHealth.max);
                OnHealthChanged?.Invoke(health);
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

        public bool IsDead { get; private set; }
        #endregion

        #region Methods
        private void Start()
        {
            health = minMaxHealth.max;
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

            IsDead = true;
            OnDie?.Invoke();
        }
        public virtual void Respawn()
        {
            Health = minMaxHealth.max;
            IsDead = false;
            OnRespawn?.Invoke();
        }

        [ContextMenu("Take Damage")]
        private void TakeRandomDamage()
        {
            TakeDamage(minMaxHealth.Random);
        }
        #endregion
    }
}