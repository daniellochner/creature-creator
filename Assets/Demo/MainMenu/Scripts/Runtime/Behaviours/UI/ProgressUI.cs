// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ProgressUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject hiddenIconPrefab;

        [Header("Level/Experience")]
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Slider experienceSlider;

        [Header("Cash")]
        [SerializeField] private TextMeshProUGUI cashText;

        [Header("Unlocked Body Parts")]
        [SerializeField] private BodyPartUI bodyPartUIPrefab;
        [SerializeField] private TextMeshProUGUI bodyPartsText;
        [SerializeField] private Slider bodyPartsSlider;
        [SerializeField] private TextMeshProUGUI bodyPartsTitleText;
        [SerializeField] private GridLayoutGroup bodyPartsGrid;

        [Header("Unlocked Patterns")]
        [SerializeField] private PatternUI patternUIPrefab;
        [SerializeField] private TextMeshProUGUI patternsText;
        [SerializeField] private Slider patternsSlider;
        [SerializeField] private TextMeshProUGUI patternsTitleText;
        [SerializeField] private GridLayoutGroup patternsGrid;
        #endregion

        #region Properties
        private Database BodyParts => DatabaseManager.GetDatabase("Body Parts");
        private Database Patterns  => DatabaseManager.GetDatabase("Patterns");
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }
        private void Setup()
        {
            // Body Parts
            foreach (string bodyPartID in BodyParts.Objects.Keys)
            {
                BodyPart bodyPart = BodyParts.GetEntry<BodyPart>(bodyPartID);
                BodyPartUI bodyPartUI = Instantiate(bodyPartUIPrefab, bodyPartsGrid.transform);
                bodyPartUI.Setup(bodyPart);
                bodyPartUI.name = bodyPartID;

                bodyPartUI.HoverUI.OnEnter.AddListener(delegate
                {
                    if (!Input.GetMouseButton(0))
                    {
                        StatisticsMenu.Instance.Setup(bodyPart);
                    }
                });
                bodyPartUI.HoverUI.OnExit.AddListener(delegate
                {
                    StatisticsMenu.Instance.Clear();
                });
                bodyPartUI.DragUI.enabled = false;

                GameObject hiddenIconGO = Instantiate(hiddenIconPrefab, bodyPartUI.transform);
                hiddenIconGO.SetActive(SettingsManager.Data.HiddenBodyParts.Contains(bodyPartID));
                bodyPartUI.ClickUI.OnLeftClick.AddListener(delegate
                {
                    int i = SettingsManager.Data.HiddenBodyParts.IndexOf(bodyPartID);
                    if (i == -1)
                    {
                        SettingsManager.Data.HiddenBodyParts.Add(bodyPartID);
                        hiddenIconGO.SetActive(true);
                    }
                    else
                    {
                        SettingsManager.Data.HiddenBodyParts.Remove(bodyPartID);
                        hiddenIconGO.SetActive(false);
                    }
                });
            }

            // Patterns
            foreach (string patternID in Patterns.Objects.Keys)
            {
                Texture2D pattern = Patterns.GetEntry<Texture2D>(patternID);
                PatternUI patternUI = Instantiate(patternUIPrefab, patternsGrid.transform);
                patternUI.Setup(pattern, null);
                patternUI.name = patternID;

                patternUI.SelectToggle.enabled = false;
                GameObject hiddenIconGO = Instantiate(hiddenIconPrefab, patternUI.transform);
                hiddenIconGO.SetActive(SettingsManager.Data.HiddenPatterns.Contains(patternID));
                patternUI.ClickUI.OnLeftClick.AddListener(delegate
                {
                    int i = SettingsManager.Data.HiddenPatterns.IndexOf(patternID);
                    if (i == -1)
                    {
                        SettingsManager.Data.HiddenPatterns.Add(patternID);
                        hiddenIconGO.SetActive(true);
                    }
                    else
                    {
                        SettingsManager.Data.HiddenPatterns.Remove(patternID);
                        hiddenIconGO.SetActive(false);
                    }
                });
            }

            UpdateInfo();
        }

        public void UpdateInfo()
        {
            // Level/Experience
            levelText.text = ProgressManager.Data.Level.ToString();
            experienceSlider.value = ProgressManager.Data.Experience;

            // Cash
            cashText.text = $"${ProgressManager.Data.Cash}";

            // Body Parts
            foreach (BodyPartUI bodyPartUI in bodyPartsGrid.GetComponentsInChildren<BodyPartUI>())
            {
                bodyPartUI.CanvasGroup.alpha = ProgressManager.Data.UnlockedBodyParts.Contains(bodyPartUI.name) ? 1f : 0.25f;
            }
            bodyPartsText.text = $"{ProgressManager.Data.UnlockedBodyParts.Count}/{BodyParts.Objects.Count}";
            bodyPartsSlider.maxValue = BodyParts.Objects.Count;
            bodyPartsSlider.value = ProgressManager.Data.UnlockedBodyParts.Count;
            bodyPartsTitleText.text = $"Unlocked Body Parts ({bodyPartsText.text})";

            // Patterns
            foreach (PatternUI patternUI in patternsGrid.GetComponentsInChildren<PatternUI>())
            {
                patternUI.CanvasGroup.alpha = ProgressManager.Data.UnlockedPatterns.Contains(patternUI.name) ? 1f : 0.25f;
            }
            patternsText.text = $"{ProgressManager.Data.UnlockedPatterns.Count}/{Patterns.Objects.Count}";
            patternsSlider.maxValue = Patterns.Objects.Count;
            patternsSlider.value = ProgressManager.Data.UnlockedPatterns.Count;
            patternsTitleText.text = $"Unlocked Patterns ({patternsText.text})";
        }
        #endregion
    }
}