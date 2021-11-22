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
        public void Setup(Sprite icon, string title, string description)
        {
            iconImage.sprite = icon;
            titleText.text = title;
            descriptionText.text = description;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
        #endregion
    }
}