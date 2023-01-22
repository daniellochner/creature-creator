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

        [SerializeField] private float hungerDepletionRate = 1f / 1200f;
        [SerializeField] private float healthTickRate = 1f;
        [SerializeField] private float healthTickDamage = 5f;

        private Coroutine hungerDepletingCoroutine;
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
        }

        [ServerRpc]
        private void SetHungerServerRpc(float h)
        {
            hunger.Value = h;
        }
        private void UpdateHunger(float oldHunger, float newHunger)
        {
            //Hunger = newHunger;
            OnHungerChanged?.Invoke(Hunger);

            if (newHunger >= 1f)
            {
                Health.HealthPercentage = 1f;
            }
        }

        public void StartDepletingHunger()
        {
            if (WorldManager.Instance.World.CreativeMode) return;

            StopDepletingHunger();

            Hunger = 1f;
            hungerDepletingCoroutine = StartCoroutine(HungerDepletionRoutine(hungerDepletionRate, healthTickRate, healthTickDamage));
        }
        public void StopDepletingHunger()
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
                {
                    Hunger -= hungerDepletionRate;
                    yield return new WaitForSeconds(1f);
                }
            }
        }
        #endregion
    }
}