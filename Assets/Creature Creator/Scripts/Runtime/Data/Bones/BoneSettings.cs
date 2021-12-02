// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class BoneSettings
    {
        #region Fields
        [SerializeField] private float radius = 0.05f;
        [SerializeField] private float length = 0.1f;
        [SerializeField] [Range(4, 25)] private int segments = 14;
        [SerializeField] [Range(2, 25)] private int rings = 4;
        #endregion

        #region Properties
        public float Radius
        {
            get => radius;
            set => radius = value;
        }
        public float Length
        {
            get => length;
            set => length = value;
        }
        public int Segments
        {
            get => segments;
            set => segments = value;
        }
        public int Rings
        {
            get => rings;
            set => rings = value;
        }
        #endregion
    }
}