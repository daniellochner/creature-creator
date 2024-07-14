using UnityEngine;
using System.Collections.Generic;

#if UNITY_STANDALONE
using Steamworks;
#endif

namespace DanielLochner.Assets.CreatureCreator
{
    public class StatsManager : MonoBehaviourSingleton<StatsManager>
    {
        #region Fields
        [SerializeField, Button("Revert", 25, true)] private bool revert;

        public static readonly int NUM_BODY_PARTS = 135;
        public static readonly int NUM_PATTERNS = 54;
        public static readonly int NUM_QUESTS = 13;
        public static readonly int NUM_MAPS = 5;
        #endregion

        #region Properties
        public int UnlockedBodyParts
        {
            get => GetStat("STA_UNLOCKED_BODY_PARTS", ProgressManager.Data.UnlockedBodyParts.Count);
            set => SetStat("STA_UNLOCKED_BODY_PARTS", value, new StatAchievement("ACH_TAXIDERMIST", NUM_BODY_PARTS));
        }
        public int UnlockedPatterns
        {
            get => GetStat("STA_UNLOCKED_PATTERNS", ProgressManager.Data.UnlockedPatterns.Count);
            set => SetStat("STA_UNLOCKED_PATTERNS", value, new StatAchievement("ACH_PAINTER", NUM_PATTERNS));
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
        public int ExperienceEarned
        {
            get => GetStat("STA_EXPERIENCE_EARNED", 0);
            set => SetStat("STA_EXPERIENCE_EARNED", value, new StatAchievement("ACH_EXPERIENCED_CREATOR", 10));
        }
        public int ReachedPeaks
        {
            get => GetStat("STA_REACHED_PEAKS", 0);
            set => SetStat("STA_REACHED_PEAKS", value, new StatAchievement("ACH_MOUNTAINEER", NUM_MAPS));
        }
        public int CompletedQuests
        {
            get => GetStat("STA_COMPLETED_QUESTS", 0);
            set => SetStat("STA_COMPLETED_QUESTS", value, new StatAchievement("ACH_ON_A_MISSION", NUM_QUESTS));
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
        public void Revert()
        {
            if (Initialized)
            {
                UnlockedBodyParts = 0;
                UnlockedPatterns = 0;
                DistanceTravelled = 0;
                Deaths = 0;
                ExperienceEarned = 0;
                ReachedPeaks = 0;
                CompletedQuests = 0;
                Kills = 0;
                CompletedBattles = 0;
                MinigamesWon = 0;
                MinigamesCompleted = 0;
            }
        }

        public int GetStat(string statId, int defaultValue = 0)
        {
            int value = 0;
            if (Initialized)
            {
#if UNITY_STANDALONE
                if (EducationManager.Instance.IsEducational)
                {
                    value = PlayerPrefs.GetInt(statId);
                }
                else
                {
                    if (!SteamUserStats.GetStat(statId, out value))
                    {
                        value = defaultValue;
                    }
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
                if (EducationManager.Instance.IsEducational)
                {
                    PlayerPrefs.SetInt(statId, value);
                }
                else
                {
                    SteamUserStats.SetStat(statId, value);
                    SteamUserStats.StoreStats();
                }
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
            var achievement = GetAchievement(achievementId);
            if (achievement != null && achievement.legacyIds != null)
            {
                foreach (string id in achievement.legacyIds)
                {
                    if (IsAchievementUnlockedInternal(id))
                    {
                        return true;
                    }
                }
            }
            return IsAchievementUnlockedInternal(achievementId);
        }
        private bool IsAchievementUnlockedInternal(string achievementId)
        {
            if (Initialized)
            {
#if UNITY_STANDALONE
                if (EducationManager.Instance.IsEducational)
                {
                    return PlayerPrefs.GetInt(achievementId) == 1;
                }
                else
                {
                    if (SteamUserStats.GetAchievement(achievementId, out bool achieved))
                    {
                        return achieved;
                    }
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
                if (EducationManager.Instance.IsEducational)
                {
                    PlayerPrefs.SetInt(achievementId, 1);
                }
                else
                {
                    SteamUserStats.SetAchievement(achievementId);
                    SteamUserStats.StoreStats();
                }
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
                // TODO: implement Steam leaderboards
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