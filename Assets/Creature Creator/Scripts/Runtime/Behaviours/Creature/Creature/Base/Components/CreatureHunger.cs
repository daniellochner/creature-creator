// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;
using Unity.Netcode;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureHunger : NetworkBehaviour
    {
        #region Fields
        [SerializeField, ReadOnly] private NetworkVariable<float> hunger = new NetworkVariable<float>(1f);
        #endregion

        #region Properties
        public Action<float> OnHungerChanged { get; set; }

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
        #endregion
    }
}