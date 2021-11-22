// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class MouthConstructor : BodyPartConstructor
    {
        public override void Add()
        {
            base.Add();
            CreatureConstructor.Mouths.Add(this);
        }
        public override void Remove()
        {
            base.Remove();
            CreatureConstructor.Mouths.Remove(this);
        }
    }
}