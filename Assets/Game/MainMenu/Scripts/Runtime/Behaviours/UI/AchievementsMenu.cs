using Steamworks;
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

        #region Methods
        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            foreach (var achievement in DatabaseManager.GetDatabase("Achievements").Objects)
            {
                AchievementUI achievementUI = Instantiate(achievementUIPrefab, achievementsGrid.transform);
                achievementUI.Setup(achievement.Value as Achievement);
            }
            UpdateInfo();
        }
        public void UpdateInfo()
        {
            foreach (AchievementUI achievementUI in achievementsGrid.GetComponentsInChildren<AchievementUI>())
            {
                achievementUI.canvasGroup.alpha = StatsManager.Instance.GetAchievement(achievementUI.name) ? 1f : 0.2f;
            }
            titleText.text = $"Unlocked Achievements ({StatsManager.Instance.NumAchievementsUnlocked}/{SteamUserStats.GetNumAchievements()})";
        }

        public override void Open(bool instant = false)
        {
            base.Open(instant);
            UpdateInfo();
        }    
        #endregion
    }
}