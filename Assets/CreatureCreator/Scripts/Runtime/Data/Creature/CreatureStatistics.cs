// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class CreatureStatistics
    {
        #region Fields
        [SerializeField] private int complexity = 0;
        [SerializeField] private List<Diet> diets = new List<Diet>();

        [SerializeField] private float weightBody = 0f;
        [SerializeField] private float weightBodyParts = 0f;

        [SerializeField] private float speedBody = 0f;
        [SerializeField] private float speedBodyParts = 0f;

        [SerializeField] private int healthBody = 0;
        [SerializeField] private int healthBodyParts = 0;
        #endregion

        #region Properties
        public int Complexity
        {
            get => complexity;
            set
            {
                complexity = value;//Mathf.Clamp(value, 0, 100);
            }
        }
        public List<Diet> Diets
        {
            get => diets;
        }

        public float Weight
        {
            get => Mathf.Clamp(0f, WeightBody + WeightBodyParts, 1000f);
        }
        public float WeightBody
        {
            get => weightBody;
            set => weightBody = value;
        }
        public float WeightBodyParts
        {
            get => weightBodyParts;
            set => weightBodyParts = value;
        }
        
        public int Health
        {
            get => Mathf.Clamp(100 + HealthBody + HealthBodyParts, 0, 1000);
        }
        public int HealthBody
        {
            get => healthBody;
            set => healthBody = value;
        }
        public int HealthBodyParts
        {
            get => healthBodyParts;
            set => healthBodyParts = value;
        }

        public float Speed
        {
            get => Mathf.Clamp(1f + SpeedBody + SpeedBodyParts + SpeedBoost, 0f, 3f);
        }
        public float SpeedBody
        {
            get => speedBody;
            set => speedBody = value;
        }
        public float SpeedBodyParts
        {
            get => speedBodyParts;
            set => speedBodyParts = value;
        }
        public float SpeedBoost
        {
            get; set;
        }

        public Diet Diet
        {
            get
            {
                Diet diet = Diet.None;

                foreach (Diet d in Diets)
                {
                    if (d == Diet.Carnivore)
                    {
                        diet = (diet == Diet.Herbivore) ? Diet.Omnivore : Diet.Carnivore;
                    }
                    else if (d == Diet.Herbivore)
                    {
                        diet = (diet == Diet.Carnivore) ? Diet.Omnivore : Diet.Herbivore;
                    }
                    else
                    {
                        diet = Diet.Omnivore;
                    }

                    if (diet == Diet.Omnivore) { break; } // Omnivore is the preferred diet.
                }

                return diet;
            }
        }
        #endregion

        #region Methods
        public void Reset()
        {
            complexity = 0;
            diets.Clear();
            healthBodyParts = 0;
            weightBodyParts = 0f;
            speedBodyParts = 0f;
        }
        #endregion
    }
}