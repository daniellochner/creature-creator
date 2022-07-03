// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class CreatureDimensions
    {
        public float height;
        public float radius;
        public BodyDimensions body;

        [Serializable]
        public class BodyDimensions
        {
            public float length;
            public float radius;
        }
    }
}