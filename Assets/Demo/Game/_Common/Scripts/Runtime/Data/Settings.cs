// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class Settings : Data
    {
        #region Fields
        [Header("Video")]
        [SerializeField] private Vector2Int displaySize;
        [SerializeField] private int refreshRate;
        [SerializeField] private bool fullscreen;
        [SerializeField] private bool vSync;
        [Space]
        [SerializeField] private CreatureMeshQualityType creatureMeshQuality;
        [SerializeField] private ShadowQualityType shadowQuality;
        [SerializeField] private TextureQualityType textureQuality;
        [SerializeField] private AmbientOcclusionType ambientOcclusion;
        [SerializeField] private AntialiasingType antialiasing;
        [SerializeField] private ScreenSpaceReflectionsType screenSpaceReflections;
        [SerializeField] private bool anisotropicFiltering ;
        [SerializeField] private bool bloom;
        [SerializeField] private bool depthOfField;
        [SerializeField] private bool motionBlur;

        [Header("Audio")]
        [SerializeField, Range(0, 1)] private float masterVolume;
        [SerializeField, Range(0, 1)] private float musicVolume;
        [SerializeField, Range(0, 1)] private float soundEffectsVolume;

        [Header("Gameplay")]
        [SerializeField] private string onlineUsername;
        [SerializeField] private List<CreatureData> creaturePresets;
        [SerializeField] private List<string> hiddenBodyParts;
        [SerializeField] private List<string> hiddenPatterns;
        [SerializeField] private bool cameraShake;
        [SerializeField] private bool debugMode;
        [SerializeField] private bool previewFeatures;
        [SerializeField] private bool networkStats;
        [SerializeField] private bool tutorial;
        [SerializeField] private bool worldChat;

        [Header("Controls")]
        [SerializeField, Range(0, 1)] private float sensitivityHorizontal;
        [SerializeField, Range(0, 1)] private float sensitivityVertical;
        [SerializeField] private bool invertHorizontal;
        [SerializeField] private bool invertVertical;
        #endregion

        #region Properties
        public Resolution Resolution
        {
            get => new Resolution()
            {
                width = displaySize.x,
                height = displaySize.y,
                refreshRate = refreshRate
            };
            set
            {
                displaySize = new Vector2Int(value.width, value.height);
                refreshRate = value.refreshRate;
            }
        }
        public bool Fullscreen
        {
            get => fullscreen;
            set => fullscreen = value;
        }
        public bool VSync
        {
            get => vSync;
            set => vSync = value;
        }
        public CreatureMeshQualityType CreatureMeshQuality
        {
            get => creatureMeshQuality;
            set => creatureMeshQuality = value;
        }
        public ShadowQualityType ShadowQuality
        {
            get => shadowQuality;
            set => shadowQuality = value;
        }
        public TextureQualityType TextureQuality
        {
            get => textureQuality;
            set => textureQuality = value;
        }
        public AmbientOcclusionType AmbientOcclusion
        {
            get => ambientOcclusion;
            set => ambientOcclusion = value;
        }
        public AntialiasingType Antialiasing
        {
            get => antialiasing;
            set => antialiasing = value;
        }
        public ScreenSpaceReflectionsType ScreenSpaceReflections
        {
            get => screenSpaceReflections;
            set => screenSpaceReflections = value;
        }
        public bool AnisotropicFiltering
        {
            get => anisotropicFiltering;
            set => anisotropicFiltering = value;
        }
        public bool Bloom
        {
            get => bloom;
            set => bloom = value;
        }
        public bool DepthOfField
        {
            get => depthOfField;
            set => depthOfField = value;
        }
        public bool MotionBlur
        {
            get => motionBlur;
            set => motionBlur = value;
        }

        public float MasterVolume
        {
            get => masterVolume;
            set => masterVolume = Mathf.Clamp01(value);
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

        public string OnlineUsername
        {
            get => onlineUsername;
            set => onlineUsername = value;
        }
        public List<CreatureData> CreaturePresets
        {
            get => creaturePresets;
        }
        public List<string> HiddenBodyParts
        {
            get => hiddenBodyParts;
        }
        public List<string> HiddenPatterns
        {
            get => hiddenPatterns;
        }
        public bool CameraShake
        {
            get => cameraShake;
            set => cameraShake = value;
        }
        public bool DebugMode
        {
            get => debugMode;
            set => debugMode = value;
        }
        public bool PreviewFeatures
        {
            get => previewFeatures;
            set => previewFeatures = value;
        }
        public bool NetworkStats
        {
            get => networkStats;
            set => networkStats = value;
        }
        public bool Tutorial
        {
            get => tutorial;
            set => tutorial = value;
        }
        public bool WorldChat
        {
            get => worldChat;
            set => worldChat = value;
        }

        public float SensitivityHorizontal
        {
            get => sensitivityHorizontal;
            set => sensitivityHorizontal = value;
        }
        public float SensitivityVertical
        {
            get => sensitivityVertical;
            set => sensitivityVertical = value;
        }
        public bool InvertHorizontal
        {
            get => invertHorizontal;
            set => invertHorizontal = value;
        }
        public bool InvertVertical
        {
            get => invertVertical;
            set => invertVertical = value;
        }
        #endregion

        #region Methods
        public override void Revert()
        {
            Resolution = Screen.currentResolution;
            Fullscreen = Screen.fullScreen;
            VSync = false;

            CreatureMeshQuality = CreatureMeshQualityType.Medium;
            ShadowQuality = ShadowQualityType.Medium;
            TextureQuality = TextureQualityType.Medium;
            Antialiasing = AntialiasingType.FXAA;
            ScreenSpaceReflections = ScreenSpaceReflectionsType.Medium;
            AnisotropicFiltering = true;
            Bloom = true;
            DepthOfField = false;
            MotionBlur = false;

            MasterVolume = 1f;
            MusicVolume = 0.75f;
            SoundEffectsVolume = 0.75f;

            OnlineUsername = "";
            CreaturePresets.Clear();
            CameraShake = true;
            DebugMode = false;
            PreviewFeatures = false;
            NetworkStats = true;
            Tutorial = true;
            WorldChat = true;

            SensitivityHorizontal = 0.5f;
            SensitivityVertical = 0.5f;
            InvertHorizontal = false;
            InvertVertical = false;
        }
        #endregion

        #region Enums
        public enum PresetType
        {
            Custom,
            VeryLow,
            Low,
            Medium,
            High,
            VeryHigh
        }
        public enum CreatureMeshQualityType
        {
            Low,
            Medium,
            High
        }
        public enum ShadowQualityType
        {
            None,
            Low,
            Medium,
            High
        }
        public enum TextureQualityType
        {
            Low,
            Medium,
            High
        }
        public enum AmbientOcclusionType
        {
            None,
            SAO,
            MSVO
        }
        public enum AntialiasingType
        {
            None,
            FXAA,
            LowSMAA,
            MediumSMAA,
            HighSMAA
        }
        public enum ScreenSpaceReflectionsType
        {
            None,
            Low,
            Medium,
            High
        }
        #endregion
    }
}