// Interactions
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class InteractionUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image icon;
        #endregion

        #region Properties
        public Interactable Interactable { get; private set; }
        #endregion

        #region Methods
        public void Setup(Interactable interactable, ToggleGroup group)
        {
            Interactable = interactable;

            toggle.group = group;
            icon.sprite = interactable.Icon;
        }
        public void Select()
        {
            toggle.isOn = true;
        }
        #endregion
    }
}