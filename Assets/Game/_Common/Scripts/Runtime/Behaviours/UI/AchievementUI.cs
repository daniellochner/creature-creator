// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AchievementUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Image icon;
        [SerializeField] private HoverUI hoverUI;
        [SerializeField] private ClickUI clickUI;
        [SerializeField] public CanvasGroup canvasGroup;
        #endregion

        #region Methods
        public void Setup(string achievementId)
        {
            Achievement achievement = StatsManager.Instance.GetAchievement(achievementId);

            name = achievementId;
            icon.sprite = achievement.unlockedIcon;

            hoverUI.OnEnter.AddListener(delegate
            {
                if (!Input.GetMouseButton(0))
                {
                    AchievementMenu.Instance.Setup(achievement, Input.mousePosition);
                }
            });
            hoverUI.OnExit.AddListener(delegate
            {
                AchievementMenu.Instance.Clear();
            });

            clickUI.OnLeftClick.AddListener(delegate
            {
                if (SystemUtility.IsDevice(DeviceType.Handheld))
                {
                    StartCoroutine(ClickToOpenRoutine(achievement));
                }
            });
        }

        private IEnumerator ClickToOpenRoutine(Achievement achievement)
        {
            AchievementMenu.Instance.Setup(achievement, transform.position);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            AchievementMenu.Instance.Close();
        }
        #endregion
    }
}