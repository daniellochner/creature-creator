using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Seal")]
    public class Seal : Dance
    {
        public override bool CanPerform => base.CanPerform && Player.Instance.Underwater.IsUnderwater;
    }
}