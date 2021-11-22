// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Body Part/Accessory/Weapon")]
    public class Weapon : Accessory
    {
        public override string PluralForm => "Weapons";
        public override int BaseComplexity => 3;
    }
}