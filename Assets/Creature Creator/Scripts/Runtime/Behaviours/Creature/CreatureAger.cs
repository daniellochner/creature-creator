// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureAger : MonoBehaviour
    {
        #region Fields
        [SerializeField, ReadOnly] private int age;

        private Coroutine agingRoutine;
        #endregion

        #region Properties
        public Action<int> OnAgeChanged { get; set; }

        public int Age
        {
            get => age;
            private set
            {
                age = Mathf.Max(0, value);
                OnAgeChanged?.Invoke(age);
            }
        }
        #endregion

        #region Methods
        public void Start()
        {
            Age = 0;

            if (agingRoutine != null)
            {
                StopCoroutine(agingRoutine);
            }
            agingRoutine = StartCoroutine(AgingRoutine());
        }
        private IEnumerator AgingRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                Age++;
            }
        }
        #endregion
    }
}