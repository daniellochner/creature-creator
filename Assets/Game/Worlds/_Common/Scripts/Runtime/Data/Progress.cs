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
        [SerializeField] private int cash = 1000;
        [SerializeField] private List<string> unlockedBodyParts = new List<string>();
        [SerializeField] private List<string> unlockedPatterns = new List<string>();
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
        #endregion

        #region Methods
        public override void Revert()
        {
            Experience = 0;
            Level = 0;
            Cash = 1000;
            UnlockedBodyParts.Clear();
            UnlockedPatterns.Clear();

            // Supporter Perks
            if (PlayerPrefs.GetString("IsSupporter") == "TRUE") // TODO: Use Steam API
            {
                GrantSupporterPerks();
            }

            // Early Access
            PlayerPrefs.SetInt("2g9ui01m", 0);
            PlayerPrefs.SetInt("h3hq9as4", 0);
        }

        public void GrantSupporterPerks()
        {
            UnlockedBodyParts.AddRange(SupporterPerks.BODY_PARTS);
            UnlockedPatterns.AddRange(SupporterPerks.PATTERNS);
            Cash += 100;
        }
        #endregion
    }
}