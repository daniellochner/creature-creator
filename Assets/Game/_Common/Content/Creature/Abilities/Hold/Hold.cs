// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Hold")]
    public class Hold : Ability
    {
        [SerializeField] private float radius = 3f;

        public override void OnPerform()
        {
            base.OnPerform();

            var holder = Player.Instance.Holder;
            foreach (var collider in Physics.OverlapSphere(holder.transform.position, radius))
            {
                if (collider.TryGetComponent(out Holdable holdable))
                {
                    holder.TryHold(holdable.Held);
                }
            }
        }
    }
}