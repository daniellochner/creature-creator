using Steamworks;
using System.Collections;
using UnityEngine;

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
            get => GetStat("STA_UNLOCKED_BODY_PARTS");
            set => SetStat("STA_UNLOCKED_BODY_PARTS", value, new StatAchievement("ACH_TAXIDERMIST", 103));
        }
        public int UnlockedPatterns
        {
            get => GetStat("STA_UNLOCKED_PATTERNS");
            set => SetStat("STA_UNLOCKED_PATTERNS", value, new StatAchievement("ACH_PAINTER", 37));
        }
        public int DistanceTravelled
        {
            get => GetStat("STA_DISTANCE_TRAVELLED");
            set => SetStat("STA_DISTANCE_TRAVELLED", value, new StatAchievement("ACH_GO_THE_DISTANCE", 10000));
        }
        public int Deaths
        {
            get => GetStat("STA_DEATHS");
            set => SetStat("STA_DEATHS", value, new StatAchievement("ACH_A_PART_OF_LIFE", 10));
        }
        public int CashSpent
        {
            get => GetStat("STA_CASH_SPENT");
            set => SetStat("STA_CASH_SPENT", value, new StatAchievement("ACH_BIG_TIME_SPENDER", 100000));
        }
        public int ReachedPeaks
        {
            get => GetStat("STA_REACHED_PEAKS");
            set => SetStat("STA_REACHED_PEAKS", value, new StatAchievement("ACH_MOUNTAINEER", 3));
        }
        public int CompletedQuests
        {
            get => GetStat("STA_COMPLETED_QUESTS");
            set => SetStat("STA_COMPLETED_QUESTS", value, new StatAchievement("ACH_ON_A_MISSION", 12));
        }
        public int Kills
        {
            get => GetStat("STA_KILLS");
            set => SetStat("STA_KILLS", value, new StatAchievement("ACH_RAMPAGE", 100));
        }

        public int NumAchievementsUnlocked
        {
            get
            {
                int counter = 0;
                foreach (string achievementId in DatabaseManager.GetDatabase("Achievements").Objects.Keys)
                {
                    SteamUserStats.GetAchievement(achievementId, out bool achieved);
                    if (achieved)
                    {
                        counter++;
                    }
                }
                return counter;
            }
        }
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => SteamManager.Initialized);
            SteamUserStats.StoreStats();

            // Necessary initial synch with the number of parts and patterns (if you have already unlocked before).
            UnlockedBodyParts = ProgressManager.Data.UnlockedBodyParts.Count;
            UnlockedPatterns  = ProgressManager.Data.UnlockedPatterns.Count;
        }

        public void Revert(bool achievementsToo = false)
        {
            if (SteamManager.Initialized)
            {
                SteamUserStats.ResetAllStats(achievementsToo);
            }
        }

        public int GetStat(string statId)
        {
            int intValue = 0;
            if (SteamManager.Initialized)
            {
                SteamUserStats.GetStat(statId, out intValue);
            }
            return intValue;
        }
        public void SetStat(string statId, int value, params StatAchievement[] statAchievements)
        {
            if (SteamManager.Initialized)
            {
                SteamUserStats.SetStat(statId, value);

                if (statAchievements.Length > 0)
                {
                    foreach (StatAchievement statAchievement in statAchievements)
                    {
                        if (statAchievement.achievementId != null)
                        {
                            SteamUserStats.GetAchievement(statAchievement.achievementId, out bool achieved);
                            if (!achieved && value >= statAchievement.target)
                            {
                                SteamUserStats.SetAchievement(statAchievement.achievementId);
                            }
                        }
                    }
                }

                SteamUserStats.StoreStats();
            }
        }
        
        public bool GetAchievement(string achievementId)
        {
            if (SteamManager.Initialized && SteamUserStats.GetAchievement(achievementId, out bool achieved))
            {
                return achieved;
            }
            return false;
        }
        public void SetAchievement(string achievementId)
        {
            if (SteamManager.Initialized)
            {
                SteamUserStats.GetAchievement(achievementId, out bool achieved);
                if (!achieved)
                {
                    SteamUserStats.SetAchievement(achievementId);
                    SteamUserStats.StoreStats();
                }
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