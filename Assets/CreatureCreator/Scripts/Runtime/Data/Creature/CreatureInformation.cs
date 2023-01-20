// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class CreatureInformation
    {
        #region Fields
        [SerializeField] private string name = "Unnamed";
        [SerializeField] private Texture2D photo = null;
        [SerializeField, Range(0, 1)] private float health = 1f;
        [SerializeField, Range(0, 1)] private float hunger = 1f;
        [SerializeField] private int age = 0; // Lifetime (in seconds) since construction.
        #endregion

        #region Properties
        public string Name
        {
            get => name;
            set => name = string.IsNullOrEmpty(value) ? "Unnamed" : value;
        }
        public Texture2D Photo
        {
            get => photo;
            set => photo = value;
        }
        public float Health
        {
            get => health;
            set => health = Mathf.Clamp01(value);
        }
        public float Hunger
        {
            get => hunger;
            set => hunger = Mathf.Clamp01(value);
        }
        public int Age
        {
            get => age;
            set => age = Mathf.Max(0, value);
        }

        public string FormattedAge
        {
            get
            {
                int minutes = age / 60;
                int seconds = age % 60;

                return $"{minutes}m{seconds}s";
            }
        }
        #endregion

        #region Methods
        public void Reset()
        {
            name = LocalizeUtility.Localize("creature-unnamed");
            photo = null;
            health = 1f;
            hunger = 1f;
            age = 0;
        }
        #endregion
    }
}