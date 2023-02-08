// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Steamworks;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ProgressManager : DataManager<ProgressManager, Progress>
    {
        #region Properties
        public override string SALT => SteamUser.GetSteamID().ToString();
        #endregion

        #region Methods
        protected override void Start()
        {
            base.Start();
            UnlockMap(Map.Island);
            
            // TODO: Quick fix for if existing player... unlock all maps
            if (PlayerPrefs.GetInt("REVERT_SETTINGS") == 1)
            {
                UnlockMap(Map.Farm);
                UnlockMap(Map.Sandbox);
            }
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