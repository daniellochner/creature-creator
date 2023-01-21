// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using SimpleFileBrowser;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
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

        [Header("Audio")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundEffectsVolumeSlider;
        [SerializeField] private OptionSelector inGameMusicOS;

        [Header("Gameplay")]
        [SerializeField] private TMP_InputField onlineUsernameTextField;
        [SerializeField] private TextMeshProUGUI creaturePresetsText;
        [SerializeField] private Slider exportPrecisionSlider;
        [SerializeField] private Button creaturePresetsButton;
        [SerializeField] private OptionSelector relayServerOS;
        [SerializeField] private Toggle cameraShakeToggle;
        [SerializeField] private Toggle debugModeToggle;
        [SerializeField] private Toggle previewFeaturesToggle;
        [SerializeField] private Toggle networkStatsToggle;
        [SerializeField] private Toggle tutorialToggle;
        [SerializeField] private Toggle worldChatToggle;
        [SerializeField] private Toggle mapToggle;
        [SerializeField] private Button resetProgressButton;

        [Header("Controls")]
        [SerializeField] private Slider sensitivityHorizontalSlider;
        [SerializeField] private Slider sensitivityVerticalSlider;
        [SerializeField] private Toggle invertHorizontalToggle;
        [SerializeField] private Toggle invertVerticalToggle;

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
            // Resolution
            for (int i = 0; i < Screen.resolutions.Length; ++i)
            {
                Resolution resolution = Screen.resolutions[i];
                resolutionOS.Options.Add(new OptionSelector.Option()
                {
                    Id = $"{resolution.width}x{resolution.height} @ {resolution.refreshRate}Hz"
                });

                Resolution current = SettingsManager.Data.Resolution;
                if ((resolution.width == current.width) && (resolution.height == current.height) && (resolution.refreshRate == current.refreshRate))
                {
                    resolutionOS.Select(i, false);
                }
            }

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
            presetOS.SetupUsingEnum<PresetType>(true);
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
            creatureMeshQualityOS.SetupUsingEnum<CreatureMeshQualityType>(true);
            creatureMeshQualityOS.Select(SettingsManager.Data.CreatureMeshQuality, false);
            creatureMeshQualityOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetCreatureMeshQuality((CreatureMeshQualityType)option);
            });

            // Shadow Quality
            shadowQualityOS.SetupUsingEnum<ShadowQualityType>(true);
            shadowQualityOS.Select(SettingsManager.Data.ShadowQuality, false);
            shadowQualityOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetShadowQuality((ShadowQualityType)option);
            });

            // Texture Quality
            textureQualityOS.SetupUsingEnum<TextureQualityType>(true);
            textureQualityOS.Select(SettingsManager.Data.TextureQuality, false);
            textureQualityOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetTextureQuality((TextureQualityType)option);
            });

            // Ambient Occlusion
            ambientOcclusionOS.SetupUsingEnum<AmbientOcclusionType>(true);
            ambientOcclusionOS.Select(SettingsManager.Data.AmbientOcclusion, false);
            ambientOcclusionOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetAmbientOcclusion((AmbientOcclusionType)option);
            });

            // Antialiasing
            antialiasingOS.SetupUsingEnum<AntialiasingType>(true);
            antialiasingOS.Select(SettingsManager.Data.Antialiasing, false);
            antialiasingOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetAntialiasing((AntialiasingType)option, true);
            });

            // Screen Space Reflections
            screenSpaceReflectionsOS.SetupUsingEnum<ScreenSpaceReflectionsType>(true);
            screenSpaceReflectionsOS.Select(SettingsManager.Data.ScreenSpaceReflections, false);
            screenSpaceReflectionsOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetScreenSpaceReflections((ScreenSpaceReflectionsType)option);
            });

            // Foliage
            foliageOS.SetupUsingEnum<FoliageType>(true);
            foliageOS.Select(SettingsManager.Data.Foliage, false);
            foliageOS.OnSelected.AddListener(delegate (int option)
            {
                SettingsManager.Instance.SetFoliage((FoliageType)option);
            });

            // Reflections
            reflectionsToggle.SetIsOnWithoutNotify(SettingsManager.Data.Reflections);
            reflectionsToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetReflections(isOn);
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

            // Background Music
            inGameMusicOS.SetupUsingEnum<InGameMusicType>(false);
            inGameMusicOS.OnSelected.AddListener(delegate (int option)
            {
                InGameMusicType type = (InGameMusicType)option;
                SettingsManager.Instance.SetInGameMusic(type);

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
            inGameMusicOS.Select(SettingsManager.Data.InGameMusic, inGame);
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
            creaturePresetsText.text = presets.ToString();
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
                    initialPath: Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                );
            });

            exportPrecisionSlider.value = SettingsManager.Data.ExportPrecision;
            exportPrecisionSlider.onValueChanged.AddListener(delegate (float precision)
            {
                SettingsManager.Instance.SetExportPrecision((int)precision);
            });

            // Camera Shake
            cameraShakeToggle.SetIsOnWithoutNotify(SettingsManager.Data.CameraShake);
            cameraShakeToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetCameraShake(isOn, true);
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
            tutorialToggle.SetIsOnWithoutNotify(SettingsManager.Data.Tutorial);
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

            // Map
            mapToggle.SetIsOnWithoutNotify(SettingsManager.Data.Map);
            mapToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetMap(isOn);
                if (inGame)
                {
                    MinimapManager.Instance.SetVisibility(isOn);
                }
            });

            // Reset Progress
            resetProgressButton.onClick.AddListener(delegate
            {
                ConfirmationDialog.Confirm(LocalizationUtility.Localize("settings_reset-progress_title"), LocalizationUtility.Localize("settings_reset-progress_message"), onYes: ResetProgress);
            });
            #endregion

            #region Controls
            // Sensitivity (Horizontal)
            sensitivityHorizontalSlider.value = SettingsManager.Data.SensitivityHorizontal;
            sensitivityHorizontalSlider.onValueChanged.AddListener(delegate (float value)
            {
                SettingsManager.Instance.SetSensitivityHorizontal(value, inGame);
            });

            // Sensitivity (Vertical)
            sensitivityVerticalSlider.value = SettingsManager.Data.SensitivityVertical;
            sensitivityVerticalSlider.onValueChanged.AddListener(delegate (float value)
            {
                SettingsManager.Instance.SetSensitivityVertical(value, inGame);
            });

            // Invert Horizontal
            invertHorizontalToggle.SetIsOnWithoutNotify(SettingsManager.Data.InvertHorizontal);
            invertHorizontalToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetInvertHorizontal(isOn, inGame);
            });

            // Invert Vertical
            invertVerticalToggle.SetIsOnWithoutNotify(SettingsManager.Data.InvertVertical);
            invertVerticalToggle.onValueChanged.AddListener(delegate (bool isOn)
            {
                SettingsManager.Instance.SetInvertVertical(isOn, inGame);
            });
            #endregion
        }
        
        #region Video
        public void ApplyResolution()
        {
            SettingsManager.Instance.SetResolution(Screen.resolutions[resolutionOS.Selected]);
        }
        #endregion

        #region Audio
        private IEnumerator PreviewMusicRoutine(string music)
        {
            MusicManager.Instance.FadeTo(music);
            yield return new WaitForSeconds(5f);
            MusicManager.Instance.FadeTo(null);
        }
        #endregion

        #region Gameplay
        public void ViewUnlockableBodyParts()
        {
            UnlockableBodyPartsMenu.Instance.Open();
        }
        public void ViewUnlockablePatterns()
        {
            UnlockablePatternsMenu.Instance.Open();
        }
        public void ViewAchievements()
        {
            AchievementsMenu.Instance.Open();
        }
        public void ChooseLanguage()
        {
            LocalizationMenu.Instance.Open();
        }

        public void ResetProgress()
        {
            ProgressManager.Instance.Revert();
#if USE_STATS
            StatsManager.Instance.Revert();
#endif

            ProgressUI.Instance.UpdateInfo();
            UnlockableBodyPartsMenu.Instance.UpdateInfo();
            UnlockablePatternsMenu.Instance.UpdateInfo();
        }
        #endregion
        #endregion
    }
}