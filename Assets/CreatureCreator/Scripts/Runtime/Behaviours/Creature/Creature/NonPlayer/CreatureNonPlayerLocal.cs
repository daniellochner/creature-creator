// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

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