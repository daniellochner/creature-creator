// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class PreviewFeaturesManager : MonoBehaviourSingleton<PreviewFeaturesManager>
    {
        #region Fields
        [SerializeField] private SimpleSideMenu optionsSSM;
        [SerializeField] private GameObject importGO;
        [SerializeField] private GameObject exportGO;
        #endregion

        #region Methods
        private void Start()
        {
            if (SettingsManager.Data.PreviewFeatures)
            {
                optionsSSM.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 325f);
                optionsSSM.Setup();

                importGO.SetActive(true);
                exportGO.SetActive(true);
            }
        }
        #endregion
    }
}