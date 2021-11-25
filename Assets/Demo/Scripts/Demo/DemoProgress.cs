// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class DemoProgress
    {
        #region Fields
        [SerializeField, Range(0, 100)] private float experience = 0;
        [SerializeField, Range(0, 50)] private int level = 0;
        [SerializeField] private int startingCash = 1000;
        [SerializeField] private List<string> unlockedBodyParts = new List<string>();
        [SerializeField] private List<string> unlockedPatterns = new List<string>();
        #endregion

        #region Properties
        public float Experience
        {
            get => experience;
            set
            {
                experience = value;
                if (experience >= MaxExperience)
                {
                    if (level >= MaxLevel)
                    {
                        experience = MaxExperience;
                    }
                    else
                    {
                        float levels = experience / MaxExperience;
                        level += (int)levels;
                        experience = (levels - (int)levels) * MaxExperience;
                    }
                }
            }
        }
        public int Level
        {
            get => level;
            set
            {
                level = value;
                if (level >= MaxLevel)
                {
                    level = MaxLevel;
                }
            }
        }
        public int StartingCash
        {
            get => startingCash;
            set => startingCash = value;
        }
        public List<string> UnlockedBodyParts
        {
            get => unlockedBodyParts;
            set => unlockedBodyParts = value;
        }
        public List<string> UnlockedPatterns
        {
            get => unlockedPatterns;
            set => unlockedPatterns = value;
        }

        public static float MaxExperience = 100;
        public static int MaxLevel = 50;
        #endregion

        #region Methods
        public void Revert()
        {
            Experience = 0;
            Level = 0;
            StartingCash = 1000;
            UnlockedBodyParts.Clear();
            UnlockedPatterns.Clear();
        }
        #endregion
    }
}