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
        [Space]
        [SerializeField, Button("TestNotify")] private bool testNotify;
        #endregion

        #region Properties
        public float OffsetX
        {
            get => notificationsRT.offsetMin.x;
            set => notificationsRT.offsetMin = new Vector2(value, 0);
        }
        public float OffsetY
        {
            get => notificationsRT.offsetMax.y;
            set => notificationsRT.offsetMax = new Vector2(0, value);
        }

        public bool IsHidden { get; set; } = false;
        #endregion

        #region Methods
        public static void Notify(string message, UnityAction onClose = null)
        {
            if (Instance.IsHidden) return;

            Instantiate(Instance.notificationTextOnlyPrefab, Instance.notificationsRT).Setup(message, onClose);
        }
        public static void Notify(Sprite icon, string title, string description, UnityAction onClose = null, float iconScale = 1f, float textScale = 1f)
        {
            if (Instance.IsHidden) return;

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