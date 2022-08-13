// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class CreatureDimensions
    {
        public float height = 0f;
        public float radius = 0f;
        public BodyDimensions body = new BodyDimensions();

        public void Reset()
        {
            height = 0f;
            radius = 0f;
            body.Reset();
        }

        #region Nested
        [Serializable]
        public class BodyDimensions
        {
            public float length = 0f;
            public float radius = 0f;

            public void Reset()
            {
                length = 0f;
                radius = 0f;
            }
        }
        #endregion
    }
}