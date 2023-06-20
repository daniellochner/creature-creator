// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class ArmAnimator : LimbAnimator
    {
        public ArmConstructor ArmConstructor => LimbConstructor as ArmConstructor;
    }
}