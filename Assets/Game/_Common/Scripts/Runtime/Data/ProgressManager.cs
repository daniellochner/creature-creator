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
            UnlockMap(Map.Island);
        }

        public override void Revert()
        {
            base.Revert();

#if USE_STATS
            StatsManager.Instance.Revert();
#endif

            UnlockMap(Map.Island);
        }

        public bool UnlockMap(Map map)
        {
            if (!IsMapUnlocked(map))
            {
                PlayerPrefs.SetInt(MapId(map), 1);
                return true;
            }
            return false;
        }
        public bool IsMapUnlocked(Map map)
        {
            return PlayerPrefs.GetInt(MapId(map)) == 1;
        }

        private string MapId(Map map)
        {
            return $"map_unlocked_{map}".ToLower();
        }
        #endregion
    }
}