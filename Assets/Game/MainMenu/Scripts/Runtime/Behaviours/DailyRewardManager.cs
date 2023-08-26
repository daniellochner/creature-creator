// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DailyRewardManager : MonoBehaviourSingleton<DailyRewardManager>
    {
        #region Methods
        public IEnumerator Setup()
        {
            // Initialize
            yield return new WaitUntil(() => WorldTimeManager.Instance.IsInitialized);

            // Check Daily Reward
            string today = WorldTimeManager.Instance.UtcNow.Value.ToShortDateString();
            if (PlayerPrefs.GetString("DAILY_REWARD") != today)
            {
                RewardsMenu.Instance.Clear();
                PremiumManager.Instance.RequestedItem = null;
                PremiumManager.Instance.AccessRandom(4);
                RewardsMenu.Instance.Open(false, LocalizationUtility.Localize("daily-reward_title"));

                PlayerPrefs.SetString("DAILY_REWARD", today);

                yield return new WaitUntil(() => !RewardsMenu.Instance.IsOpen);
            }
        }
        #endregion
    }
}