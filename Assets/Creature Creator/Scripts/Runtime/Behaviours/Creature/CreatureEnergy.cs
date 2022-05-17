// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureEnergy : MonoBehaviour
    {
        #region Fields
        [SerializeField, ReadOnly] private float energy = 1f;
        #endregion

        #region Properties
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
    }
}