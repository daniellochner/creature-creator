using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Achievement")]
    public class Achievement : ScriptableObject
    {
        public AchievementNames gameServicesId;
        public new string name;
        public string description;
        public Sprite unlockedIcon;
        public Sprite lockedIcon;
        public string[] legacyIds;
    }
}