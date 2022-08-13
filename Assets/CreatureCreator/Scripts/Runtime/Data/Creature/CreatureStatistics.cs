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
        [SerializeField] private int health = 0;
        [SerializeField] private float weight = 0f;
        [SerializeField] private float speed = 1f;
        #endregion

        #region Properties
        public int Complexity
        {
            get => complexity;
            set
            {
                complexity = Mathf.Clamp(value, 0, 100);
            }
        }
        public List<Diet> Diets
        {
            get => diets;
        }
        public int Health
        {
            get => health;
            set
            {
                health = Mathf.Clamp(value, 0, 1000);
            }
        }
        public float Weight
        {
            get => weight;
            set
            {
                weight = Mathf.Clamp(value, 0, 1000);
            }
        }
        public float Speed
        {
            get => speed;
            set
            {
                speed = Mathf.Clamp(value, 0f, 3f);
            }
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
            health = 0;
            weight = 0f;
            speed = 1f;
        }
        #endregion
    }
}