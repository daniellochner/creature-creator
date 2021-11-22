// Notifications
// Copyright (c) Daniel Lochner

using UnityEngine;

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

        #region Methods
        public static void Notify(string message)
        {
            Instantiate(Instance.notificationTextOnlyPrefab, Instance.notificationsRT).Setup(message);
        }
        public static void Notify(Sprite icon, string title, string description)
        {
            Instantiate(Instance.notificationPrefab, Instance.notificationsRT).Setup(icon, title, description);
        }

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
        #endregion
    }
}