using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Placed Unlockables")]
    public class PlacedUnlockables : ScriptableObject
    {
        public List<string> bodyparts = new List<string>();
        public List<string> patterns = new List<string>();
    }
}