// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Drop")]
    public class Drop : Ability
    {
        public override bool CanPerform => base.CanPerform && Player.Instance.Holder.IsHolding.Value;

        public override void OnPerform()
        {
            Player.Instance.Holder.DropAll();
        }
    }
}