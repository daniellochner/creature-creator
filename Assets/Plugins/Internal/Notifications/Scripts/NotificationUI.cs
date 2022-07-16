// Notifications
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class NotificationUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        #endregion

        #region Methods
        public void Setup(Sprite icon, string title, string description, float iconScale, float textScale)
        {
            iconImage.sprite = icon;
            titleText.text = title;
            descriptionText.text = description;

            iconImage.transform.localScale = Vector3.one * iconScale;
            titleText.fontSize *= textScale;
            descriptionText.fontSize *= textScale;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
        #endregion
    }
}