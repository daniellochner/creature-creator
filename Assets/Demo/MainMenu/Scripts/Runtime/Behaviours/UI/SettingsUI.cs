// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using IngameDebugConsole;
using Pinwheel.Griffin;
using Pinwheel.Poseidon;
using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using static DanielLochner.Assets.CreatureCreator.Settings;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SettingsUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private bool inGame;

        [Header("Video")]
        [SerializeField] private OptionSelector resolutionOS;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Toggle vSyncToggle;
        [SerializeField] private OptionSelector presetOS;
        [SerializeField] private OptionSelector creatureMeshQualityOS;
        [SerializeField] private OptionSelector shadowQualityOS;
        [SerializeField] private OptionSelector textureQualityOS;
        [SerializeField] private OptionSelector ambientOcclusionOS;
        [SerializeField] private OptionSelector antialiasingOS;
        [SerializeField] private OptionSelector screenSpaceReflectionsOS;
        [SerializeField] private OptionSelector foliageOS;
        [SerializeField] private Toggle reflectionsToggle;
        [SerializeField] private Toggle anisotropicFilteringToggle;
        [SerializeField] private Toggle bloomToggle;
        [SerializeField] private Toggle depthOfFieldToggle;
        [SerializeField] private Toggle motionBlurToggle;
        [SerializeField] private CreatureQuality creatureQualitySettings;
        [SerializeField] private GameObject[] cameras;
        [SerializeField] private PostProcessProfile[] profiles;
        [SerializeField] private PWaterProfile[] waterProfiles;
        [SerializeField] private GRendering[] renderingProfiles;
        [SerializeField] private Material[] windMaterials;

        [Header("Audio")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundEffectsVolumeSlider;
        [SerializeField] private OptionSelector inGameMusicOS;
        [SerializeField] private AudioMixer masterAudioMixer;
        [SerializeField] private MinMax minMaxVolumeDB = new MinMax(-20, 1);

        [Header("Gameplay")]
        [SerializeField] private TMP_InputField onlineUsernameTextField;
        [SerializeField] private TextMeshProUGUI creaturePresetsText;
        [SerializeField] private Button creaturePresetsButton;
        [SerializeField] private Toggle cameraShakeToggle;
        [SerializeField] private Toggle debugModeToggle;
        [SerializeField] private Toggle previewFeaturesToggle;
        [SerializeField] private Toggle networkStatsToggle;
        [SerializeField] private Toggle tutorialToggle;
        [SerializeField] private Toggle worldChatToggle;
        [SerializeField] private Button resetProgressButton;
        [SerializeField] private NetworkStatsManager statsManagerPrefab;

        [Header("Controls")]
        [SerializeField] private Slider sensitivityHorizontalSlider;
        [SerializeField] private Slider sensitivityVerticalSlider;
        [SerializeField] private Toggle invertHorizontalToggle;
        [SerializeField] private Toggle invertVerticalToggle;
        [SerializeField] private CameraOrbit cameraOrbitPrefab;

        private Coroutine previewMusicCoroutine;
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }
        private void OnDestroy()
        {
            SettingsManager.Instance.Save();
        }

        private void Setup()
        {
            #region Video
            SetResolution(SettingsManager.Data.Resolution);
            SetFullscreen(SettingsManager.Data.Fullscreen);
            SetVSync(SettingsManager.Data.VSync);
            SetCreatureMeshQuality(SettingsManager.Data.CreatureMeshQuality);
            SetShadowQuality(SettingsManager.Data.ShadowQuality);
            SetTextureQuality(SettingsManager.Data.TextureQuality);
            SetAmbientOcclusion(SettingsManager.Data.AmbientOcclusion);
            SetAntialiasing(SettingsManager.Data.Antialiasing);
            SetScreenSpaceReflections(SettingsManager.Data.ScreenSpaceReflections);
            SetFoliage(SettingsManager.Data.Foliage);
            SetReflections(SettingsManager.Data.Reflections);
            SetAnisotropicFiltering(SettingsManager.Data.AnisotropicFiltering);
            SetBloom(SettingsManager.Data.Bloom);
            SetDebugMode(SettingsManager.Data.DebugMode);
            SetDepthOfField(SettingsManager.Data.DepthOfField);
            SetMotionBlur(SettingsManager.Data.MotionBlur);

            // Resolution
            for (int i = 0; i < Screen.resolutions.Length; ++i)
            {
                Resolution resolution = Screen.resolutions[i];
                resolutionOS.Options.Add(new OptionSelector.Option()
                {
                    Name = $"{resolution.width}x{resolution.height} @ {resolution.refreshRate}Hz"
                });
                if (resolution.Equals(Screen.currentResolution))
                {
                    resolutionOS.Select(i, false);
                }
            }

            // Fullscreen
            fullscreenToggle.SetIsOnWithoutNotify(SettingsManager.Data.Fullscreen);
            fullscreenToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetFullscreen(isOn);
            });

            // V-Sync
            vSyncToggle.SetIsOnWithoutNotify(SettingsManager.Data.VSync);
            vSyncToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetVSync(isOn);
            });

            // Preset
            presetOS.SetupUsingEnum<PresetType>();
            presetOS.Select(PresetType.Custom, false);
            presetOS.OnSelected.AddListener(delegate (int option)
            {
                PresetType presetType = (PresetType)option;
                switch (presetType)
                {
                    case PresetType.VeryLow:
                        creatureMeshQualityOS.Select(CreatureMeshQualityType.Low);
                        shadowQualityOS.Select(ShadowQualityType.Low);
                        textureQualityOS.Select(TextureQualityType.Low);
                        ambientOcclusionOS.Select(AmbientOcclusionType.None);
                        antialiasingOS.Select(AntialiasingType.None);
                        screenSpaceReflectionsOS.Select(ScreenSpaceReflectionsType.None);
                        foliageOS.Select(FoliageType.VeryLow);
                        reflectionsToggle.isOn = false;
                        anisotropicFilteringToggle.isOn = false;
                        bloomToggle.isOn = false;
                        break;

                    case PresetType.Low:
                        creatureMeshQualityOS.Select(CreatureMeshQualityType.Low);
                        shadowQualityOS.Select(ShadowQualityType.Low);
                        textureQualityOS.Select(TextureQualityType.Low);
                        ambientOcclusionOS.Select(AmbientOcclusionType.None);
                        antialiasingOS.Select(AntialiasingType.FXAA);
                        screenSpaceReflectionsOS.Select(ScreenSpaceReflectionsType.Low);
                        foliageOS.Select(FoliageType.Low);
                        reflectionsToggle.isOn = false;
                        anisotropicFilteringToggle.isOn = false;
                        bloomToggle.isOn = false;
                        break;

                    case PresetType.Medium:
                        creatureMeshQualityOS.Select(CreatureMeshQualityType.Medium);
                        shadowQualityOS.Select(ShadowQualityType.Medium);
                        textureQualityOS.Select(TextureQualityType.Medium);
                        ambientOcclusionOS.Select(AmbientOcclusionType.SAO);
                        antialiasingOS.Select(AntialiasingType.MediumSMAA);
                        screenSpaceReflectionsOS.Select(ScreenSpaceReflectionsType.Medium);
                        foliageOS.Select(FoliageType.Medium);
                        reflectionsToggle.isOn = true;
                        anisotropicFilteringToggle.isOn = true;
                        bloomToggle.isOn = true;
                        break;

                    case PresetType.High:
                        creatureMeshQualityOS.Select(CreatureMeshQualityType.High);
                        shadowQualityOS.Select(ShadowQualityType.High);
                        textureQualityOS.Select(TextureQualityType.High);
                        ambientOcclusionOS.Select(AmbientOcclusionType.MSVO);
                        antialiasingOS.Select(AntialiasingType.HighSMAA);
                        screenSpaceReflectionsOS.Select(ScreenSpaceReflectionsType.High);
                        foliageOS.Select(FoliageType.High);
                        reflectionsToggle.isOn = true;
                        anisotropicFilteringToggle.isOn = true;
                        bloomToggle.isOn = true;
                        break;

                    case PresetType.VeryHigh:
                        creatureMeshQualityOS.Select(CreatureMeshQualityType.High);
                        shadowQualityOS.Select(ShadowQualityType.High);
                        textureQualityOS.Select(TextureQualityType.High);
                        ambientOcclusionOS.Select(AmbientOcclusionType.MSVO);
                        antialiasingOS.Select(AntialiasingType.Temporal);
                        screenSpaceReflectionsOS.Select(ScreenSpaceReflectionsType.VeryHigh);
                        foliageOS.Select(FoliageType.High);
                        reflectionsToggle.isOn = true;
                        anisotropicFilteringToggle.isOn = true;
                        bloomToggle.isOn = true;
                        break;
                }
            });

            // Creature Mesh Quality
            creatureMeshQualityOS.SetupUsingEnum<CreatureMeshQualityType>();
            creatureMeshQualityOS.Select(SettingsManager.Data.CreatureMeshQuality, false);
            creatureMeshQualityOS.OnSelected.AddListener(delegate (int option)
            {
                SetCreatureMeshQuality((CreatureMeshQualityType)option);
            });

            // Shadow Quality
            shadowQualityOS.SetupUsingEnum<ShadowQualityType>();
            shadowQualityOS.Select(SettingsManager.Data.ShadowQuality, false);
            shadowQualityOS.OnSelected.AddListener(delegate (int option)
            {
                SetShadowQuality((ShadowQualityType)option);
            });

            // Texture Quality
            textureQualityOS.SetupUsingEnum<TextureQualityType>();
            textureQualityOS.Select(SettingsManager.Data.TextureQuality, false);
            textureQualityOS.OnSelected.AddListener(delegate (int option)
            {
                SetTextureQuality((TextureQualityType)option);
            });

            // Ambient Occlusion
            ambientOcclusionOS.SetupUsingEnum<AmbientOcclusionType>();
            ambientOcclusionOS.Select(SettingsManager.Data.AmbientOcclusion, false);
            ambientOcclusionOS.OnSelected.AddListener(delegate (int option)
            {
                SetAmbientOcclusion((AmbientOcclusionType)option);
            });

            // Antialiasing
            antialiasingOS.SetupUsingEnum<AntialiasingType>();
            antialiasingOS.Select(SettingsManager.Data.Antialiasing, false);
            antialiasingOS.OnSelected.AddListener(delegate (int option)
            {
                SetAntialiasing((AntialiasingType)option, true);
            });

            // Screen Space Reflections
            screenSpaceReflectionsOS.SetupUsingEnum<ScreenSpaceReflectionsType>();
            screenSpaceReflectionsOS.Select(SettingsManager.Data.ScreenSpaceReflections, false);
            screenSpaceReflectionsOS.OnSelected.AddListener(delegate (int option)
            {
                SetScreenSpaceReflections((ScreenSpaceReflectionsType)option);
            });

            // Foliage
            foliageOS.SetupUsingEnum<FoliageType>();
            foliageOS.Select(SettingsManager.Data.Foliage, false);
            foliageOS.OnSelected.AddListener(delegate (int option)
            {
                SetFoliage((FoliageType)option);
            });

            // Reflections
            reflectionsToggle.SetIsOnWithoutNotify(SettingsManager.Data.Reflections);
            reflectionsToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetReflections(isOn);
            });

            // Anisotropic Filtering
            anisotropicFilteringToggle.SetIsOnWithoutNotify(SettingsManager.Data.AnisotropicFiltering);
            anisotropicFilteringToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetAnisotropicFiltering(isOn);
            });

            // Bloom
            bloomToggle.SetIsOnWithoutNotify(SettingsManager.Data.Bloom);
            bloomToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetBloom(isOn);
            });

            // Depth Of Field
            depthOfFieldToggle.SetIsOnWithoutNotify(SettingsManager.Data.DepthOfField);
            depthOfFieldToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetDepthOfField(isOn);
            });

            // Motion Blur
            motionBlurToggle.SetIsOnWithoutNotify(SettingsManager.Data.MotionBlur);
            motionBlurToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetMotionBlur(isOn);
            });
            #endregion

            #region Audio
            SetMasterVolume(SettingsManager.Data.MasterVolume);
            SetMusicVolume(SettingsManager.Data.MusicVolume);
            SetSoundEffectsVolume(SettingsManager.Data.SoundEffectsVolume);
            SetInGameMusic(SettingsManager.Data.InGameMusic);

            // Master Volume
            masterVolumeSlider.value = SettingsManager.Data.MasterVolume * 100;
            masterVolumeSlider.onValueChanged.AddListener(delegate (float value)
            {
                SetMasterVolume(value / 100f);
            });

            // Music Volume
            musicVolumeSlider.value = SettingsManager.Data.MusicVolume * 100;
            musicVolumeSlider.onValueChanged.AddListener(delegate (float value)
            {
                SetMusicVolume(value / 100f);
            });

            // Sound Effects Volume
            soundEffectsVolumeSlider.value = SettingsManager.Data.SoundEffectsVolume * 100;
            soundEffectsVolumeSlider.onValueChanged.AddListener(delegate (float value)
            {
                SetSoundEffectsVolume(value / 100f);
            });

            // Background Music
            inGameMusicOS.SetupUsingEnum<InGameMusicType>();
            inGameMusicOS.Select(SettingsManager.Data.InGameMusic, false);
            inGameMusicOS.OnSelected.AddListener(delegate (int option)
            {
                InGameMusicType type = (InGameMusicType)option;
                SetInGameMusic(type);

                string music = type.ToString();
                if (type == InGameMusicType.None)
                {
                    music = null;
                }

                if (inGame)
                {
                    MusicManager.Instance.FadeTo(music);
                }
                else
                {
                    if (previewMusicCoroutine != null)
                    {
                        StopCoroutine(previewMusicCoroutine);
                    }
                    previewMusicCoroutine = StartCoroutine(PreviewMusicRoutine(music));
                }
            });
            #endregion

            #region Gameplay
            SetOnlineUsername(SettingsManager.Data.OnlineUsername);
            SetCameraShake(SettingsManager.Data.CameraShake);
            SetDebugMode(SettingsManager.Data.DebugMode);
            SetPreviewFeatures(SettingsManager.Data.PreviewFeatures);
            SetNetworkStats(SettingsManager.Data.NetworkStats);
            SetTutorial(SettingsManager.Data.Tutorial);
            SetWorldChat(SettingsManager.Data.WorldChat);

            // Online Username
            onlineUsernameTextField.text = SettingsManager.Data.OnlineUsername;
            onlineUsernameTextField.onValueChanged.AddListener(delegate (string text) 
            {
                SetOnlineUsername(text);
            });
            
            // Creature Preset(s)
            int presets = SettingsManager.Data.CreaturePresets.Count;
            creaturePresetsText.text = (presets == 0) ? "None" : $"{presets} presets";
            creaturePresetsButton.onClick.AddListener(delegate
            {
                FileBrowser.ShowLoadDialog(
                    onSuccess: delegate (string[] paths)
                    {
                        SettingsManager.Data.CreaturePresets.Clear();
                        foreach (string path in paths)
                        {
                            CreatureData creature = SaveUtility.Load<CreatureData>(path);
                            if (creature != null)
                            {
                                SettingsManager.Data.CreaturePresets.Add(creature);
                            }
                        }
                    },
                    onCancel: null,
                    pickMode: FileBrowser.PickMode.Files,
                    allowMultiSelection: true,
                    initialPath: Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    title: "Select Creature Preset(s)",
                    loadButtonText: "Select"
                );
            });

            // Camera Shake
            cameraShakeToggle.SetIsOnWithoutNotify(SettingsManager.Data.CameraShake);
            cameraShakeToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetCameraShake(isOn, true);
            });

            // Debug Mode
            debugModeToggle.SetIsOnWithoutNotify(SettingsManager.Data.DebugMode);
            debugModeToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetDebugMode(isOn);
            });

            // Preview Features
            previewFeaturesToggle.SetIsOnWithoutNotify(SettingsManager.Data.PreviewFeatures);
            previewFeaturesToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetPreviewFeatures(isOn);
            });

            // Network Stats
            networkStatsToggle.SetIsOnWithoutNotify(SettingsManager.Data.NetworkStats);
            networkStatsToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetNetworkStats(isOn);
            });

            // Tutorial
            tutorialToggle.SetIsOnWithoutNotify(SettingsManager.Data.NetworkStats);
            tutorialToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetTutorial(isOn);
            });

            // World Chat
            worldChatToggle.SetIsOnWithoutNotify(SettingsManager.Data.WorldChat);
            worldChatToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetWorldChat(isOn);
            });

            // Reset Progress
            resetProgressButton.onClick.AddListener(delegate
            {
                ConfirmationDialog.Confirm("Reset Progress?", "This will reset all your unlocked body parts and patterns, as well as your cash, level and experience. It will <u>not</u> remove your creatures.", onYes: ResetProgress);
            });
            #endregion

            #region Controls
            SetSensitivityHorizontal(SettingsManager.Data.SensitivityHorizontal);
            SetSensitivityVertical(SettingsManager.Data.SensitivityVertical);
            SetInvertHorizontal(SettingsManager.Data.InvertHorizontal);
            SetInvertVertical(SettingsManager.Data.InvertVertical);

            // Sensitivity (Horizontal)
            sensitivityHorizontalSlider.value = SettingsManager.Data.SensitivityHorizontal;
            sensitivityHorizontalSlider.onValueChanged.AddListener(delegate (float value)
            {
                SetSensitivityHorizontal(value, inGame);
            });

            // Sensitivity (Vertical)
            sensitivityVerticalSlider.value = SettingsManager.Data.SensitivityVertical;
            sensitivityVerticalSlider.onValueChanged.AddListener(delegate (float value)
            {
                SetSensitivityVertical(value, inGame);
            });

            // Invert Horizontal
            invertHorizontalToggle.SetIsOnWithoutNotify(SettingsManager.Data.InvertHorizontal);
            invertHorizontalToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetInvertHorizontal(isOn, inGame);
            });

            // Invert Vertical
            invertVerticalToggle.SetIsOnWithoutNotify(SettingsManager.Data.InvertVertical);
            invertVerticalToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SetInvertVertical(isOn, inGame);
            });
            #endregion
        }
        
        #region Video
        public void SetResolution(Resolution resolution)
        {
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
            SettingsManager.Data.Resolution = resolution;
        }
        public void SetFullscreen(bool fullscreen)
        {
            Screen.fullScreen = SettingsManager.Data.Fullscreen = fullscreen;
        }
        public void SetVSync(bool vSync)
        {
            QualitySettings.vSyncCount = vSync ? 1 : 0;
            SettingsManager.Data.VSync = vSync;
        }
        public void ApplyResolution()
        {
            SetResolution(Screen.resolutions[resolutionOS.Selected]);
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
            SettingsManager.Data.CreatureMeshQuality = type;
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
            SettingsManager.Data.ShadowQuality = type;
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
            SettingsManager.Data.TextureQuality = type;
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
            SettingsManager.Data.AmbientOcclusion = type;
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
            SettingsManager.Data.Antialiasing = type;
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
            SettingsManager.Data.ScreenSpaceReflections = type;
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
            SettingsManager.Data.Foliage = type;
        }
        public void SetReflections(bool reflections)
        {
            foreach (PWaterProfile waterProfile in waterProfiles)
            {
                waterProfile.EnableReflection = reflections;
            }
            SettingsManager.Data.Reflections = reflections;
        }
        public void SetAnisotropicFiltering(bool anisotropicFiltering)
        {
            QualitySettings.anisotropicFiltering = anisotropicFiltering ? AnisotropicFiltering.Enable : AnisotropicFiltering.Disable;
            SettingsManager.Data.AnisotropicFiltering = anisotropicFiltering;
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
            SettingsManager.Data.Bloom = bloom;
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
            SettingsManager.Data.DepthOfField = depthOfField;
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
            SettingsManager.Data.MotionBlur = motionBlur;
        }
        #endregion

        #region Audio
        public void SetMasterVolume(float t)
        {
            SettingsManager.Data.MasterVolume = t;
            SetMusicVolume(SettingsManager.Data.MusicVolume);
            SetSoundEffectsVolume(SettingsManager.Data.SoundEffectsVolume);
        }
        public void SetMusicVolume(float t)
        {
            SettingsManager.Data.MusicVolume = t;
            SetVolume("MusicVolume", t);
        }
        public void SetSoundEffectsVolume(float t)
        {
            SettingsManager.Data.SoundEffectsVolume = t;
            SetVolume("SoundEffectsVolume", t);
        }
        private void SetVolume(string param, float t)
        {
            t *= SettingsManager.Data.MasterVolume;

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
            SettingsManager.Data.InGameMusic = type;
        }
        private IEnumerator PreviewMusicRoutine(string music)
        {
            MusicManager.Instance.FadeTo(music);
            yield return new WaitForSeconds(5f);
            MusicManager.Instance.FadeTo(null);
        }
        #endregion

        #region Gameplay
        public void SetOnlineUsername(string username)
        {
            SettingsManager.Data.OnlineUsername = username;
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
            SettingsManager.Data.CameraShake = cameraShake;
        }
        public void SetDebugMode(bool debugMode)
        {
            DebugLogManager.Instance.gameObject.SetActive(debugMode);
            SettingsManager.Data.DebugMode = debugMode;
        }
        public void SetPreviewFeatures(bool previewFeatures)
        {
            SettingsManager.Data.PreviewFeatures = previewFeatures;
        }
        public void SetNetworkStats(bool networkStats)
        {
            statsManagerPrefab.UseStats = networkStats;
            SettingsManager.Data.NetworkStats = networkStats;
        }
        public void SetTutorial(bool tutorial)
        {
            SettingsManager.Data.Tutorial = tutorial;
        }
        public void SetWorldChat(bool worldChat)
        {
            SettingsManager.Data.WorldChat = worldChat;
        }
        public void ResetProgress()
        {
            ProgressManager.Instance.Revert();

            ProgressUI.Instance.UpdateInfo();
            UnlockableBodyPartsMenu.Instance.UpdateInfo();
            UnlockablePatternsMenu.Instance.UpdateInfo();
        }
        #endregion

        #region Controls
        public void SetSensitivityHorizontal(float sensitivity, bool updateMain = false)
        {
            Vector2 mouseSensitivity = new Vector2(sensitivity, SettingsManager.Data.SensitivityVertical);

            cameraOrbitPrefab.MouseSensitivity = mouseSensitivity;
            if (updateMain)
            {
                Camera.main.GetComponentInParent<CameraOrbit>().MouseSensitivity = mouseSensitivity;
            }

            SettingsManager.Data.SensitivityHorizontal = sensitivity;
        }
        public void SetSensitivityVertical(float sensitivity, bool updateMain = false)
        {
            Vector2 mouseSensitivity = new Vector2(SettingsManager.Data.SensitivityHorizontal, sensitivity);

            cameraOrbitPrefab.MouseSensitivity = mouseSensitivity;
            if (updateMain)
            {
                Camera.main.GetComponentInParent<CameraOrbit>().MouseSensitivity = mouseSensitivity;
            }

            SettingsManager.Data.SensitivityVertical = sensitivity;
        }
        public void SetInvertHorizontal(bool invert, bool updateMain = false)
        {
            cameraOrbitPrefab.InvertMouseX = invert;
            if (updateMain)
            {
                Camera.main.GetComponentInParent<CameraOrbit>().InvertMouseX = invert;
            }

            SettingsManager.Data.InvertHorizontal = invert;
        }
        public void SetInvertVertical(bool invert, bool updateMain = false)
        {
            cameraOrbitPrefab.InvertMouseY = invert;
            if (updateMain)
            {
                Camera.main.GetComponentInParent<CameraOrbit>().InvertMouseY = invert;
            }

            SettingsManager.Data.InvertVertical = invert;
        }
        #endregion
        #endregion
    }
}