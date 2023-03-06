// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AchievementMenu : MenuSingleton<AchievementMenu>
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Transform dialog;
        #endregion

        #region Properties
        public bool HasEntered { get; set; }
        #endregion

        #region Methods
        public void Setup(Achievement achievement, Vector3 position)
        {
            titleText.text = LocalizationUtility.Localize(achievement.name);
            descriptionText.text = LocalizationUtility.Localize(achievement.description);

            ContentSizeFitter[] fitters = GetComponentsInChildren<ContentSizeFitter>(true);
            for (int i = 0; i < fitters.Length; ++i)
            {
                ContentSizeFitter fitter = fitters[fitters.Length - i - 1];
                fitter.SetLayoutVertical();
            }

            dialog.position = position;
            Open(HasEntered);

            HasEntered = true;
        }
        public void Clear()
        {
            HasEntered = false;

            this.Invoke(delegate
            {
                if (!HasEntered)
                {
                    Close();
                }
            }, 0.15f);
        }
        #endregion
    }
}