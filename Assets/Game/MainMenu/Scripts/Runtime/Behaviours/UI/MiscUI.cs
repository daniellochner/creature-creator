// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MiscUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private CanvasGroup m_UI;
        private bool m_IsVisible = true;
        #endregion

        #region Methods
        private void Start()
        {
            if (!SettingsManager.Instance.ShowTutorial)
            {
                PremiumManager.Instance.ShowBannerAd();
            }
        }
        private void OnDestroy()
        {
            PremiumManager.Instance?.HideBannerAd();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.U))
            {
                m_IsVisible = !m_IsVisible;
                StartCoroutine(m_UI.FadeRoutine(m_IsVisible, 0.25f));
            }
        }

        public void SubscribeToYouTubeChannel()
        {
            Application.OpenURL("https://www.youtube.com/channel/UCGLR3v7NaV1t92dnzWZNSKA?sub_confirmation=1");
        }
        public void FollowTwitterAccount()
        {
            Application.OpenURL("https://twitter.com/daniellochner");
        }
        public void JoinDiscordServer()
        {
            Application.OpenURL("https://discord.gg/sJysbdu");
        }
        public void ViewGitHubSourceCode()
        {
            Application.OpenURL("https://github.com/daniellochner/creature-creator");
            Application.OpenURL("https://github.com/daniellochner/creature-creator-game");

#if USE_STATS
            StatsManager.Instance.UnlockAchievement("ACH_HACKERMAN");
#endif
        }
        public void Quit()
        {
            ConfirmationDialog.Confirm(LocalizationUtility.Localize("mainmenu_quit_title"), LocalizationUtility.Localize("mainmenu_quit_message"), onYes: Application.Quit);
        }
        #endregion
    }
}