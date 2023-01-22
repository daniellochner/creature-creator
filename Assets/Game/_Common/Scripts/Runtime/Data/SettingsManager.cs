// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using IngameDebugConsole;
using Pinwheel.Griffin;
using Pinwheel.Poseidon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering.PostProcessing;
using static DanielLochner.Assets.CreatureCreator.Settings;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SettingsManager : DataManager<SettingsManager, Settings>
    {
        #region Fields
        [Header("Settings")]
        [SerializeField] private CreatureQuality creatureQualitySettings;
        [SerializeField] private GameObject[] cameras;
        [SerializeField] private PostProcessProfile[] profiles;
        [SerializeField] private PWaterProfile[] waterProfiles;
        [SerializeField] private GRendering[] renderingProfiles;
        [SerializeField] private Material[] windMaterials;
        [Space]
        [SerializeField] private AudioMixer masterAudioMixer;
        [SerializeField] private MinMax minMaxVolumeDB = new MinMax(-20, 1);
        [Space]
        [SerializeField] private NetworkStatsManager statsManagerPrefab;
        [SerializeField] private GameObject creatureBasePrefab;
        [Space]
        [SerializeField] private CameraOrbit cameraOrbitPrefab;
        #endregion

        #region Methods
        protected override void Start()
        {
            base.Start();



            // TODO: Revert settings back to defaults for everyone!
            // Remove this in the next update...
            if (PlayerPrefs.GetInt("REVERT_SETTINGS") == 0)
            {
                Revert();
                PlayerPrefs.SetInt("REVERT_SETTINGS", 1);
            }



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
            SetReflections(Data.Reflections);
            SetAnisotropicFiltering(Data.AnisotropicFiltering);
            SetBloom(Data.Bloom);
            SetDebugMode(Data.DebugMode);
            SetDepthOfField(Data.DepthOfField);
            SetMotionBlur(Data.MotionBlur);

            SetMasterVolume(Data.MasterVolume);
            SetMusicVolume(Data.MusicVolume);
            SetSoundEffectsVolume(Data.SoundEffectsVolume);
            SetInGameMusic(Data.InGameMusic);

            SetOnlineUsername(Data.OnlineUsername);
            SetExportPrecision(Data.ExportPrecision);
            SetCameraShake(Data.CameraShake);
            SetDebugMode(Data.DebugMode);
            SetPreviewFeatures(Data.PreviewFeatures);
            SetNetworkStats(Data.NetworkStats);
            SetTutorial(Data.Tutorial);
            SetWorldChat(Data.WorldChat);
            SetMap(Data.Map);

            SetSensitivityHorizontal(Data.SensitivityHorizontal);
            SetSensitivityVertical(Data.SensitivityVertical);
            SetInvertHorizontal(Data.InvertHorizontal);
            SetInvertVertical(Data.InvertVertical);
        }
        private void OnDestroy()
        {
            Save();
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
            switch (type)
            {
                case CreatureMeshQualityType.Low:
                    creatureQualitySettings.Segments = 8;
                    creatureQualitySettings.Rings = 2;
                    break;
                case CreatureMeshQualityType.Medium:
                    creatureQualitySettings.Segments = 14;
                    creatureQualitySettings.Rings = 4;
                    break;
                case CreatureMeshQualityType.High:
                    creatureQualitySettings.Segments = 20;
                    creatureQualitySettings.Rings = 8;
                    break;
            }
            Data.CreatureMeshQuality = type;
        }
        public void SetShadowQuality(ShadowQualityType type)
        {
            if (type == ShadowQualityType.None)
            {
                QualitySettings.shadows = ShadowQuality.Disable;
            }
            else
            {
                QualitySettings.shadows = ShadowQuality.All;
                switch (type)
                {
                    case ShadowQualityType.Low:
                        QualitySettings.shadowResolution = ShadowResolution.Low;
                        QualitySettings.shadowDistance = 50;
                        break;
                    case ShadowQualityType.Medium:
                        QualitySettings.shadowResolution = ShadowResolution.Medium;
                        QualitySettings.shadowDistance = 100;
                        break;
                    case ShadowQualityType.High:
                        QualitySettings.shadowResolution = ShadowResolution.High;
                        QualitySettings.shadowDistance = 150;
                        break;
                    case ShadowQualityType.VeryHigh:
                        QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
                        QualitySettings.shadowDistance = 200;
                        break;
                }
            }
            Data.ShadowQuality = type;
        }
        public void SetTextureQuality(TextureQualityType type)
        {
            switch (type)
            {
                case TextureQualityType.VeryLow:
                    QualitySettings.masterTextureLimit = 4;
                    break;
                case TextureQualityType.Low:
                    QualitySettings.masterTextureLimit = 3;
                    break;
                case TextureQualityType.Medium:
                    QualitySettings.masterTextureLimit = 2;
                    break;
                case TextureQualityType.High:
                    QualitySettings.masterTextureLimit = 1;
                    break;
                case TextureQualityType.VeryHigh:
                    QualitySettings.masterTextureLimit = 0;
                    break;
            }
            Data.TextureQuality = type;
        }
        public void SetAmbientOcclusion(AmbientOcclusionType type)
        {
            foreach (PostProcessProfile profile in profiles)
            {
                AmbientOcclusion ao = profile.GetSetting<AmbientOcclusion>();
                if (ao != null)
                {
                    if (type == AmbientOcclusionType.None)
                    {
                        ao.active = false;
                    }
                    else
                    {
                        ao.active = true;
                        switch (type)
                        {
                            case AmbientOcclusionType.SAO:
                                ao.mode = new AmbientOcclusionModeParameter() { value = AmbientOcclusionMode.ScalableAmbientObscurance };
                                break;
                            case AmbientOcclusionType.MSVO:
                                ao.mode = new AmbientOcclusionModeParameter() { value = AmbientOcclusionMode.MultiScaleVolumetricObscurance };
                                break;
                        }
                    }
                }
            }
            Data.AmbientOcclusion = type;
        }
        public void SetAntialiasing(AntialiasingType type, bool updateMain = false)
        {
            List<PostProcessLayer> layers = new List<PostProcessLayer>();
            foreach (GameObject camera in cameras)
            {
                layers.AddRange(camera.GetComponentsInChildren<PostProcessLayer>(true));
            }
            if (updateMain)
            {
                layers.AddRange(Camera.main.GetComponents<PostProcessLayer>());
            }

            foreach (PostProcessLayer layer in layers)
            {
                switch (type)
                {
                    case AntialiasingType.None:
                        layer.antialiasingMode = PostProcessLayer.Antialiasing.None;
                        break;
                    case AntialiasingType.FXAA:
                        layer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                        break;
                    case AntialiasingType.LowSMAA:
                        layer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                        layer.subpixelMorphologicalAntialiasing.quality = SubpixelMorphologicalAntialiasing.Quality.Low;
                        break;
                    case AntialiasingType.MediumSMAA:
                        layer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                        layer.subpixelMorphologicalAntialiasing.quality = SubpixelMorphologicalAntialiasing.Quality.Medium;
                        break;
                    case AntialiasingType.HighSMAA:
                        layer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                        layer.subpixelMorphologicalAntialiasing.quality = SubpixelMorphologicalAntialiasing.Quality.High;
                        break;
                    case AntialiasingType.Temporal:
                        layer.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
                        break;
                }
            }
            Data.Antialiasing = type;
        }
        public void SetScreenSpaceReflections(ScreenSpaceReflectionsType type)
        {
            foreach (PostProcessProfile profile in profiles)
            {
                ScreenSpaceReflections ssr = profile.GetSetting<ScreenSpaceReflections>();
                if (ssr != null)
                {
                    if (type == ScreenSpaceReflectionsType.None)
                    {
                        ssr.active = false;
                    }
                    else
                    {
                        ssr.active = true;
                        switch (type)
                        {
                            case ScreenSpaceReflectionsType.Low:
                                ssr.preset = new ScreenSpaceReflectionPresetParameter() { value = ScreenSpaceReflectionPreset.Low };
                                break;
                            case ScreenSpaceReflectionsType.Medium:
                                ssr.preset = new ScreenSpaceReflectionPresetParameter() { value = ScreenSpaceReflectionPreset.Medium };
                                break;
                            case ScreenSpaceReflectionsType.High:
                                ssr.preset = new ScreenSpaceReflectionPresetParameter() { value = ScreenSpaceReflectionPreset.High };
                                break;
                            case ScreenSpaceReflectionsType.VeryHigh:
                                ssr.preset = new ScreenSpaceReflectionPresetParameter() { value = ScreenSpaceReflectionPreset.Higher };
                                break;
                        }
                    }
                }
            }
            Data.ScreenSpaceReflections = type;
        }
        public void SetFoliage(FoliageType type)
        {
            foreach (GRendering terrainProfile in renderingProfiles)
            {
                switch (type)
                {
                    case FoliageType.VeryLow:
                        terrainProfile.DrawGrasses = false;
                        terrainProfile.TreeDistance = 100;
                        foreach (Material material in windMaterials)
                        {
                            material.shader = Shader.Find("Standard");
                        }
                        break;
                    case FoliageType.Low:
                        terrainProfile.DrawGrasses = true;
                        terrainProfile.GrassDistance = 50;
                        terrainProfile.TreeDistance = 150;
                        foreach (Material material in windMaterials)
                        {
                            material.shader = Shader.Find("Standard");
                        }
                        break;
                    case FoliageType.Medium:
                        terrainProfile.DrawGrasses = true;
                        terrainProfile.GrassDistance = 100;
                        terrainProfile.TreeDistance = 200;
                        foreach (Material material in windMaterials)
                        {
                            material.shader = Shader.Find("Nicrom/LPW/ASE/Low Poly Vegetation");
                        }
                        break;
                    case FoliageType.High:
                        terrainProfile.DrawGrasses = true;
                        terrainProfile.GrassDistance = 150;
                        terrainProfile.TreeDistance = 250;
                        foreach (Material material in windMaterials)
                        {
                            material.shader = Shader.Find("Nicrom/LPW/ASE/Low Poly Vegetation");
                        }
                        break;
                    case FoliageType.VeryHigh:
                        terrainProfile.DrawGrasses = true;
                        terrainProfile.GrassDistance = 200;
                        terrainProfile.TreeDistance = 300;
                        foreach (Material material in windMaterials)
                        {
                            material.shader = Shader.Find("Nicrom/LPW/ASE/Low Poly Vegetation");
                        }
                        break;
                }
            }
            Data.Foliage = type;
        }
        public void SetReflections(bool reflections)
        {
            //foreach (PWaterProfile waterProfile in waterProfiles)
            //{
            //    waterProfile.EnableReflection = reflections;
            //}
            Data.Reflections = reflections;
        }
        public void SetAnisotropicFiltering(bool anisotropicFiltering)
        {
            QualitySettings.anisotropicFiltering = anisotropicFiltering ? AnisotropicFiltering.Enable : AnisotropicFiltering.Disable;
            Data.AnisotropicFiltering = anisotropicFiltering;
        }
        public void SetBloom(bool bloom)
        {
            foreach (PostProcessProfile profile in profiles)
            {
                Bloom b = profile.GetSetting<Bloom>();
                if (b != null)
                {
                    b.active = bloom;
                }
            }
            Data.Bloom = bloom;
        }
        public void SetDepthOfField(bool depthOfField)
        {
            foreach (PostProcessProfile profile in profiles)
            {
                DepthOfField dof = profile.GetSetting<DepthOfField>();
                if (dof != null)
                {
                    dof.active = depthOfField;
                }
            }
            Data.DepthOfField = depthOfField;
        }
        public void SetMotionBlur(bool motionBlur)
        {
            foreach (PostProcessProfile profile in profiles)
            {
                MotionBlur mb = profile.GetSetting<MotionBlur>();
                if (mb != null)
                {
                    mb.active = motionBlur;
                }
            }
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
        public void SetInGameMusic(InGameMusicType type)
        {
            Data.InGameMusic = type;
        }
        #endregion

        #region Gameplay
        public void SetOnlineUsername(string username)
        {
            Data.OnlineUsername = username;
        }
        public void SetExportPrecision(int precision)
        {
            Data.ExportPrecision = precision;
        }
        public void SetCameraShake(bool cameraShake, bool updateMain = false)
        {
            List<StressReceiver> receivers = new List<StressReceiver>();
            foreach (GameObject camera in cameras)
            {
                receivers.AddRange(camera.GetComponents<StressReceiver>());
            }
            if (updateMain)
            {
                receivers.AddRange(Camera.main.GetComponents<StressReceiver>());
            }

            foreach (StressReceiver receiver in receivers)
            {
                receiver.enabled = cameraShake;
                receiver.Reset();
            }
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

            cameraOrbitPrefab.HandleClipping = previewFeatures;
        }
        public void SetNetworkStats(bool networkStats)
        {
            statsManagerPrefab.UseStats = networkStats;
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
        public void SetMap(bool map)
        {
            Data.Map = map;
        }
        public void SetLocale(string locale)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(locale);
            Data.Locale = locale;
        }
        #endregion

        #region Controls
        public void SetSensitivityHorizontal(float sensitivity, bool updateMain = false)
        {
            Vector2 mouseSensitivity = new Vector2(sensitivity, Data.SensitivityVertical);

            cameraOrbitPrefab.MouseSensitivity = mouseSensitivity;
            if (updateMain)
            {
                Camera.main.GetComponentInParent<CameraOrbit>().MouseSensitivity = mouseSensitivity;
            }

            Data.SensitivityHorizontal = sensitivity;
        }
        public void SetSensitivityVertical(float sensitivity, bool updateMain = false)
        {
            Vector2 mouseSensitivity = new Vector2(Data.SensitivityHorizontal, sensitivity);

            cameraOrbitPrefab.MouseSensitivity = mouseSensitivity;
            if (updateMain)
            {
                Camera.main.GetComponentInParent<CameraOrbit>().MouseSensitivity = mouseSensitivity;
            }

            Data.SensitivityVertical = sensitivity;
        }
        public void SetInvertHorizontal(bool invert, bool updateMain = false)
        {
            cameraOrbitPrefab.InvertMouseX = invert;
            if (updateMain)
            {
                Camera.main.GetComponentInParent<CameraOrbit>().InvertMouseX = invert;
            }

            Data.InvertHorizontal = invert;
        }
        public void SetInvertVertical(bool invert, bool updateMain = false)
        {
            cameraOrbitPrefab.InvertMouseY = invert;
            if (updateMain)
            {
                Camera.main.GetComponentInParent<CameraOrbit>().InvertMouseY = invert;
            }

            Data.InvertVertical = invert;
        }
        #endregion
        #endregion
    }
}