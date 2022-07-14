// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureHealth))]
    [RequireComponent(typeof(CreatureAge))]
    public class CreatureAger : MonoBehaviour
    {
        #region Fields
        private Coroutine agingRoutine;
        #endregion

        #region Properties
        public CreatureHealth Health { get; private set; }
        public CreatureAge Age { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Health = GetComponent<CreatureHealth>();
            Age = GetComponent<CreatureAge>();
        }

        public void OnEnable()
        {
            if (agingRoutine != null)
            {
                StopCoroutine(agingRoutine);
            }
            agingRoutine = StartCoroutine(AgingRoutine());
        }

        private IEnumerator AgingRoutine()
        {
            while (!Health.IsDead)
            {
                yield return new WaitForSeconds(1f);
                Age.Age++;
            }
        }
        #endregion
    }
}