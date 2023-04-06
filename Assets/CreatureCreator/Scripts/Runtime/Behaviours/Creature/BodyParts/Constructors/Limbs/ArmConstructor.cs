// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ArmConstructor : LimbConstructor
    {
        public HandConstructor ConnectedHand => ConnectedExtremity as HandConstructor;
    }
}