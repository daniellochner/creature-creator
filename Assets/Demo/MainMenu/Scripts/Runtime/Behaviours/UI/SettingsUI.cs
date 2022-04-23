// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using SimpleFileBrowser;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DanielLochner.Assets.CreatureCreator.Settings;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SettingsUI : MonoBehaviour
    {
        #region Fields
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
        [SerializeField] private Toggle anisotropicFilteringToggle;
        [SerializeField] private Toggle bloomToggle;
        [SerializeField] private Toggle depthOfFieldToggle;
        [SerializeField] private Toggle motionBlurToggle;
        
        [Header("Audio")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundEffectsVolumeSlider;
        
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
        [SerializeField] private ProgressUI progressUI;

        [Header("Controls")]
        [SerializeField] private Slider sensitivityHorizontalSlider;
        [SerializeField] private Slider sensitivityVerticalSlider;
        [SerializeField] private Toggle invertHorizontalToggle;
        [SerializeField] private Toggle invertVerticalToggle;
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }
        private void Setup()
        {
            #region Video
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
            resolutionOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetResolution(Screen.resolutions[option]);
            });

            // Fullscreen
            fullscreenToggle.SetIsOnWithoutNotify(SettingsManager.Data.Fullscreen);
            fullscreenToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetFullscreen(isOn);
            });

            // V-Sync
            vSyncToggle.SetIsOnWithoutNotify(SettingsManager.Data.VSync);
            vSyncToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetVSync(isOn);
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
                        screenSpaceReflectionsOS.Select(ScreenSpaceReflectionsType.Low);
                        anisotropicFilteringToggle.isOn = false;
                        bloomToggle.isOn = false;
                        depthOfFieldToggle.isOn = false;
                        motionBlurToggle.isOn = false;
                        break;

                    case PresetType.Low:
                        creatureMeshQualityOS.Select(CreatureMeshQualityType.Low);
                        shadowQualityOS.Select(ShadowQualityType.Low);
                        textureQualityOS.Select(TextureQualityType.Low);
                        ambientOcclusionOS.Select(AmbientOcclusionType.None);
                        antialiasingOS.Select(AntialiasingType.None);
                        screenSpaceReflectionsOS.Select(ScreenSpaceReflectionsType.Low);
                        anisotropicFilteringToggle.isOn = false;
                        bloomToggle.isOn = false;
                        depthOfFieldToggle.isOn = false;
                        motionBlurToggle.isOn = false;
                        break;

                    case PresetType.Medium:
                        creatureMeshQualityOS.Select(CreatureMeshQualityType.Medium);
                        shadowQualityOS.Select(ShadowQualityType.Medium);
                        textureQualityOS.Select(TextureQualityType.Medium);
                        ambientOcclusionOS.Select(AmbientOcclusionType.SAO);
                        antialiasingOS.Select(AntialiasingType.FXAA);
                        screenSpaceReflectionsOS.Select(ScreenSpaceReflectionsType.Medium);
                        anisotropicFilteringToggle.isOn = true;
                        bloomToggle.isOn = true;
                        depthOfFieldToggle.isOn = false;
                        motionBlurToggle.isOn = false;
                        break;

                    case PresetType.High:
                        creatureMeshQualityOS.Select(CreatureMeshQualityType.High);
                        shadowQualityOS.Select(ShadowQualityType.High);
                        textureQualityOS.Select(TextureQualityType.High);
                        ambientOcclusionOS.Select(AmbientOcclusionType.MSVO);
                        antialiasingOS.Select(AntialiasingType.HighSMAA);
                        screenSpaceReflectionsOS.Select(ScreenSpaceReflectionsType.High);
                        anisotropicFilteringToggle.isOn = true;
                        bloomToggle.isOn = true;
                        depthOfFieldToggle.isOn = true;
                        motionBlurToggle.isOn = true;
                        break;

                    case PresetType.VeryHigh:
                        creatureMeshQualityOS.Select(CreatureMeshQualityType.High);
                        shadowQualityOS.Select(ShadowQualityType.High);
                        textureQualityOS.Select(TextureQualityType.High);
                        ambientOcclusionOS.Select(AmbientOcclusionType.MSVO);
                        antialiasingOS.Select(AntialiasingType.HighSMAA);
                        screenSpaceReflectionsOS.Select(ScreenSpaceReflectionsType.High);
                        anisotropicFilteringToggle.isOn = true;
                        bloomToggle.isOn = true;
                        depthOfFieldToggle.isOn = true;
                        motionBlurToggle.isOn = true;
                        break;
                }
            });

            // Creature Mesh Quality
            creatureMeshQualityOS.SetupUsingEnum<CreatureMeshQualityType>();
            creatureMeshQualityOS.Select(SettingsManager.Data.CreatureMeshQuality, false);
            creatureMeshQualityOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetCreatureMeshQuality((CreatureMeshQualityType)option);
            });

            // Shadow Quality
            shadowQualityOS.SetupUsingEnum<ShadowQualityType>();
            shadowQualityOS.Select(SettingsManager.Data.ShadowQuality, false);
            shadowQualityOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetShadowQuality((ShadowQualityType)option);
            });

            // Texture Quality
            textureQualityOS.SetupUsingEnum<TextureQualityType>();
            textureQualityOS.Select(SettingsManager.Data.TextureQuality, false);
            textureQualityOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetTextureQuality((TextureQualityType)option);
            });

            // Ambient Occlusion
            ambientOcclusionOS.SetupUsingEnum<AmbientOcclusionType>();
            ambientOcclusionOS.Select(SettingsManager.Data.AmbientOcclusion, false);
            ambientOcclusionOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetAmbientOcclusion((AmbientOcclusionType)option);
            });

            // Antialiasing
            antialiasingOS.SetupUsingEnum<AntialiasingType>();
            antialiasingOS.Select(SettingsManager.Data.Antialiasing, false);
            antialiasingOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetAntialiasing((AntialiasingType)option);
            });

            // Screen Space Reflections
            screenSpaceReflectionsOS.SetupUsingEnum<ScreenSpaceReflectionsType>();
            screenSpaceReflectionsOS.Select(SettingsManager.Data.ScreenSpaceReflections, false);
            screenSpaceReflectionsOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetScreenSpaceReflections((ScreenSpaceReflectionsType)option);
            });

            // Anisotropic Filtering
            anisotropicFilteringToggle.SetIsOnWithoutNotify(SettingsManager.Data.AnisotropicFiltering);
            anisotropicFilteringToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetAnisotropicFiltering(isOn);
            });

            // Bloom
            bloomToggle.SetIsOnWithoutNotify(SettingsManager.Data.Bloom);
            bloomToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetBloom(isOn);
            });

            // Depth Of Field
            depthOfFieldToggle.SetIsOnWithoutNotify(SettingsManager.Data.DepthOfField);
            depthOfFieldToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetDepthOfField(isOn);
            });

            // Motion Blur
            motionBlurToggle.SetIsOnWithoutNotify(SettingsManager.Data.MotionBlur);
            motionBlurToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetMotionBlur(isOn);
            });
            #endregion

            #region Audio
            // Master Volume
            masterVolumeSlider.value = SettingsManager.Data.MasterVolume * 100;
            masterVolumeSlider.onValueChanged.AddListener(delegate (float value)
            {
                SettingsManager.Instance.SetMasterVolume(value / 100f);
            });

            // Music Volume
            musicVolumeSlider.value = SettingsManager.Data.MusicVolume * 100;
            musicVolumeSlider.onValueChanged.AddListener(delegate (float value)
            {
                SettingsManager.Instance.SetMusicVolume(value / 100f);
            });

            // Sound Effects Volume
            soundEffectsVolumeSlider.value = SettingsManager.Data.SoundEffectsVolume * 100;
            soundEffectsVolumeSlider.onValueChanged.AddListener(delegate (float value)
            {
                SettingsManager.Instance.SetSoundEffectsVolume(value / 100f);
            });
            #endregion

            #region Gameplay
            // Online Username
            onlineUsernameTextField.text = SettingsManager.Data.OnlineUsername;
            onlineUsernameTextField.onValueChanged.AddListener(delegate (string text) 
            {
                SettingsManager.Instance.SetOnlineUsername(text);
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
                SettingsManager.Instance.SetCameraShake(isOn);
            });

            // Debug Mode
            debugModeToggle.SetIsOnWithoutNotify(SettingsManager.Data.DebugMode);
            debugModeToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetDebugMode(isOn);
            });

            // Preview Features
            previewFeaturesToggle.SetIsOnWithoutNotify(SettingsManager.Data.PreviewFeatures);
            previewFeaturesToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetPreviewFeatures(isOn);
            });

            // Network Stats
            networkStatsToggle.SetIsOnWithoutNotify(SettingsManager.Data.NetworkStats);
            networkStatsToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetNetworkStats(isOn);
            });

            // Tutorial
            tutorialToggle.SetIsOnWithoutNotify(SettingsManager.Data.NetworkStats);
            tutorialToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetTutorial(isOn);
            });

            // World Chat
            worldChatToggle.SetIsOnWithoutNotify(SettingsManager.Data.WorldChat);
            worldChatToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetWorldChat(isOn);
            });

            // Reset Progress
            resetProgressButton.onClick.AddListener(delegate
            {
                ConfirmationDialog.Confirm("Reset Progress?", "This will reset all your unlocked body parts and patterns, as well as your cash, level and experience. It will <u>not</u> remove your creatures.", yesEvent: ResetProgress);
            });
            #endregion

            #region Controls
            // Sensitivity (Horizontal)
            sensitivityHorizontalSlider.SetValueWithoutNotify(SettingsManager.Data.SensitivityHorizontal);
            sensitivityHorizontalSlider.onValueChanged.AddListener(delegate (float value)
            {
                SettingsManager.Instance.SetSensitivityHorizontal(value);
            });

            // Sensitivity (Vertical)
            sensitivityVerticalSlider.SetValueWithoutNotify(SettingsManager.Data.SensitivityVertical);
            sensitivityVerticalSlider.onValueChanged.AddListener(delegate (float value)
            {
                SettingsManager.Instance.SetSensitivityVertical(value);
            });

            // Invert Horizontal
            invertHorizontalToggle.SetIsOnWithoutNotify(SettingsManager.Data.InvertHorizontal);
            invertHorizontalToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetInvertHorizontal(isOn);
            });

            // Invert Vertical
            invertVerticalToggle.SetIsOnWithoutNotify(SettingsManager.Data.InvertVertical);
            invertVerticalToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetInvertVertical(isOn);
            });
            #endregion
        }

        public void ResetProgress()
        {
            ProgressManager.Instance.Revert();
            progressUI.UpdateInfo();
        }
        #endregion
    }
}