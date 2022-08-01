// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Bite")]
    public class Bite : Ability
    {
        [SerializeField] private PlayerEffects.Sound[] biteSounds;

        public override void OnPerform()
        {
            CreatureAbilities.GetComponent<PlayerEffects>().PlaySound(biteSounds);
        }
    }
}