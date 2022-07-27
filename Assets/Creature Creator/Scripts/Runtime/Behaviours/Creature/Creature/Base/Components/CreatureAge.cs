// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureAge : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private NetworkVariable<int> age = new NetworkVariable<int>(0);
        #endregion

        #region Properties
        public Action<int> OnAgeChanged { get; set; }

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
        #endregion
    }
}