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
            SetNotificationsHeight(150);
        }
        public override void Hide()
        {
            base.Hide();
            SetVisibility(false);
            SetNotificationsHeight(0);
        }

        private void SetNotificationsHeight(int height)
        {
            var group = NotificationsManager.Instance.GetComponentInChildren<VerticalLayoutGroup>();
            group.padding.top = height + 10;
            group.enabled = false;
            group.enabled = true;
        }
        #endregion
    }
}