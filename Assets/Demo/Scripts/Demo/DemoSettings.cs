// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class DemoSettings
    {
        #region Fields
        [Header("Video")]
        [SerializeField] private Resolution resolution = new Resolution();
        [SerializeField] private bool fullscreen = true;
        [SerializeField] private bool vSync = false;
        [Space]
        [SerializeField] private CreatureMeshQualityType creatureMeshQuality = CreatureMeshQualityType.Medium;
        [SerializeField] private ShadowQualityType shadowQuality = ShadowQualityType.Medium;
        [SerializeField] private TextureQualityType textureQuality = TextureQualityType.Medium;
        [SerializeField] private AmbientOcclusionType ambientOcclusion = AmbientOcclusionType.MSVO;
        [SerializeField] private AntialiasingType antialiasing = AntialiasingType.HighSMAA;
        [SerializeField] private ScreenSpaceReflectionsType screenSpaceReflections = ScreenSpaceReflectionsType.Medium;
        [SerializeField] private bool anisotropicFiltering = true;
        [SerializeField] private bool bloom = true;
        [SerializeField] private bool depthOfField = true;
        [SerializeField] private bool motionBlur = false;

        [Header("Audio")]
        [SerializeField, Range(0, 1)] private float masterVolume = 1f;
        [SerializeField, Range(0, 1)] private float musicVolume = 0.75f;
        [SerializeField, Range(0, 1)] private float soundEffectsVolume = 0.75f;

        [Header("Gameplay")]
        [SerializeField] private string onlineUsername = "";
        [SerializeField] private List<CreatureData> creaturePresets = new List<CreatureData>();
        [SerializeField] private bool cameraShake = true;
        [SerializeField] private bool debugMode = false;
        [SerializeField] private bool previewFeatures = true;
        [SerializeField] private bool networkStats = true;
        [SerializeField] private bool tutorial = true;
        [SerializeField] private bool worldChat = true;

        [Header("Controls")]
        [SerializeField, Range(0, 1)] private float sensitivityHorizontal = 0.5f;
        [SerializeField, Range(0, 1)] private float sensitivityVertical = 0.5f;
        [SerializeField] private bool invertHorizontal = false;
        [SerializeField] private bool invertVertical = false;
        #endregion

        #region Properties
        public Resolution Resolution
        {
            get => resolution;
            set => resolution = value;
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
            set => creaturePresets = value;
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