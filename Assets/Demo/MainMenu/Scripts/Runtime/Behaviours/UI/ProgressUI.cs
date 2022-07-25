// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ProgressUI : MonoBehaviourSingleton<ProgressUI>
    {
        #region Fields
        [Header("Level/Experience")]
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Slider experienceSlider;

        [Header("Cash")]
        [SerializeField] private TextMeshProUGUI cashText;

        [Header("Unlocked Body Parts")]
        [SerializeField] private Slider bodyPartsSlider;
        [SerializeField] private TextMeshProUGUI bodyPartsText;
        [SerializeField] private Button bodyPartsButton;

        [Header("Unlocked Patterns")]
        [SerializeField] private Slider patternsSlider;
        [SerializeField] private TextMeshProUGUI patternsText;
        [SerializeField] private Button patternsButton;
        #endregion

        #region Methods
        private void Start()
        {
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
            Database bodyParts = DatabaseManager.GetDatabase("Body Parts");
            bodyPartsText.text = $"{ProgressManager.Data.UnlockedBodyParts.Count}/{bodyParts.Objects.Count}";
            bodyPartsSlider.maxValue = bodyParts.Objects.Count;
            bodyPartsSlider.value = ProgressManager.Data.UnlockedBodyParts.Count;
            bodyPartsButton.onClick.AddListener(delegate
            {
                UnlockableBodyPartsMenu.Instance.Open();
            });

            // Patterns
            Database patterns = DatabaseManager.GetDatabase("Patterns");
            patternsText.text = $"{ProgressManager.Data.UnlockedPatterns.Count}/{patterns.Objects.Count}";
            patternsSlider.maxValue = patterns.Objects.Count;
            patternsSlider.value = ProgressManager.Data.UnlockedPatterns.Count;
            patternsButton.onClick.AddListener(delegate
            {
                UnlockablePatternsMenu.Instance.Open();
            });
        }
        #endregion
    }
}