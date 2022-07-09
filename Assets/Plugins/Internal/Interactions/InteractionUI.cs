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
            icon.sprite = Sprite.Create(interactable.Cursor, new Rect(Vector2.zero, new Vector2(interactable.Cursor.width, interactable.Cursor.height)), Vector2.one / 2f);
        }
        public void Select()
        {
            toggle.isOn = true;
        }
        #endregion
    }
}