// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureNonPlayerLocal : CreatureNonPlayer
    {
        public override void Setup()
        {
            base.Setup();
            Spawner.Spawn();
        }
    }
}