// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Body Part/Accessory/Wing")]
    public class Wing : Accessory
    {
        public override string PluralForm => "Wings";
        public override int BaseComplexity => 5;
    }
}