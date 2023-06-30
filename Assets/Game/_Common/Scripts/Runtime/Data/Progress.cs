// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class Progress : Data
    {
        #region Fields
        [SerializeField, Range(0, 1)] private float experience = 0;
        [SerializeField, Range(0, 50)] private int level = 0;
        [SerializeField] private int cash = 500;
        [SerializeField] private List<string> unlockedBodyParts = new List<string>();
        [SerializeField] private List<string> unlockedPatterns = new List<string>();

        public readonly string[] QUESTS = new string[]
        {
            // Island
            "quest_27dh3g2",

            // Farm
            "quest_9n5pdf6",
            "quest_j5pz7s0",
            "quest_8s7s83i",
            "quest_lo4zz8f",
            "quest_01lfpx7",
            "quest_mn72a0b",
            "quest_f8s5x02",

            // Sandbox
            "quest_8nsgy3m",
            "quest_9js6hk4",

            // Cave
            "quest_k2nx0l",

            // City
            "quest_fkfnwa"
        };
        #endregion

        #region Properties
        public static float MaxExperience = 100;
        public static int MaxLevel = 50;

        public float Experience
        {
            get => experience;
            set
            {
                experience = value;
                if (experience >= MaxExperience)
                {
                    if (Level >= MaxLevel)
                    {
                        experience = MaxExperience;
                    }
                    else
                    {
                        float levels = experience / MaxExperience;
                        Level += (int)levels;
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
        public int Cash
        {
            get => cash;
            set => cash = value;
        }
        public List<string> UnlockedBodyParts
        {
            get => unlockedBodyParts;
        }
        public List<string> UnlockedPatterns
        {
            get => unlockedPatterns;
        }
        public int CompletedQuests
        {
            get
            {
                int counter = 0;
                foreach (string quest in QUESTS)
                {
                    if (PlayerPrefs.GetInt(quest) == 1)
                    {
                        counter++;
                    }
                }
                return counter;
            }
        }
        #endregion

        #region Methods
        public override void Revert()
        {
            PlayerPrefs.DeleteAll();
            Experience = 0;
            Level = 0;
            Cash = 500;
            UnlockedBodyParts.Clear();
            UnlockedPatterns.Clear();
        }
        #endregion
    }
}