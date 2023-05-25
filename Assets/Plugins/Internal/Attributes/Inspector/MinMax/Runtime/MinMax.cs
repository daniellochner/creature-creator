using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    [Serializable]
    public struct MinMax
    {
        public float min;
        public float max;

        public float Random
        {
            get => UnityEngine.Random.Range(min, max);
        }
        public float Average
        {
            get => (min + max) / 2f;
        }
        public float Range
        {
            get => max - min;
        }

        public MinMax(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
        public float Clamp(float value)
        {
            return Mathf.Clamp(value, min, max);
        }
    }
}