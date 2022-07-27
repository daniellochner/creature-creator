// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
	[RequireComponent(typeof(CreatureHealth))]
	[RequireComponent(typeof(CreatureHunger))]
    public class CreatureHungerDepleter : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float energyDepletionRate = 1f / 1200f;
        [SerializeField] private float healthTickRate = 1f;
        [SerializeField] private float healthTickDamage = 5f;

        private Coroutine energyDepletingCoroutine;
        #endregion

        #region Properties
        public CreatureHealth Health { get; private set; }
        public CreatureHunger Hunger { get; private set; }

        public Action<float> OnEnergyChanged { get; set; }
        public bool DepleteEnergy { get; set; } = true;
        #endregion

        #region Methods
        private void Awake()
        {
            Health = GetComponent<CreatureHealth>();
            Hunger = GetComponent<CreatureHunger>();
		}
        private void OnEnable()
        {
            if (energyDepletingCoroutine != null)
            {
                StopCoroutine(energyDepletingCoroutine);
            }
            energyDepletingCoroutine = StartCoroutine(EnergyDepletionRoutine(energyDepletionRate, healthTickRate, healthTickDamage));
        }

        private IEnumerator EnergyDepletionRoutine(float energyDepletionRate, float healthTickRate, float healthTickDamage)
        {
            while (!Health.IsDead && DepleteEnergy)
            {
                yield return new WaitForSeconds(1f);
                Hunger.Hunger -= energyDepletionRate;

                if (Hunger.Hunger <= 0)
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