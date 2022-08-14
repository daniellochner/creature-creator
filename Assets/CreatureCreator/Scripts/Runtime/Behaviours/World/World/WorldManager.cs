// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class WorldManager : MonoBehaviourSingleton<WorldManager>
    {
        public World World
        {
            get;
            set;
        }
    }
}