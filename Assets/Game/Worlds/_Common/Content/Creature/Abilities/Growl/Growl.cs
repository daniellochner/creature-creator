// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator.Abilities
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/Growl")]
    public class Growl : Ability
    {
        [SerializeField] private PlayerEffects.Sound[] growlSounds;

        public override void OnPerform()
        {
            Player.Instance.Effects.PlaySound(growlSounds);
        }
    }
}