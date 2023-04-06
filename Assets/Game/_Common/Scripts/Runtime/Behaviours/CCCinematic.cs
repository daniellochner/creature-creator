using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CCCinematic : Cinematic
    {
        #region Methods
        private void SetVisibility(bool isVisible)
        {
            if (Player.Instance)
            {
                Player.Instance.Camera.enabled = !isVisible;
            }
        }
        public override void Show()
        {
            base.Show();
            SetVisibility(true);
            SetNotificationOffset(1);
        }
        public override void Hide()
        {
            base.Hide();
            SetVisibility(false);
            SetNotificationOffset(-1);
        }

        private void SetNotificationOffset(int dir)
        {
            int value = SystemUtility.IsDevice(DeviceType.Handheld) ? 40 : 150;

            var group = NotificationsManager.Instance.GetComponentInChildren<VerticalLayoutGroup>();
            group.padding.top += dir * value;
            group.enabled = false;
            group.enabled = true;
        }
        #endregion
    }
}