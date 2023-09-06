// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

#if UNITY_STANDALONE
using Steamworks;
#endif

namespace DanielLochner.Assets.CreatureCreator
{
    public class ProgressManager : DataManager<ProgressManager, Progress>
    {
        #region Properties
        public override string SALT
        {
            get
            {
#if UNITY_STANDALONE
                if (EducationManager.Instance.IsEducational)
                {
                    return SystemInfo.deviceUniqueIdentifier;
                }
                else
                {
                    return SteamUser.GetSteamID().ToString();
                }
#elif UNITY_IOS || UNITY_ANDROID
                return SystemInfo.deviceUniqueIdentifier;
#endif
            }
        }
        #endregion

        #region Methods
        protected override void Start()
        {
            base.Start();
            MigrateLegacy();
        }

        public void UnlockMap(Map map)
        {
            if (!IsMapUnlocked(map))
            {
                Data.UnlockedMaps.Add(map);
                Save();
            }
        }
        public bool IsMapUnlocked(Map map)
        {
            return Data.UnlockedMaps.Contains(map);
        }

        #region Legacy
        private static readonly Map[] MAPS = new Map[]
        {
            Map.Island,
            Map.Farm,
            Map.Sandbox,
            Map.Cave,
            Map.City
        };
        private static readonly string[] QUESTS = new string[]
        {
            // Island
            "27dh3g2",

            // Farm
            "9n5pdf6",
            "j5pz7s0",
            "8s7s83i",
            "lo4zz8f",
            "01lfpx7",
            "mn72a0b",
            "f8s5x02",

            // Sandbox
            "8nsgy3m",
            "9js6hk4",

            // Cave
            "k2nx0l",

            // City
            "fkfnwa",
            "s9f2ln"
        };

        private void MigrateLegacy()
        {
            foreach (string questId in QUESTS)
            {
                if (PlayerPrefs.GetInt($"quest_{questId}") == 1 && !Data.CompletedQuests.Contains(questId))
                {
                    Data.CompletedQuests.Add(questId);
                }
            }

            foreach (Map map in MAPS)
            {
                if (PlayerPrefs.GetInt($"map_unlocked_{map}".ToLower()) == 1 && !Data.UnlockedMaps.Contains(map))
                {
                    Data.UnlockedMaps.Add(map);
                }
            }

            foreach (Map map in MAPS)
            {
                if (PlayerPrefs.GetInt($"HP_{map}".ToUpper()) == 1 && !Data.ReachedPeaks.Contains(map))
                {
                    Data.ReachedPeaks.Add(map);
                }
            }

            Save();
        }
        #endregion
        #endregion
    }
}