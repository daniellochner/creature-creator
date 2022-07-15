// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Quality Settings")]
    public class BoneSettings : ScriptableObject
    {
        #region Fields
        [SerializeField] private float radius = 0.1f;
        [SerializeField] private float length = 0.2f;
        [SerializeField] [Range(4, 25)] private int segments = 20;
        [SerializeField] [Range(2, 25)] private int rings = 8;
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