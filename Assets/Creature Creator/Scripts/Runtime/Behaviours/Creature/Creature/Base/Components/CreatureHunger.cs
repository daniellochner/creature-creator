// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureHunger : MonoBehaviour
    {
        #region Fields
        [SerializeField, ReadOnly] private float hunger = 1f;
        #endregion

        #region Properties
        public Action<float> OnHungerChanged { get; set; }

        public float Hunger
        {
            get => hunger;
            set
            {
                hunger = Mathf.Clamp01(value);
                OnHungerChanged?.Invoke(hunger);
            }
        }
        #endregion
    }
}