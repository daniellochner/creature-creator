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
        [SerializeField] private int experience = 0;
        [SerializeField] private List<string> unlockedBodyParts = new List<string>();
        [SerializeField] private List<string> unlockedPatterns = new List<string>();
        [SerializeField] private List<string> completedQuests = new List<string>();
        [SerializeField] private List<Map> unlockedMaps = new List<Map>();
        [SerializeField] private List<Map> reachedPeaks = new List<Map>();
        #endregion

        #region Properties
        public static int ExperiencePerLevel = 100;
        public static int MaxLevel = 50;

        public int Experience
        {
            get => experience;
            set => experience = value;
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

        public int Level
        {
            get
            {
                return Mathf.Min(Experience / ExperiencePerLevel, MaxLevel);
            }
        }
        #endregion

        #region Methods
        public override void Revert()
        {
            Experience = 0;
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