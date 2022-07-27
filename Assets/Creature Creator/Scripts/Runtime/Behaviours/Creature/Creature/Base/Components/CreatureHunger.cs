// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureHealth))]
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
        public bool DepleteHunger { get; set; } = true;
        public CreatureHealth Health { get; private set; }

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
        }
        private void Start()
        {
            hunger.OnValueChanged += UpdateHunger;
            hunger.SetDirty(true);
        }
        private void OnEnable()
        {
            if (IsServer)
            {
                if (hungerDepletingCoroutine != null)
                {
                    StopCoroutine(hungerDepletingCoroutine);
                }
                hungerDepletingCoroutine = StartCoroutine(HungerDepletionRoutine(hungerDepletionRate, healthTickRate, healthTickDamage));
            }
        }

        [ServerRpc]
        private void SetHungerServerRpc(float h)
        {
            hunger.Value = h;
        }
        private void UpdateHunger(float oldHunger, float newHunger)
        {
            Hunger = newHunger;
            OnHungerChanged?.Invoke(Hunger);
        }

        private IEnumerator HungerDepletionRoutine(float hungerDepletionRate, float healthTickRate, float healthTickDamage)
        {
            while (!Health.IsDead && DepleteHunger)
            {
                yield return new WaitForSeconds(1f);
                Hunger -= hungerDepletionRate;

                if (Hunger <= 0)
                {
                    Health.TakeDamage(healthTickDamage);
                    yield return new WaitForSeconds(1f / healthTickRate);
                }
                else yield return null;
            }
        }
        #endregion
    }
}