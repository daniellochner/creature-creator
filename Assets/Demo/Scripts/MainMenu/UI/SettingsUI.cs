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
        [SerializeField] private Toggle cameraShakeToggle;
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
                    Name = $"{resolution.width}x{resolution.height} @ {resolution.refreshRate}Hz"
                });
            }
            resolutionOS.OnSelected.AddListener(delegate (int option)
            {
                Screen.SetResolution(Screen.resolutions[option].width, Screen.resolutions[option].height, Screen.fullScreen, Screen.resolutions[option].refreshRate);
                DemoManager.Settings.Resolution = option;
            });
            resolutionOS.Select(DemoManager.Settings.Resolution);

            // Fullscreen
            fullscreenToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                Screen.fullScreen = isOn;
                DemoManager.Settings.Fullscreen = isOn;
            });
            fullscreenToggle.isOn = DemoManager.Settings.Fullscreen;

            // V-Sync
            vSyncToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.VSync = isOn;
            });
            vSyncToggle.isOn = DemoManager.Settings.VSync;

            // Preset
            presetOS.SetupUsingEnum<PresetType>();
            presetOS.OnSelected.AddListener(delegate (int option)
            {
            });
            presetOS.Select(PresetType.Custom);

            // Creature Mesh Quality
            creatureMeshQualityOS.SetupUsingEnum<CreatureMeshQualityType>();
            creatureMeshQualityOS.OnSelected.AddListener(delegate (int option)
            {
                DemoManager.Settings.CreatureMeshQuality = (CreatureMeshQualityType)option;
            });
            creatureMeshQualityOS.Select(DemoManager.Settings.CreatureMeshQuality);

            // Shadow Quality
            shadowQualityOS.SetupUsingEnum<ShadowQualityType>();
            shadowQualityOS.OnSelected.AddListener(delegate (int option)
            {
                DemoManager.Settings.ShadowQuality = (ShadowQualityType)option;
            });
            shadowQualityOS.Select(DemoManager.Settings.ShadowQuality);

            // Texture Quality
            textureQualityOS.SetupUsingEnum<TextureQualityType>();
            textureQualityOS.OnSelected.AddListener(delegate (int option)
            {
                DemoManager.Settings.TextureQuality = (TextureQualityType)option;
            });
            textureQualityOS.Select(DemoManager.Settings.TextureQuality);

            // Ambient Occlusion
            ambientOcclusionOS.SetupUsingEnum<AmbientOcclusionType>();
            ambientOcclusionOS.OnSelected.AddListener(delegate (int option)
            {
                DemoManager.Settings.AmbientOcclusion = (AmbientOcclusionType)option;
            });
            ambientOcclusionOS.Select(DemoManager.Settings.AmbientOcclusion);

            // Antialiasing
            antialiasingOS.SetupUsingEnum<AntialiasingType>();
            antialiasingOS.OnSelected.AddListener(delegate (int option)
            {
                DemoManager.Settings.Antialiasing = (AntialiasingType)option;
            });
            antialiasingOS.Select(DemoManager.Settings.Antialiasing);

            // Screen Space Reflections
            screenSpaceReflectionsOS.SetupUsingEnum<ScreenSpaceReflectionsType>();
            screenSpaceReflectionsOS.OnSelected.AddListener(delegate (int option)
            {
                DemoManager.Settings.ScreenSpaceReflections = (ScreenSpaceReflectionsType)option;
            });
            screenSpaceReflectionsOS.Select(DemoManager.Settings.ScreenSpaceReflections);

            // Anisotropic Filtering
            anisotropicFilteringToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.AnisotropicFiltering = isOn;
            });
            anisotropicFilteringToggle.isOn = DemoManager.Settings.AnisotropicFiltering;

            // Bloom
            bloomToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.Bloom = isOn;
            });
            bloomToggle.isOn = DemoManager.Settings.Bloom;

            // Depth Of Field
            depthOfFieldToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.DepthOfField = isOn;
            });
            depthOfFieldToggle.isOn = DemoManager.Settings.DepthOfField;

            // Motion Blur
            motionBlurToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.MotionBlur = isOn;
            });
            motionBlurToggle.isOn = DemoManager.Settings.MotionBlur;
            #endregion

            #region Audio
            // Master Volume
            masterVolumeSlider.onValueChanged.AddListener(delegate (float value)
            {
                masterAudioMixer.SetFloat("MasterVolume", Mathf.Lerp(minMaxVolumeDB.min, minMaxVolumeDB.max, value));
                DemoManager.Settings.MasterVolume = value;
            });
            masterVolumeSlider.value = DemoManager.Settings.MasterVolume;

            // Music Volume
            musicVolumeSlider.onValueChanged.AddListener(delegate (float value)
            {
                masterAudioMixer.SetFloat("MusicVolume", Mathf.Lerp(minMaxVolumeDB.min, minMaxVolumeDB.max, value));
                DemoManager.Settings.MusicVolume = value;
            });
            musicVolumeSlider.value = DemoManager.Settings.MusicVolume;

            // Sound Effects Volume
            soundEffectsVolumeSlider.onValueChanged.AddListener(delegate (float value)
            {
                masterAudioMixer.SetFloat("SoundEffectsVolume", Mathf.Lerp(minMaxVolumeDB.min, minMaxVolumeDB.max, value));
                DemoManager.Settings.SoundEffectsVolume = value;
            });
            soundEffectsVolumeSlider.value = DemoManager.Settings.SoundEffectsVolume;
            #endregion

            #region Gameplay
            // Online Username
            onlineUsernameTextField.onValueChanged.AddListener(delegate (string text) 
            {
                DemoManager.Settings.OnlineUsername = text;
            });
            onlineUsernameTextField.text = DemoManager.Settings.OnlineUsername;
            
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

            // Camera Shake
            cameraShakeToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.CameraShake = isOn;
            });
            cameraShakeToggle.isOn = DemoManager.Settings.CameraShake;

            // Debug Mode
            debugModeToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DebugLogManager.Instance.gameObject.SetActive(isOn);
                DemoManager.Settings.DebugMode = isOn;
            });
            debugModeToggle.isOn = DemoManager.Settings.DebugMode;

            // Preview Features
            previewFeaturesToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.PreviewFeatures = isOn;
            });
            previewFeaturesToggle.isOn = DemoManager.Settings.PreviewFeatures;

            // Network Stats
            networkStatsToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.NetworkStats = isOn;
            });
            networkStatsToggle.isOn = DemoManager.Settings.NetworkStats;

            // Tutorial
            tutorialToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.Tutorial = isOn;
            });
            tutorialToggle.isOn = DemoManager.Settings.NetworkStats;

            // World Chat
            worldChatToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.WorldChat = isOn;
            });
            worldChatToggle.isOn = DemoManager.Settings.WorldChat;

            // Reset Progress
            resetProgressButton.onClick.AddListener(delegate
            {
                ConfirmationDialog.Confirm("Reset Progress?", "This will reset all your unlocked body parts and patterns, as well as your cash, level and experience. It will <u>not</u> remove your creatures.", noEvent: DemoManager.Progress.Reset);
            });
            #endregion

            #region Controls
            // Camera Sensitivity (Horizontal)
            cameraSensitivityHorizontalSlider.onValueChanged.AddListener(delegate (float value)
            {
                DemoManager.Settings.CameraSensitivityHorizontal = value;
            });
            cameraSensitivityHorizontalSlider.value = DemoManager.Settings.CameraSensitivityHorizontal;

            // Camera Sensitivity (Vertical)
            cameraSensitivityVerticalSlider.onValueChanged.AddListener(delegate (float value)
            {
                DemoManager.Settings.CameraSensitivityVertical = value;
            });
            cameraSensitivityVerticalSlider.value = DemoManager.Settings.CameraSensitivityVertical;

            // Invert Horizontal
            invertHorizontalToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.InvertHorizontal = isOn;
            });
            invertHorizontalToggle.isOn = DemoManager.Settings.InvertHorizontal;

            // Invert Vertical
            invertVerticalToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                DemoManager.Settings.InvertVertical = isOn;
            });
            invertVerticalToggle.isOn = DemoManager.Settings.InvertVertical;
            #endregion
        }
        #endregion
    }
}