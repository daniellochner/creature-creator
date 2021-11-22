// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureHealth))]
    public class CreatureEnergy : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float energyDepletionRate = 1f / 1200f;
        [SerializeField] private float healthTickRate = 1f;
        [SerializeField] private float healthTickDamage = 5f;
        [Space]
        [SerializeField, ReadOnly] private float energy = 1f;
        #endregion

        #region Properties
        private CreatureHealth CreatureHealth { get; set; }

        public Action<float> OnEnergyChanged { get; set; }

        public float Energy
        {
            get => energy;
            set
            {
                energy = Mathf.Clamp01(value);
                OnEnergyChanged?.Invoke(energy);
            }
        }
        #endregion

        #region Methods
        private void Awake()
        {
            CreatureHealth = GetComponent<CreatureHealth>();
        }
        private void Start()
        {
            StartCoroutine(EnergyDepletionRoutine(energyDepletionRate, healthTickRate, healthTickDamage));
        }

        private IEnumerator EnergyDepletionRoutine(float energyDepletionRate, float healthTickRate, float healthTickDamage)
        {
            while (!CreatureHealth.IsDead)
            {
                yield return new WaitForSeconds(1f);
                Energy -= energyDepletionRate;

                if (Energy <= 0)
                {
                    CreatureHealth.TakeDamage(healthTickDamage);
                    yield return new WaitForSeconds(1f / healthTickRate);
                }
                else yield return null;
            }
        }
        #endregion
    }
}