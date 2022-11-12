using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AchievementsMenu : Dialog<AchievementsMenu>
    {
        #region Fields
        [SerializeField] private AchievementUI achievementUIPrefab;
        [SerializeField] private GridLayoutGroup bodyPartGrid;
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }

        private void Setup()
        {

            UpdateInfo();
        }

        public override void Open(bool instant = false)
        {
            base.Open(instant);
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            //foreach (BodyPartUI bodyPartUI in bodyPartGrid.GetComponentsInChildren<BodyPartUI>())
            //{
            //    bodyPartUI.CanvasGroup.alpha = ProgressManager.Data.UnlockedBodyParts.Contains(bodyPartUI.name) ? 1f : 0.2f;
            //}
            titleText.text = $"Unlocked Achievements (/)";
        }
        #endregion
    }
}