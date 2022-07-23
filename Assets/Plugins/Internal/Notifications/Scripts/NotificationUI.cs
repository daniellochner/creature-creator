// Notifications
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class NotificationUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button closeButton;
        #endregion

        #region Methods
        public void Setup(Sprite icon, string title, string description, UnityAction onClose, float iconScale, float textScale)
        {
            iconImage.sprite = icon;
            titleText.text = title;
            descriptionText.text = description;

            iconImage.transform.localScale = Vector3.one * iconScale;
            titleText.fontSize *= textScale;
            descriptionText.fontSize *= textScale;

            if (onClose != null)
            {
                closeButton.onClick.AddListener(onClose);
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
        #endregion
    }
}