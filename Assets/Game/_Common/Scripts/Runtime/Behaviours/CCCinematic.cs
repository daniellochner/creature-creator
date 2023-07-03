using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CCCinematic : Cinematic
    {
        #region Fields
        [SerializeField] private bool showBannerAd = true;
        #endregion

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
            InteractionsManager.Instance.enabled = false;
        }
        public override void Hide()
        {
            base.Hide();
            SetVisibility(false);
            SetNotificationOffset(-1);
            InteractionsManager.Instance.enabled = true;
        }

        public override void Begin()
        {
            base.Begin();
            if (showBannerAd)
            {
                PremiumManager.Instance?.ShowBannerAd();
            }

            InformationDialog.Instance?.Close();
            InputDialog.Instance?.Close();
            ConfirmationDialog.Instance?.Close();
            KeybindingsDialog.Instance?.Close();
            UnlockableBodyPartsMenu.Instance?.Close();
            UnlockablePatternsMenu.Instance?.Close();
            AchievementsMenu.Instance?.Close();
            LocalizationMenu.Instance?.Close();
            PremiumMenu.Instance?.Close();
            RewardsMenu.Instance?.Close();
            PauseMenu.Instance?.Close();
            NetworkPlayersMenu.Instance?.Close();
        }
        public override void End()
        {
            base.End();
            PremiumManager.Instance?.HideBannerAd();
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