// Notifications
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class TextOnlyNotificationUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI messageText;
        #endregion

        #region Methods
        public void Setup(string message)
        {
            messageText.text = message;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
        #endregion
    }
}