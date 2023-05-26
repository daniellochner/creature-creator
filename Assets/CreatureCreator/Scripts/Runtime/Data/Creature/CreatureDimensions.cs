// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class CreatureDimensions
    {
        #region Fields
        public float Height;
        public BodyDimensions Body = new BodyDimensions();
        #endregion

        #region Methods
        public void Reset()
        {
            Height = 0f;
            Body.Reset();
        }
        #endregion

        #region Nested
        [Serializable]
        public class BodyDimensions
        {
            public float Width, Height, Length;
            public void Reset()
            {
                Width = Height = Length = 0f;
            }
        }
        #endregion
    }
}