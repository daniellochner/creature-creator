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
        [SerializeField] private List<string> unlockedBodyParts = new List<string>();
        [SerializeField] private List<string> unlockedPatterns = new List<string>();
        [SerializeField] private List<string> completedQuests = new List<string>();
        [SerializeField] private List<Map> unlockedMaps = new List<Map>();
        [SerializeField] private List<Map> reachedPeaks = new List<Map>();
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
        public List<string> UnlockedBodyParts
        {
            get => unlockedBodyParts;
        }
        public List<string> UnlockedPatterns
        {
            get => unlockedPatterns;
        }
        public List<string> CompletedQuests
        {
            get => completedQuests;
        }
        public List<Map> UnlockedMaps
        {
            get => unlockedMaps;
        }
        public List<Map> ReachedPeaks
        {
            get => reachedPeaks;
        }
        #endregion

        #region Methods
        public override void Revert()
        {
            Experience = 0;
            Level = 0;
            UnlockedBodyParts.Clear();
            UnlockedPatterns.Clear();
            CompletedQuests.Clear();
            UnlockedMaps.Clear();
            ReachedPeaks.Clear();

            UnlockedMaps.Add(Map.Island);
        }
        #endregion
    }
}