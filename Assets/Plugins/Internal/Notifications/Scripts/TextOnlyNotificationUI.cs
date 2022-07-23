// Notifications
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class TextOnlyNotificationUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button closeButton;
        #endregion

        #region Methods
        public void Setup(string message, UnityAction onClose)
        {
            messageText.text = message;

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