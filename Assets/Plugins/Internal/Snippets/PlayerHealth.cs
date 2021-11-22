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

        public bool IsDead { get; private set; }
        #endregion

        #region Methods
        private void Start()
        {
            health = minMaxHealth.max;
        }

        public void TakeDamage(float damage)
        {
            if (IsDead) return;

            Health -= damage;
            OnTakeDamage(damage);

            if (Health <= minMaxHealth.min)
            {
                Die();
            }
        }
        public void Die()
        {
            if (IsDead) return;

            IsDead = true;
            OnDie();
        }

        protected virtual void OnTakeDamage(float damage)
        {

        }
        protected virtual void OnDie()
        {

        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            health = minMaxHealth.max;
        }

        [ContextMenu("Take Damage")]
        private void TakeRandomDamage()
        {
            TakeDamage(UnityEngine.Random.Range(minMaxHealth.min, minMaxHealth.max));
        }
#endif
        #endregion
    }
}