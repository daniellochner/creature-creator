// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class UnlockablePatternsMenu : Dialog<UnlockablePatternsMenu>
    {
        #region Fields
        [SerializeField] private GameObject hiddenIconPrefab;
        [SerializeField] private PatternUI patternUIPrefab;
        [SerializeField] private GridLayoutGroup patternGrid;
        #endregion

        #region Properties
        private Database Patterns => DatabaseManager.GetDatabase("Patterns");
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }
        private void Setup()
        {
            foreach (string patternID in Patterns.Objects.Keys)
            {
                Texture2D pattern = Patterns.GetEntry<Texture2D>(patternID);
                PatternUI patternUI = Instantiate(patternUIPrefab, patternGrid.transform);
                patternUI.Setup(pattern, null);
                patternUI.name = patternID;
                patternUI.SelectToggle.isOn = true;

                patternUI.SelectToggle.enabled = false;
                GameObject hiddenIconGO = Instantiate(hiddenIconPrefab, patternUI.transform);
                hiddenIconGO.SetActive(SettingsManager.Data.HiddenPatterns.Contains(patternID));
                patternUI.ClickUI.OnLeftClick.AddListener(delegate
                {
                    if (!ProgressManager.Data.UnlockedPatterns.Contains(patternID)) return;

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

        public override void Open(bool instant = false)
        {
            base.Open(instant);
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            foreach (PatternUI patternUI in patternGrid.GetComponentsInChildren<PatternUI>())
            {
                patternUI.CanvasGroup.alpha = ProgressManager.Data.UnlockedPatterns.Contains(patternUI.name) ? 1f : 0.2f;
            }
            //titleText.text = $"Unlocked Patterns ({ProgressManager.Data.UnlockedPatterns.Count}/{Patterns.Objects.Count})";
            titleText.SetArguments(ProgressManager.Data.UnlockedPatterns.Count, Patterns.Objects.Count);
        }
        #endregion
    }
}