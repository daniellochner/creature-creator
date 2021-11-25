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
        [SerializeField] private Vector2Int resolution = new Vector2Int(Screen.width, Screen.height);
        [SerializeField] private bool fullscreen = true;
        [SerializeField] private bool vSync = false;
        [Space]
        [SerializeField] private CreatureMeshQualityType creatureMeshQuality = CreatureMeshQualityType.Medium;
        [SerializeField] private ShadowQualityType shadowQuality = ShadowQualityType.Medium;
        [SerializeField] private TextureQualityType textureQuality = TextureQualityType.Medium;
        [SerializeField] private AmbientOcclusionType ambientOcclusion = AmbientOcclusionType.MSVO;
        [SerializeField] private AntialiasingType antialiasing = AntialiasingType.HighSMAA;
        [SerializeField] private ScreenSpaceReflectionType screenSpaceReflection = ScreenSpaceReflectionType.Medium;
        [SerializeField] private bool anisotropicFiltering = true;
        [SerializeField] private bool bloom = true;
        [SerializeField] private bool depthOfField = true;
        [SerializeField] private bool motionBlur = false;

        [Header("Audio")]
        [SerializeField, Range(0, 1)] private float masterVolume = 1f;
        [SerializeField, Range(0, 1)] private float musicVolume = 0.75f;
        [SerializeField, Range(0, 1)] private float soundEffectsVolume = 0.75f;

        [Header("Gameplay")]
        [SerializeField] private string username = "Creature";
        [SerializeField] private List<string> creaturePresets = new List<string>();
        [SerializeField] private bool debugMode = false;
        [SerializeField] private bool previewFeatures = true;
        [SerializeField] private bool networkStats = true;
        [SerializeField] private bool tutorial = true;
        [SerializeField] private bool worldChat = true;

        [Header("Controls")]
        [SerializeField, Range(0, 1)] private float cameraSensitivityHorizontal = 0.5f;
        [SerializeField, Range(0, 1)] private float cameraSensitivityVertical = 0.5f;
        [SerializeField] private bool invertHorizontal = false;
        [SerializeField] private bool invertVertical = false;
        #endregion

        #region Properties
        public Vector2Int Resolution
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
        public ScreenSpaceReflectionType ScreenSpaceReflection
        {
            get => screenSpaceReflection;
            set => screenSpaceReflection = value;
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
            set => masterVolume = value;
        }
        public float MusicVolume
        {
            get => musicVolume;
            set => musicVolume = value;
        }
        public float SoundEffectsVolume
        {
            get => soundEffectsVolume;
            set => soundEffectsVolume = value;
        }

        public string Username
        {
            get => username;
            set => username = value;
        }
        public List<string> CreaturePresets
        {
            get => creaturePresets;
            set => creaturePresets = value;
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

        public float CameraSensitivityHorizontal
        {
            get => cameraSensitivityHorizontal;
            set => cameraSensitivityHorizontal = value;
        }
        public float CameraSensitivityVertical
        {
            get => cameraSensitivityVertical;
            set => cameraSensitivityVertical = value;
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
        public void Revert()
        {

        }
        #endregion

        #region Enums
        public enum PresetType
        {
            Low,
            Medium,
            High,
            Custom
        }
        public enum CreatureMeshQualityType
        {
            Low,
            Medium,
            High
        }
        public enum ShadowQualityType
        {
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
        public enum ScreenSpaceReflectionType
        {
            None,
            Low,
            Medium,
            High
        }
        #endregion
    }
}