using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DailyRewardManager : MonoBehaviourSingleton<DailyRewardManager>
    {
        private IEnumerator Start()
        {
            if (SettingsManager.Instance.ShowTutorial || PremiumManager.Instance.IsEverythingUsable()) yield break;

            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => WorldTimeManager.Instance.IsInitialized);

            string today = WorldTimeManager.Instance.UtcNow.Value.ToShortDateString();
            if (PlayerPrefs.GetString("DAILY_REWARD") != today)
            {
                RewardsMenu.Instance.Clear();
                PremiumManager.Instance.RequestedItem = null;
                PremiumManager.Instance.AccessRandom(4);
                RewardsMenu.Instance.Open(false, LocalizationUtility.Localize("daily-reward_title"));

                PlayerPrefs.SetString("DAILY_REWARD", today);
            }
        }
    }
}