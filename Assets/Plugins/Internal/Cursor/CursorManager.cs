// Cursor
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class CursorManager : MonoBehaviourSingleton<CursorManager>
    {
        [SerializeField] private Sprite defaultCursorIcon;

        [SerializeField] private Image cursorIcon;
        [SerializeField] private Image cursorSubIcon;

        private void Update()
        {
            cursorIcon.gameObject.SetActive(!Cursor.visible);
        }

        private void OnEnable()
        {
            Cursor.visible = false;
        }
        private void OnDisable()
        {
            Cursor.visible = true;
        }

        public void SetIcon(Sprite icon)
        {
            if (icon == null)
            {
                icon = defaultCursorIcon;
            }
            cursorIcon.sprite = icon;
        }
        public void SetSubIcon(Sprite icon)
        {
            cursorSubIcon.gameObject.SetActive(icon != null);
            cursorSubIcon.sprite = icon;
        }

        public void SetScale(float scale)
        {
            cursorIcon.transform.localScale = scale * Vector3.one;
        }
    }
}