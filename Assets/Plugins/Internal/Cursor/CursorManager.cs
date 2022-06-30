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

        [SerializeField] private bool useCustomCursor;

        private void OnEnable()
        {
            if (useCustomCursor) Cursor.visible = false;
        }
        private void OnDisable()
        {
            if (useCustomCursor) Cursor.visible = true;
        }
        private void Update()
        {
            if (useCustomCursor) cursorIcon.gameObject.SetActive(!Cursor.visible);
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