// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Roar")]
    public class Roar : Ability
    {
        [SerializeField] private PlayerEffects.Sound[] roarSounds;

        public override void OnPerform()
        {
            CreatureAbilities.GetComponent<PlayerEffects>().PlaySound(roarSounds);
        }
    }
}