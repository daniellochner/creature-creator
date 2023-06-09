using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Leaderboard")]
    public class Leaderboard : ScriptableObject
    {
        public LeaderboardNames gameServicesId;
        public new string name;
    }
}