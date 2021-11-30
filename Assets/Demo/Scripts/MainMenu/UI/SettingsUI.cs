// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using IngameDebugConsole;
using SimpleFileBrowser;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static DanielLochner.Assets.CreatureCreator.DemoSettings;

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
        [SerializeField] private AudioMixer masterAudioMixer;
        [SerializeField] private MinMax minMaxVolumeDB;
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundEffectsVolumeSlider;
        
        [Header("Gameplay")]
        [SerializeField] private TMP_InputField onlineUsernameTextField;
        [SerializeField] private TextMeshProUGUI creaturePresetsText;
        [SerializeField] private Button creaturePresetsButton;
        [SerializeField] private Toggle debugModeToggle;
        [SerializeField] private Toggle previewFeaturesToggle;
        [SerializeField] private Toggle networkStatsToggle;
        [SerializeField] private Toggle tutorialToggle;
        [SerializeField] private Toggle worldChatToggle;
        [SerializeField] private Button resetProgressButton;

        [Header("Controls")]
        [SerializeField] private Slider cameraSensitivityHorizontalSlider;
        [SerializeField] private Slider cameraSensitivityVerticalSlider;
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
            foreach (Resolution resolution in Screen.resolutions)
            {
                resolutionOS.Options.Add(new OptionSelector.Option()
                {
                    Name = $"{resolution.width}x{resolution.height} @ {resolution.refreshRate}"
                });
            }
            resolutionOS.Select(DemoManager.Settings.Resolution, false);
            resolutionOS.OnSelected.AddListener(delegate (int option)
            {
                Screen.SetResolution(Screen.resolutions[option].width, Screen.resolutions[option].height, Screen.fullScreen, Screen.resolutions[option].refreshRate);
                DemoManager.Settings.Resolution = option;
            });

            // Fullscreen
            fullscreenToggle.SetIsOnWithoutNotify(DemoManager.Settings.Fullscreen);
            fullscreenToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                Screen.fullScreen = isOn;
                DemoManager.Settings.Fullscreen = isOn;
            });

            // V-Sync
            vSyncToggle.SetIsOnWithoutNotify(DemoManager.Settings.VSync);
            vSyncToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.VSync = isOn;
            });

            // Preset
            presetOS.SetupUsingEnum<PresetType>();
            presetOS.Select((int)PresetType.Custom, false);
            presetOS.OnSelected.AddListener(delegate (int option)
            {
            });

            // Creature Mesh Quality
            creatureMeshQualityOS.SetupUsingEnum<CreatureMeshQualityType>();
            creatureMeshQualityOS.Select((int)DemoManager.Settings.CreatureMeshQuality, false);
            creatureMeshQualityOS.OnSelected.AddListener(delegate (int option)
            {
                DemoManager.Settings.CreatureMeshQuality = (CreatureMeshQualityType)option;
            });

            // Shadow Quality
            shadowQualityOS.SetupUsingEnum<ShadowQualityType>();
            shadowQualityOS.Select((int)DemoManager.Settings.ShadowQuality, false);
            shadowQualityOS.OnSelected.AddListener(delegate (int option)
            {
                DemoManager.Settings.ShadowQuality = (ShadowQualityType)option;
            });

            // Texture Quality
            textureQualityOS.SetupUsingEnum<TextureQualityType>();
            textureQualityOS.Select((int)DemoManager.Settings.TextureQuality, false);
            textureQualityOS.OnSelected.AddListener(delegate (int option)
            {
                DemoManager.Settings.TextureQuality = (TextureQualityType)option;
            });

            // Ambient Occlusion
            ambientOcclusionOS.SetupUsingEnum<AmbientOcclusionType>();
            ambientOcclusionOS.Select((int)DemoManager.Settings.AmbientOcclusion, false);
            ambientOcclusionOS.OnSelected.AddListener(delegate (int option)
            {
                DemoManager.Settings.AmbientOcclusion = (AmbientOcclusionType)option;
            });

            // Antialiasing
            antialiasingOS.SetupUsingEnum<AntialiasingType>();
            antialiasingOS.Select((int)DemoManager.Settings.Antialiasing, false);
            antialiasingOS.OnSelected.AddListener(delegate (int option)
            {
                DemoManager.Settings.Antialiasing = (AntialiasingType)option;
            });

            // Screen Space Reflections
            screenSpaceReflectionsOS.SetupUsingEnum<ScreenSpaceReflectionsType>();
            screenSpaceReflectionsOS.Select((int)DemoManager.Settings.ScreenSpaceReflections, false);
            screenSpaceReflectionsOS.OnSelected.AddListener(delegate (int option)
            {
                DemoManager.Settings.ScreenSpaceReflections = (ScreenSpaceReflectionsType)option;
            });

            // Anisotropic Filtering
            anisotropicFilteringToggle.SetIsOnWithoutNotify(DemoManager.Settings.AnisotropicFiltering);
            anisotropicFilteringToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.AnisotropicFiltering = isOn;
            });

            // Bloom
            bloomToggle.SetIsOnWithoutNotify(DemoManager.Settings.Bloom);
            bloomToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.Bloom = isOn;
            });

            // Depth Of Field
            depthOfFieldToggle.SetIsOnWithoutNotify(DemoManager.Settings.DepthOfField);
            depthOfFieldToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.DepthOfField = isOn;
            });

            // Motion Blur
            motionBlurToggle.SetIsOnWithoutNotify(DemoManager.Settings.MotionBlur);
            motionBlurToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.MotionBlur = isOn;
            });
            #endregion

            #region Audio
            // Master Volume
            masterVolumeSlider.SetValueWithoutNotify(DemoManager.Settings.MasterVolume);
            masterVolumeSlider.onValueChanged.AddListener(delegate (float value)
            {
                masterAudioMixer.SetFloat("MasterVolume", Mathf.Lerp(minMaxVolumeDB.min, minMaxVolumeDB.max, value));
                DemoManager.Settings.MasterVolume = value;
            });

            // Music Volume
            musicVolumeSlider.SetValueWithoutNotify(DemoManager.Settings.MusicVolume);
            musicVolumeSlider.onValueChanged.AddListener(delegate (float value)
            {
                masterAudioMixer.SetFloat("MusicVolume", Mathf.Lerp(minMaxVolumeDB.min, minMaxVolumeDB.max, value));
                DemoManager.Settings.MusicVolume = value;
            });

            // Sound Effects Volume
            soundEffectsVolumeSlider.SetValueWithoutNotify(DemoManager.Settings.SoundEffectsVolume);
            soundEffectsVolumeSlider.onValueChanged.AddListener(delegate (float value)
            {
                masterAudioMixer.SetFloat("SoundEffectsVolume", Mathf.Lerp(minMaxVolumeDB.min, minMaxVolumeDB.max, value));
                DemoManager.Settings.SoundEffectsVolume = value;
            });
            #endregion

            #region Gameplay
            // Online Username
            onlineUsernameTextField.SetTextWithoutNotify(DemoManager.Settings.OnlineUsername);
            onlineUsernameTextField.onValueChanged.AddListener(delegate (string text) 
            {
                DemoManager.Settings.OnlineUsername = text;
            });
            
            // Creature Preset(s)
            int presets = DemoManager.Settings.CreaturePresets.Count;
            creaturePresetsText.text = (presets == 0) ? "None" : $"{presets} presets";
            creaturePresetsButton.onClick.AddListener(delegate
            {
                FileBrowser.ShowLoadDialog(
                    onSuccess: delegate (string[] paths)
                    {
                        DemoManager.Settings.CreaturePresets.Clear();
                        foreach (string path in paths)
                        {
                            CreatureData creature = SaveUtility.Load<CreatureData>(path);
                            if (creature != null)
                            {
                                DemoManager.Settings.CreaturePresets.Add(creature);
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

            // Debug Mode
            debugModeToggle.SetIsOnWithoutNotify(DemoManager.Settings.DebugMode);
            debugModeToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DebugLogManager.Instance.gameObject.SetActive(isOn);
                DemoManager.Settings.DebugMode = isOn;
            });

            // Preview Features
            previewFeaturesToggle.SetIsOnWithoutNotify(DemoManager.Settings.PreviewFeatures);
            previewFeaturesToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.PreviewFeatures = isOn;
            });

            // Network Stats
            networkStatsToggle.SetIsOnWithoutNotify(DemoManager.Settings.NetworkStats);
            networkStatsToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.NetworkStats = isOn;
            });

            // Tutorial
            tutorialToggle.SetIsOnWithoutNotify(DemoManager.Settings.NetworkStats);
            tutorialToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.Tutorial = isOn;
            });

            // World Chat
            worldChatToggle.SetIsOnWithoutNotify(DemoManager.Settings.WorldChat);
            worldChatToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.WorldChat = isOn;
            });

            // Reset Progress
            resetProgressButton.onClick.AddListener(delegate
            {
                ConfirmationDialog.Confirm("Reset Progress?", "This will reset all your unlocked body parts and patterns, as well as your cash, level and experience. It will <u>not</u> remove your creatures.", noEvent: DemoManager.Progress.Reset);
            });
            #endregion

            #region Controls
            // Camera Sensitivity (Horizontal)
            cameraSensitivityHorizontalSlider.SetValueWithoutNotify(DemoManager.Settings.CameraSensitivityHorizontal);
            cameraSensitivityHorizontalSlider.onValueChanged.AddListener(delegate (float value)
            {
                DemoManager.Settings.CameraSensitivityHorizontal = value;
            });

            // Camera Sensitivity (Vertical)
            cameraSensitivityVerticalSlider.SetValueWithoutNotify(DemoManager.Settings.CameraSensitivityVertical);
            cameraSensitivityVerticalSlider.onValueChanged.AddListener(delegate (float value)
            {
                DemoManager.Settings.CameraSensitivityVertical = value;
            });

            // Invert Horizontal
            invertHorizontalToggle.SetIsOnWithoutNotify(DemoManager.Settings.InvertHorizontal);
            invertHorizontalToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.InvertHorizontal = isOn;
            });

            // Invert Vertical
            invertVerticalToggle.SetIsOnWithoutNotify(DemoManager.Settings.InvertVertical);
            invertVerticalToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.InvertVertical = isOn;
            });
            #endregion
        }
        #endregion
    }
}