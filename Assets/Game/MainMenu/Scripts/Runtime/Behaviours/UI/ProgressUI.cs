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
        [Header("Achievements")]
        [SerializeField] private TextMeshProUGUI achievementsText;
        [SerializeField] private Slider achievementsSlider;
        [SerializeField] private Button achievementsButton;

        [Header("Quests")]
        [SerializeField] private Slider questsSlider;
        [SerializeField] private TextMeshProUGUI questsText;
        [SerializeField] private Button questsButton;

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
            // Achievements
            int unlocked = StatsManager.Instance.NumAchievementsUnlocked;
            int total = DatabaseManager.GetDatabase("Achievements").Objects.Count;
            achievementsText.text = $"{unlocked}/{total}";
            achievementsSlider.value = ((float)unlocked) / total;
            achievementsButton.onClick.AddListener(delegate
            {
                if (SystemUtility.IsDevice(DeviceType.Handheld) && GameServices.Instance.IsLoggedIn())
                {
                    GameServices.Instance.ShowAchievementsUI();
                }
                else
                {
                    AchievementsMenu.Instance.Open();
                }
            });

            // Quests
            int completedQuests = ProgressManager.Data.CompletedQuests.Count;
            questsText.text = $"{completedQuests}/{StatsManager.NUM_QUESTS}";
            questsSlider.maxValue = StatsManager.NUM_QUESTS;
            questsSlider.value = completedQuests;
            questsButton.onClick.AddListener(delegate
            {
                InformationDialog.Inform(LocalizationUtility.Localize("mainmenu_completed-quests_title"), LocalizationUtility.Localize("mainmenu_completed-quests_description", ProgressManager.Data.CompletedQuests.Count, StatsManager.NUM_QUESTS));
            });

            // Body Parts
            bodyPartsText.text = $"{ProgressManager.Data.UnlockedBodyParts.Count}/{StatsManager.NUM_BODY_PARTS}";
            bodyPartsSlider.maxValue = StatsManager.NUM_BODY_PARTS;
            bodyPartsSlider.value = ProgressManager.Data.UnlockedBodyParts.Count;
            bodyPartsButton.onClick.AddListener(delegate
            {
                UnlockableBodyPartsMenu.Instance.Open();
            });

            // Patterns
            patternsText.text = $"{ProgressManager.Data.UnlockedPatterns.Count}/{StatsManager.NUM_PATTERNS}";
            patternsSlider.maxValue = StatsManager.NUM_PATTERNS;
            patternsSlider.value = ProgressManager.Data.UnlockedPatterns.Count;
            patternsButton.onClick.AddListener(delegate
            {
                UnlockablePatternsMenu.Instance.Open();
            });
        }
        #endregion
    }
}