// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class ArmConstructor : LimbConstructor
    {
        public override void Add()
        {
            base.Add();
            CreatureConstructor.Arms.Add(this);
        }
        public override void Remove()
        {
            base.Remove();
            CreatureConstructor.Arms.Remove(this);
        }
    }
}