// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ShopMenu : MenuSingleton<ShopMenu>
    {
        #region Fields
        [SerializeField] private GameObject shopButton;
        #endregion

        #region Methods
        private void Start()
        {
            shopButton.SetActive(!SettingsManager.Instance.ShowTutorial);
        }

        public void Visit()
        {
            Application.OpenURL("https://playcreature.com/merch");
        }
        #endregion
    }
}