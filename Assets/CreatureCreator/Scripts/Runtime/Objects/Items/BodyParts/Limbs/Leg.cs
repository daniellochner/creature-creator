// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Body Part/Limb/Leg")]
    public class Leg : Limb
    {
        #region Properties
        public override string PluralForm => "Legs";
        #endregion
    }
}