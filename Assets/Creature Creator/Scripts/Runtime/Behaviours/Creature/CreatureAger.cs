// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureAge))]
    public class CreatureAger : MonoBehaviour
    {
        #region Fields
        private Coroutine agingRoutine;
        #endregion

        #region Properties
        public CreatureAge Age { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Age = GetComponent<CreatureAge>();
        }

        public void Start()
        {
            Age.Age = 0;

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
                Age.Age++;
            }
        }
        #endregion
    }
}