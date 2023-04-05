// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class UnlockableBodyPartsMenu : Dialog<UnlockableBodyPartsMenu>
    {
        #region Fields
        [SerializeField] protected GameObject hiddenIconPrefab;
        [SerializeField] private BodyPartUI bodyPartUIPrefab;
        [SerializeField] private GridLayoutGroup bodyPartGrid;
        #endregion

        #region Properties
        private Database BodyParts => DatabaseManager.GetDatabase("Body Parts");
        #endregion

        #region Methods
        protected override void Start()
        {
            base.Start();
            Setup();
        }
        private void Setup()
        {
            foreach (string bodyPartID in BodyParts.Objects.Keys)
            {
                BodyPart bodyPart = BodyParts.GetEntry<BodyPart>(bodyPartID);
                BodyPartUI bodyPartUI = Instantiate(bodyPartUIPrefab, bodyPartGrid.transform);
                bodyPartUI.Setup(bodyPart);
                bodyPartUI.name = bodyPartID;

                bodyPartUI.DragUI.enabled = false;

                GameObject hiddenIconGO = Instantiate(hiddenIconPrefab, bodyPartUI.transform);
                hiddenIconGO.SetActive(SettingsManager.Data.HiddenBodyParts.Contains(bodyPartID));
                bodyPartUI.ClickUI.OnRightClick.AddListener(delegate
                {
                    if (!ProgressManager.Data.UnlockedBodyParts.Contains(bodyPartID)) return;

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
            UpdateInfo();
        }

        public override void Open(bool instant = false)
        {
            base.Open(instant);
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            foreach (BodyPartUI bodyPartUI in bodyPartGrid.GetComponentsInChildren<BodyPartUI>())
            {
                bodyPartUI.CanvasGroup.alpha = ProgressManager.Data.UnlockedBodyParts.Contains(bodyPartUI.name) ? 1f : 0.2f;
            }
            //titleText.text = $"Unlocked Body Parts ({ProgressManager.Data.UnlockedBodyParts.Count}/{BodyParts.Objects.Count})";
            titleText.SetArguments(ProgressManager.Data.UnlockedBodyParts.Count, BodyParts.Objects.Count);
        }
        #endregion
    }
}