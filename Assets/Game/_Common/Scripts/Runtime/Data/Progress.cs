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
        #endregion

        #region Properties
        public static string[] Quests = new string[]
        {
            "quest_is75n9d",
            "quest_07dm32j",
            "quest_sl1c8xz",
            "quest_8nsgy3m",
            "quest_9js6hk4",
            "quest_9n5pdf6",
            "quest_j5pz7s0",
            "quest_8s7s83i",
            "quest_lo4zz8f",
            "quest_01lfpx7",
            "quest_mn72a0b",
            "quest_f8s5x02",
            "quest_27dh3g2"
        };
        public static string[] HighestPoints = new string[]
        {
            "HP_ISLAND",
            "HP_SANDBOX",
            "HP_FARM"
        };


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
        #endregion

        #region Methods
        public override void Revert()
        {
            Experience = 0;
            Level = 0;
            Cash = 500;
            UnlockedBodyParts.Clear();
            UnlockedPatterns.Clear();

            foreach (string questId in Quests)
            {
                PlayerPrefs.SetInt(questId, 0);
            }
            foreach (string highestPointId in HighestPoints)
            {
                PlayerPrefs.SetInt(highestPointId, 0);
            }
        }
        #endregion
    }
}