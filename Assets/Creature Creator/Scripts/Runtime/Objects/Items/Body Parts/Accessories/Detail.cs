// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Body Part/Accessory/Detail")]
    public class Detail : Accessory
    {
        public override string PluralForm => "Details";
        public override int BaseComplexity => 1;
    }
}