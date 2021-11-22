// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Body Part/Limb/Arm")]
    public class Arm : Limb
    {
        public override string PluralForm => "Arms";
    }
}