// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Body Part/Accessory/Tail")]
    public class Tail : Accessory
    {
        public override string PluralForm => "Tails";
        public override int BaseComplexity => 5;
    }
}