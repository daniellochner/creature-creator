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

        #region Methods
        private void Start()
        {
            Setup();
        }
        private void Setup()
        {
            // Level/Experience
            levelText.text = ProgressManager.Data.Level.ToString();
            experienceSlider.value = ProgressManager.Data.Experience;

            // Cash
            cashText.text = $"${ProgressManager.Data.Cash}";

            // Body Parts
            Database bodyParts = DatabaseManager.GetDatabase("Body Parts");
            foreach (string bodyPartID in bodyParts.Objects.Keys)
            {
                BodyPart bodyPart = bodyParts.GetEntry<BodyPart>(bodyPartID);
                BodyPartUI bodyPartUI = Instantiate(bodyPartUIPrefab, bodyPartsGrid.transform);
                bodyPartUI.Setup(bodyPart);

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
                if (!ProgressManager.Data.UnlockedBodyParts.Contains(bodyPartID))
                {
                    bodyPartUI.CanvasGroup.alpha = 0.25f;
                }
            }
            bodyPartsText.text = $"{ProgressManager.Data.UnlockedBodyParts.Count}/{bodyParts.Objects.Count}";
            bodyPartsSlider.maxValue = bodyParts.Objects.Count;
            bodyPartsSlider.value = ProgressManager.Data.UnlockedBodyParts.Count;
            bodyPartsTitleText.text = $"Unlocked Body Parts ({bodyPartsText.text})";

            // Patterns
            Database patterns = DatabaseManager.GetDatabase("Patterns");
            foreach (string patternID in patterns.Objects.Keys)
            {
                Texture2D pattern = patterns.GetEntry<Texture2D>(patternID);
                PatternUI patternUI = Instantiate(patternUIPrefab, patternsGrid.transform);
                patternUI.Setup(pattern);

                patternUI.SelectToggle.enabled = false;
                patternUI.ClickUI.enabled = false;
                if (!ProgressManager.Data.UnlockedPatterns.Contains(patternID))
                {
                    patternUI.CanvasGroup.alpha = 0.25f;
                }
            }
            patternsText.text = $"{ProgressManager.Data.UnlockedPatterns.Count}/{patterns.Objects.Count}";
            patternsSlider.maxValue = patterns.Objects.Count;
            patternsSlider.value = ProgressManager.Data.UnlockedPatterns.Count;
            patternsTitleText.text = $"Unlocked Patterns ({patternsText.text})";
        }
        #endregion
    }
}