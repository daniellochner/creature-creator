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
        [SerializeField] private float hungerDepletionRate = 1f / 1200f;
        [SerializeField] private float healthTickRate = 1f;
        [SerializeField] private float healthTickDamage = 5f;

        private Coroutine hungerDepletingCoroutine;
        #endregion

        #region Properties
        public CreatureHealth Health { get; private set; }
        public CreatureHunger Hunger { get; private set; }

        public bool DepleteHunger { get; set; } = true;
        #endregion

        #region Methods
        private void Awake()
        {
            Health = GetComponent<CreatureHealth>();
            Hunger = GetComponent<CreatureHunger>();
		}
        private void OnEnable()
        {
            if (hungerDepletingCoroutine != null)
            {
                StopCoroutine(hungerDepletingCoroutine);
            }
            hungerDepletingCoroutine = StartCoroutine(HungerDepletionRoutine(hungerDepletionRate, healthTickRate, healthTickDamage));
        }

        private IEnumerator HungerDepletionRoutine(float hungerDepletionRate, float healthTickRate, float healthTickDamage)
        {
            while (!Health.IsDead && DepleteHunger)
            {
                yield return new WaitForSeconds(1f);
                Hunger.Hunger -= hungerDepletionRate;

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