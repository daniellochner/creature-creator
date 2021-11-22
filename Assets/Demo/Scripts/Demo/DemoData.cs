// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class DemoData
    {
        #region Fields
        [SerializeField, Range(0, 100)] private float experience = 0;
        [SerializeField, Range(0, 50)] private int level = 0;
        [SerializeField] private int startingCash = 1000;
        [SerializeField] private List<string> unlockedBodyParts = new List<string>();
        [SerializeField] private List<string> unlockedPatterns = new List<string>();
        [Space]
        [SerializeField] private int displaySize = 0;
        [SerializeField] private int qualityLevel = 2;
        [SerializeField, Range(0, 1)] private float musicVolume = 0.75f;
        [SerializeField, Range(0, 1)] private float soundEffectsVolume = 0.75f;
        [SerializeField] private bool debugMode = false;
        [SerializeField] private bool hideChat = false;
        [SerializeField] private bool previewFeatures = false;
        [SerializeField] private bool shownHint = false;
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
        public int DisplaySize
        {
            get => displaySize;
            set => displaySize = value;
        }
        public int QualityLevel
        {
            get => qualityLevel;
            set => qualityLevel = value;
        }
        public float MusicVolume
        {
            get => musicVolume;
            set => musicVolume = Mathf.Clamp01(value);
        }
        public float SoundEffectsVolume
        {
            get => soundEffectsVolume;
            set => soundEffectsVolume = Mathf.Clamp01(value);
        }
        public bool DebugMode
        {
            get => debugMode;
            set => debugMode = value;
        }
        public bool HideChat
        {
            get => hideChat;
            set => hideChat = value;
        }
        public bool PreviewFeatures
        {
            get => previewFeatures;
            set => previewFeatures = value;
        }
        public bool ShownHint
        {
            get => shownHint;
            set => shownHint = value;
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
            DisplaySize = 0;
            QualityLevel = 2;
            MusicVolume = 0.75f;
            SoundEffectsVolume = 0.75f;
            DebugMode = false;
            HideChat = false;
            PreviewFeatures = false;
            ShownHint = false;
        }
        #endregion
    }
}