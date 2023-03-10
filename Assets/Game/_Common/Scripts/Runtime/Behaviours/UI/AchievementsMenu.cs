using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AchievementsMenu : Dialog<AchievementsMenu>
    {
        #region Fields
        [SerializeField] private AchievementUI achievementUIPrefab;
        [SerializeField] private GridLayoutGroup achievementsGrid;
        #endregion

        #region Properties
        private Database Achievements => DatabaseManager.GetDatabase("Achievements");
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            foreach (string achievementId in Achievements.Objects.Keys)
            {
                AchievementUI achievementUI = Instantiate(achievementUIPrefab, achievementsGrid.transform);
                achievementUI.Setup(achievementId);
            }
            UpdateInfo();
        }
        public void UpdateInfo()
        {
            foreach (AchievementUI achievementUI in achievementsGrid.GetComponentsInChildren<AchievementUI>())
            {
                achievementUI.canvasGroup.alpha = StatsManager.Instance.IsAchievementUnlocked(achievementUI.name) ? 1f : 0.2f;
            }
            titleText.SetArguments(StatsManager.Instance.NumAchievementsUnlocked, Achievements.Objects.Count);
        }

        public override void Open(bool instant = false)
        {
            base.Open(instant);
            UpdateInfo();

            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                GameServices.Instance.ShowAchievementsUI();
            }
        }    
        #endregion
    }
}