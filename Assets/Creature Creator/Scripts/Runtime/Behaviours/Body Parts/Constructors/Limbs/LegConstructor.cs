// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class LegConstructor : LimbConstructor
    {
        private void LateUpdate()
        {
            Extremity.rotation = CreatureConstructor.Body.rotation;
        }

        public override void Add()
        {
            base.Add();
            CreatureConstructor.Legs.Add(this);
        }
        public override void Remove()
        {
            base.Remove();
            CreatureConstructor.Legs.Remove(this);
        }
    }
}