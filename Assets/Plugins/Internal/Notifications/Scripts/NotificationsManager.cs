// Notifications
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class NotificationsManager : MonoBehaviourSingleton<NotificationsManager>
    {
        #region Fields
        [SerializeField] private RectTransform notificationsRT;
        [SerializeField] private NotificationUI notificationPrefab;
        [SerializeField] private TextOnlyNotificationUI notificationTextOnlyPrefab;
        [SerializeField] private float mobileOffset;
        [Space]
        [SerializeField, Button("TestNotify")] private bool testNotify;
        #endregion

        #region Methods
        private void Start()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                notificationsRT.offsetMax = Vector2.up * mobileOffset;
            }
        }

        public static void Notify(string message, UnityAction onClose = null)
        {
            Instantiate(Instance.notificationTextOnlyPrefab, Instance.notificationsRT).Setup(message, onClose);
        }
        public static void Notify(Sprite icon, string title, string description, UnityAction onClose = null, float iconScale = 1f, float textScale = 1f)
        {
            Instantiate(Instance.notificationPrefab, Instance.notificationsRT).Setup(icon, title, description, onClose, iconScale, textScale);
        }

#if UNITY_EDITOR
        public void TestNotify()
        {
            if (Application.isPlaying)
            {
                if (Random.Range(0f, 1f) > 0.5f)
                {
                    Notify("Message");
                }
                else
                {
                    Notify(null, "Title", "Description");
                }
            }
        }
#endif
        #endregion
    }
}