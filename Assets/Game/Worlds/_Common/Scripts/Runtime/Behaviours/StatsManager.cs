using Steamworks;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class StatsManager : MonoBehaviour
    {
        #region Properties
        public int UnlockedBodyParts
        {
            get => GetInt("STA_UNLOCKED_BODY_PARTS");
            set => SetInt("STA_UNLOCKED_BODY_PARTS", value);
        }
        public int UnlockedPatterns
        {
            get => GetInt("STA_UNLOCKED_PATTERNS");
            set => SetInt("STA_UNLOCKED_PATTERNS", value);
        }
        public int DistanceTravelled
        {
            get => GetInt("STA_DISTANCE_TRAVELLED");
            set => SetInt("STA_DISTANCE_TRAVELLED", value);
        }
        public int Deaths
        {
            get => GetInt("STA_DEATHS");
            set => SetInt("STA_DEATHS", value);
        }
        public int CashSpent
        {
            get => GetInt("STA_CASH_SPENT");
            set => SetInt("STA_CASH_SPENT", value);
        }
        public int ReachedPeaks
        {
            get => GetInt("STA_REACHED_PEAKS");
            set => SetInt("STA_REACHED_PEAKS", value);
        }
        public int CompletedQuests
        {
            get => GetInt("STA_COMPLETED_QUESTS");
            set => SetInt("STA_COMPLETED_QUESTS", value);
        }
        public int Kills
        {
            get => GetInt("STA_KILLS");
            set => SetInt("STA_KILLS", value);
        }
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => SteamManager.Initialized);
            SteamUserStats.StoreStats();
        }

        public int GetInt(string id)
        {
            int intValue = 0;
            if (SteamManager.Initialized)
            {
                SteamUserStats.GetStat(id, out intValue);
            }
            return intValue;
        }
        public void SetInt(string id, int intValue)
        {
            if (SteamManager.Initialized)
            {
                SteamUserStats.SetStat(id, intValue);
            }
        }

        public float GetFloat(string id)
        {
            float floatValue = 0;
            if (SteamManager.Initialized)
            {
                SteamUserStats.GetStat(id, out floatValue);
            }
            return floatValue;
        }
        public void SetFloat(string id, float value)
        {
            if (SteamManager.Initialized)
            {
                SteamUserStats.SetStat(id, value);
                SteamUserStats.StoreStats();
            }
        }
        #endregion
    }
}