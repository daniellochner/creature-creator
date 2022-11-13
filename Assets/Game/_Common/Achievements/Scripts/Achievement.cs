using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Achievement")]
    public class Achievement : ScriptableObject
    {
        public string id;
        public string description;
        public Sprite unlockedIcon;
        public Sprite lockedIcon;
    }
}