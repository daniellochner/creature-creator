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
        public int complexity = 0;
        public List<Diet> diets = new List<Diet>();
        public int health = 0;
        [SerializeField] private float speed = 1f;
        public float weight = 0f;
        #endregion

        #region Properties
        public Diet Diet
        {
            get
            {
                Diet diet = Diet.None;

                foreach (Diet d in diets)
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

        public float Speed
        {
            get => speed;
            set
            {
                speed = Mathf.Clamp(value, 0f, 5f);
            }
        }
        #endregion
    }
}