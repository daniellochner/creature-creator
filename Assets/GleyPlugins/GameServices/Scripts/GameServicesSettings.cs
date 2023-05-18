namespace GleyGameServices
{
    using System.Collections.Generic;
    using UnityEngine;

    //used to set up achievements and leaderboards properties
    public class GameServicesSettings : ScriptableObject
    {
        public bool useForAndroid;
        public bool useForIos;
        public bool usePlaymaker;
        public string googleAppId;
        public List<Achievement> allGameAchievements = new List<Achievement>();
        public List<Leaderboard> allGameLeaderboards = new List<Leaderboard>();
        public bool useGameFlow;
        public bool useBolt;
    }
}
