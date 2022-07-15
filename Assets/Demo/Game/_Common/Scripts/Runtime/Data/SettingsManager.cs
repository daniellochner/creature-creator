// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using IngameDebugConsole;
using UnityEngine;
using UnityEngine.Audio;
using static DanielLochner.Assets.CreatureCreator.Settings;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SettingsManager : DataManager<SettingsManager, Settings>
    {
        #region Fields
        [Header("Settings")]
        [SerializeField] private CreatureConstructor baseCreaturePrefab;
        [SerializeField] private AudioMixer masterAudioMixer;
        [SerializeField] private MinMax minMaxVolumeDB = new MinMax(-20, 1);
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }
        private void Setup()
        {
            SetResolution(Data.Resolution);
            SetFullscreen(Data.Fullscreen);
            SetVSync(Data.VSync);
            SetCreatureMeshQuality(Data.CreatureMeshQuality);
            SetShadowQuality(Data.ShadowQuality);
            SetTextureQuality(Data.TextureQuality);
            SetAmbientOcclusion(Data.AmbientOcclusion);
            SetAntialiasing(Data.Antialiasing);
            SetScreenSpaceReflections(Data.ScreenSpaceReflections);
            SetFoliage(Data.Foliage);
            SetBloom(Data.Bloom);
            SetDebugMode(Data.DebugMode);
            SetDepthOfField(Data.DepthOfField);
            SetMotionBlur(Data.MotionBlur);

            SetMasterVolume(Data.MasterVolume);
            SetMusicVolume(Data.MusicVolume);
            SetSoundEffectsVolume(Data.SoundEffectsVolume);
            SetBackgroundMusic(Data.BackgroundMusic);

            SetOnlineUsername(Data.OnlineUsername);
            SetCameraShake(Data.CameraShake);
            SetDebugMode(Data.DebugMode);
            SetPreviewFeatures(Data.PreviewFeatures);
            SetNetworkStats(Data.NetworkStats);
            SetTutorial(Data.Tutorial);
            SetWorldChat(Data.WorldChat);

            SetSensitivityHorizontal(Data.SensitivityHorizontal);
            SetSensitivityVertical(Data.SensitivityVertical);
            SetInvertHorizontal(Data.InvertHorizontal);
            SetInvertVertical(Data.InvertVertical);
        }

        #region Video
        public void SetResolution(Resolution resolution)
        {
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
            Data.Resolution = resolution;
        }
        public void SetFullscreen(bool fullscreen)
        {
            Screen.fullScreen = Data.Fullscreen = fullscreen;
        }
        public void SetVSync(bool vSync)
        {
            QualitySettings.vSyncCount = vSync ? 1 : 0;
            Data.VSync = vSync;
        }

        public void SetCreatureMeshQuality(CreatureMeshQualityType type)
        {
            BoneSettings boneSettings = baseCreaturePrefab.BoneSettings;
            switch (type)
            {
                case CreatureMeshQualityType.Low:
                    boneSettings.Segments = 8;
                    boneSettings.Rings = 2;
                    break;
                case CreatureMeshQualityType.Medium:
                    boneSettings.Segments = 14;
                    boneSettings.Rings = 4;
                    break;
                case CreatureMeshQualityType.High:
                    boneSettings.Segments = 20;
                    boneSettings.Rings = 8;
                    break;
            }
            Data.CreatureMeshQuality = type;
        }
        public void SetShadowQuality(ShadowQualityType type)
        {
            switch (type)
            {
                case ShadowQualityType.None:
                    break;
                case ShadowQualityType.Low:
                    break;
                case ShadowQualityType.Medium:
                    break;
                case ShadowQualityType.High:
                    break;
            }
            Data.ShadowQuality = type;
        }
        public void SetTextureQuality(TextureQualityType type)
        {

            Data.TextureQuality = type;
        }
        public void SetAmbientOcclusion(AmbientOcclusionType type)
        {
            Data.AmbientOcclusion = type;
        }
        public void SetAntialiasing(AntialiasingType type)
        {
            Data.Antialiasing = type;
        }
        public void SetScreenSpaceReflections(ScreenSpaceReflectionsType type)
        {
            Data.ScreenSpaceReflections = type;
        }
        public void SetFoliage(FoliageType type)
        {
            Data.Foliage = type;

            // rendering of grass and trees
            // low-poly wind shader
        }
        public void SetAnisotropicFiltering(bool anisotropicFiltering)
        {
            Data.AnisotropicFiltering = anisotropicFiltering;
        }
        public void SetBloom(bool bloom)
        {
            Data.Bloom = bloom;
        }
        public void SetDepthOfField(bool depthOfField)
        {
            Data.DepthOfField = depthOfField;
        }
        public void SetMotionBlur(bool motionBlur)
        {
            Data.MotionBlur = motionBlur;
        }
        #endregion

        #region Audio
        public void SetMasterVolume(float t)
        {
            Data.MasterVolume = t;
            SetMusicVolume(Data.MusicVolume);
            SetSoundEffectsVolume(Data.SoundEffectsVolume);
        }
        public void SetMusicVolume(float t)
        {
            Data.MusicVolume = t;
            SetVolume("MusicVolume", t);
        }
        public void SetSoundEffectsVolume(float t)
        {
            Data.SoundEffectsVolume = t;
            SetVolume("SoundEffectsVolume", t);
        }
        private void SetVolume(string param, float t)
        {
            t *= Data.MasterVolume;

            float volume = 0f;
            if (t == 0f)
            {
                volume = -80f;
            }
            else
            {
                volume = Mathf.Lerp(minMaxVolumeDB.min, minMaxVolumeDB.max, t);
            }
            masterAudioMixer.SetFloat(param, volume);
        }
        private float GetVolume(string param)
        {
            if (masterAudioMixer.GetFloat("param", out float value))
            {
                return Mathf.InverseLerp(minMaxVolumeDB.min, minMaxVolumeDB.max, value);
            }
            return 0f;
        }
        public void SetBackgroundMusic(BackgroundMusicType type)
        {
            Data.BackgroundMusic = type;
        }
        #endregion

        #region Gameplay
        public void SetOnlineUsername(string username)
        {
            Data.OnlineUsername = username;
        }
        public void SetCameraShake(bool cameraShake)
        {
            Data.CameraShake = cameraShake;
        }
        public void SetDebugMode(bool debugMode)
        {
            DebugLogManager.Instance.gameObject.SetActive(debugMode);
            Data.DebugMode = debugMode;
        }
        public void SetPreviewFeatures(bool previewFeatures)
        {
            Data.PreviewFeatures = previewFeatures;
        }
        public void SetNetworkStats(bool networkStats)
        {
            Data.NetworkStats = networkStats;
        }
        public void SetTutorial(bool tutorial)
        {
            Data.Tutorial = tutorial;
        }
        public void SetWorldChat(bool worldChat)
        {
            Data.WorldChat = worldChat;
        }
        #endregion

        #region Controls
        public void SetSensitivityHorizontal(float sensitivity)
        {
            Data.SensitivityHorizontal = sensitivity;
        }
        public void SetSensitivityVertical(float sensitivity)
        {
            Data.SensitivityVertical = sensitivity;
        }
        public void SetInvertHorizontal(bool invert)
        {
            Data.InvertHorizontal = invert;
        }
        public void SetInvertVertical(bool invert)
        {
            Data.InvertVertical = invert;
        }
        #endregion
        #endregion
    }
}