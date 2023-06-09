// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureHealth))]
    [RequireComponent(typeof(CreatureSpawner))]
    public class CreatureHunger : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private NetworkVariable<float> hunger = new NetworkVariable<float>(1f);

        [SerializeField] private float hungerHealThreshold = 0.9f;
        [SerializeField] private float hungerDepletionRate = 1f / 1200f;
        [SerializeField] private float healthTickRate = 1f;
        [SerializeField] private float healthTickDamage = 5f;
        [SerializeField] private float healthTickHeal = 10f;
        [SerializeField] private float healFromHungerCooldown = 10f;

        private Coroutine hungerDepletingCoroutine;
        private float timeLeftToHealFromHunger;
        #endregion

        #region Properties
        public Action<float> OnHungerChanged { get; set; }
        public CreatureHealth Health { get; private set; }
        public CreatureSpawner Spawner { get; private set; }

        public float Hunger
        {
            get => hunger.Value;
            set
            {
                if (IsServer)
                {
                    hunger.Value = Mathf.Clamp01(value);
                }
                else
                {
                    SetHungerServerRpc(value);
                }
            }
        }
        #endregion

        #region Methods
        private void Awake()
        {
            Health = GetComponent<CreatureHealth>();
            Spawner = GetComponent<CreatureSpawner>();
        }
        private void Start()
        {
            hunger.OnValueChanged += UpdateHunger;
            hunger.SetDirty(true);

            if (IsServer)
            {
                Health.OnTakeDamage += delegate
                {
                    timeLeftToHealFromHunger = healFromHungerCooldown;
                };
            }
        }
        private void Update()
        {
            if (IsServer && timeLeftToHealFromHunger > 0)
            {
                timeLeftToHealFromHunger -= Time.deltaTime;
            }
        }

        [ServerRpc]
        private void SetHungerServerRpc(float h)
        {
            hunger.Value = h;
        }
        private void UpdateHunger(float oldHunger, float newHunger)
        {
            OnHungerChanged?.Invoke(Hunger);
        }

        public void StartHunger()
        {
            StopHunger();

            Hunger = 1f;
            hungerDepletingCoroutine = StartCoroutine(HungerDepletionRoutine(hungerDepletionRate, healthTickRate, healthTickDamage));
            timeLeftToHealFromHunger = 0f;
        }
        public void StopHunger()
        {
            if (hungerDepletingCoroutine != null)
            {
                StopCoroutine(hungerDepletingCoroutine);
            }
        }

        private IEnumerator HungerDepletionRoutine(float hungerDepletionRate, float healthTickRate, float healthTickDamage)
        {
            while (!Health.IsDead)
            {
                if (Hunger <= 0)
                {
                    Health.TakeDamage(healthTickDamage);
                    yield return new WaitForSeconds(1f / healthTickRate);
                }
                else
                if (Hunger > hungerHealThreshold)
                {
                    if (timeLeftToHealFromHunger <= 0 && Health.HealthPercentage < 1f)
                    {
                        Health.Health += healthTickHeal;
                    }
                    yield return new WaitForSeconds(1f / healthTickRate);
                }
                else 
                if (!WorldManager.Instance.World.CreativeMode && hungerDepletionRate > 0)
                {
                    Hunger -= hungerDepletionRate;
                    yield return new WaitForSeconds(1f);
                }
            }
        }
        #endregion
    }
}