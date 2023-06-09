using UnityEngine;

#if UNITY_STANDALONE
using Steamworks;
#endif

namespace DanielLochner.Assets.CreatureCreator
{
    public class StatsManager : MonoBehaviourSingleton<StatsManager>
    {
        #region Fields
        [SerializeField, Button("Revert", 25, true)] private bool revert;
        #endregion

        #region Properties
        public int UnlockedBodyParts
        {
            get => GetStat("STA_UNLOCKED_BODY_PARTS", ProgressManager.Data.UnlockedBodyParts.Count);
            set => SetStat("STA_UNLOCKED_BODY_PARTS", value, new StatAchievement("ACH_TAXIDERMIST", 117));
        }
        public int UnlockedPatterns
        {
            get => GetStat("STA_UNLOCKED_PATTERNS1", ProgressManager.Data.UnlockedPatterns.Count);
            set => SetStat("STA_UNLOCKED_PATTERNS1", value, new StatAchievement("ACH_PAINTER", 49));
        }
        public int DistanceTravelled
        {
            get => GetStat("STA_DISTANCE_TRAVELLED", 0);
            set => SetStat("STA_DISTANCE_TRAVELLED", value, new StatAchievement("ACH_GO_THE_DISTANCE", 10000));
        }
        public int Deaths
        {
            get => GetStat("STA_DEATHS", 0);
            set => SetStat("STA_DEATHS", value, new StatAchievement("ACH_A_PART_OF_LIFE", 10));
        }
        public int CashSpent
        {
            get => GetStat("STA_CASH_SPENT", 0);
            set => SetStat("STA_CASH_SPENT", value, new StatAchievement("ACH_BIG_TIME_SPENDER", 10000));
        }
        public int ReachedPeaks
        {
            get => GetStat("STA_REACHED_PEAKS", 0);
            set => SetStat("STA_REACHED_PEAKS", value, new StatAchievement("ACH_MOUNTAINEER", 4));
        }
        public int CompletedQuests
        {
            get => GetStat("STA_COMPLETED_QUESTS", 0);
            set => SetStat("STA_COMPLETED_QUESTS", value, new StatAchievement("ACH_ON_A_MISSION", 11));
        }
        public int Kills
        {
            get => GetStat("STA_KILLS", 0);
            set => SetStat("STA_KILLS", value, new StatAchievement("ACH_RAMPAGE", 100));
        }
        public int CompletedBattles
        {
            get => GetStat("STA_COMPLETED_BATTLES", 0);
            set => SetStat("STA_COMPLETED_BATTLES", value, new StatAchievement("ACH_GLADIATOR", 10));
        }
        public int MinigamesWon
        {
            get => GetStat("STA_MINIGAMES_WON", 0);
            set => SetStat("STA_MINIGAMES_WON", value);
        }
        public int MinigamesCompleted
        {
            get => GetStat("STA_MINIGAMES_COMPLETED", 0);
            set => SetStat("STA_MINIGAMES_COMPLETED", value, new StatAchievement("ACH_MINIGAMER", 10));
        }

        public int NumAchievementsUnlocked
        {
            get
            {
                int counter = 0;
                foreach (string achievementId in DatabaseManager.GetDatabase("Achievements").Objects.Keys)
                {
                    if (IsAchievementUnlocked(achievementId))
                    {
                        counter++;
                    }
                }
                return counter;
            }
        }

        private bool Initialized => AuthenticationManager.Instance.Status == AuthenticationManager.AuthStatus.Success;
        #endregion

        #region Methods
        public void Revert(bool achievementsToo = false)
        {
            if (Initialized)
            {
#if UNITY_STANDALONE
                SteamUserStats.ResetAllStats(achievementsToo);
#elif UNITY_IOS || UNITY_ANDROID
#endif
            }
        }

        public int GetStat(string statId, int defaultValue = 0)
        {
            int value = 0;
            if (Initialized)
            {
#if UNITY_STANDALONE
                if (!SteamUserStats.GetStat(statId, out value))
                {
                    value = defaultValue;
                }
#elif UNITY_IOS || UNITY_ANDROID
                value = PlayerPrefs.GetInt(statId);
#endif
            }
            return value;
        }
        public void SetStat(string statId, int value, params StatAchievement[] statAchievements)
        {
            if (Initialized)
            {
#if UNITY_STANDALONE
                SteamUserStats.SetStat(statId, value);
                SteamUserStats.StoreStats();
#elif UNITY_IOS || UNITY_ANDROID
                PlayerPrefs.SetInt(statId, value);
#endif
            }

            foreach (StatAchievement statAchievement in statAchievements)
            {
                if (!IsAchievementUnlocked(statAchievement.achievementId) && value >= statAchievement.target)
                {
                    UnlockAchievement(statAchievement.achievementId);
                }
            }
        }

        public Achievement GetAchievement(string achievementId)
        {
            return DatabaseManager.GetDatabaseEntry<Achievement>("Achievements", achievementId);
        }
        public bool IsAchievementUnlocked(string achievementId)
        {
            if (Initialized)
            {
#if UNITY_STANDALONE
                if (SteamUserStats.GetAchievement(achievementId, out bool achieved))
                {
                    return achieved;
                }
#elif UNITY_IOS || UNITY_ANDROID
                return PlayerPrefs.GetInt(achievementId) == 1;
#endif
            }
            return false;
        }
        public void UnlockAchievement(string achievementId)
        {
            if (Initialized && !IsAchievementUnlocked(achievementId))
            {
#if UNITY_STANDALONE
                SteamUserStats.SetAchievement(achievementId);
                SteamUserStats.StoreStats();
#elif UNITY_IOS || UNITY_ANDROID
                if (GameServices.Instance.IsLoggedIn())
                {
                    GameServices.Instance.SubmitAchievement(GetAchievement(achievementId).gameServicesId);
                }
                PlayerPrefs.SetInt(achievementId, 1);
#endif
            }
        }

        public Leaderboard GetLeaderboard(string leaderboardId)
        {
            return DatabaseManager.GetDatabaseEntry<Leaderboard>("Leaderboards", leaderboardId);
        }
        public void SubmitScore(string leaderboardId, long score)
        {
            if (Initialized)
            {
#if UNITY_STANDALONE
                // TODO: Implement leaderboards for Steam
#elif UNITY_IOS || UNITY_ANDROID
                if (GameServices.Instance.IsLoggedIn())
                {
                    GameServices.Instance.SubmitScore(score, GetLeaderboard(leaderboardId).gameServicesId);
                }
#endif
            }
        }
        #endregion

        #region Nested
        public class StatAchievement
        {
            public string achievementId = null;
            public int target = -1;

            public StatAchievement(string a, int t)
            {
                achievementId = a;
                target = t;
            }
        }
        #endregion
    }
}