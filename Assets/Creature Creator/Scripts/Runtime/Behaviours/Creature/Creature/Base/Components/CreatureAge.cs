// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureHealth))]
    [RequireComponent(typeof(CreatureHider))]
    public class CreatureAge : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private NetworkVariable<int> age = new NetworkVariable<int>(0);
        private Coroutine agingRoutine;
        #endregion

        #region Properties
        public Action<int> OnAgeChanged { get; set; }
        
        public CreatureHealth Health { get; private set; }
        public CreatureHider Hider { get; private set; }

        public int Age
        {
            get => age.Value;
            set
            {
                if (IsServer)
                {
                    age.Value = Mathf.Max(0, value);
                }
                else
                {
                    SetAgeServerRpc(value);
                }
            }
        }
        #endregion

        #region Methods
        private void Awake()
        {
            Health = GetComponent<CreatureHealth>();
            Hider = GetComponent<CreatureHider>();
        }
        private void Start()
        {
            age.OnValueChanged += UpdateAge;
            age.SetDirty(true);

            if (IsServer)
            {
                Hider.OnShow += delegate
                {
                    agingRoutine = StartCoroutine(AgingRoutine());
                };
                Hider.OnHide += delegate
                {
                    if (agingRoutine != null)
                    {
                        StopCoroutine(agingRoutine);
                    }
                    Age = 0;
                };
            }
        }

        [ServerRpc]
        private void SetAgeServerRpc(int a)
        {
            age.Value = a;
        }
        private void UpdateAge(int oldAge, int newAge)
        {
            Age = newAge;
            OnAgeChanged?.Invoke(Age);
        }

        private IEnumerator AgingRoutine()
        {
            while (!Health.IsDead)
            {
                yield return new WaitForSeconds(1f);
                Age++;
            }
        }
        #endregion
    }
}