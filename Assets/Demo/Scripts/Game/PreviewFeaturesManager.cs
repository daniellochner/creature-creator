// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class PreviewFeaturesManager : MonoBehaviourSingleton<PreviewFeaturesManager>
    {
        #region Fields
        [SerializeField] private RectTransform optionsRT;
        [SerializeField] private GameObject importGO;
        [SerializeField] private GameObject exportGO;
        #endregion

        #region Methods
        private void Start()
        {
            if (SettingsManager.Data.PreviewFeatures)
            {
                optionsRT.sizeDelta = new Vector2(0f, 325f);
                importGO.SetActive(true);
                exportGO.SetActive(true);
            }
        }
        #endregion
    }
}