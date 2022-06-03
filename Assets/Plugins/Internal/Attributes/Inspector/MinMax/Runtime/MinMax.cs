using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    [Serializable]
    public struct MinMax
    {
        public float min;
        public float max;

        public MinMax(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float Random
        {
            get => UnityEngine.Random.Range(min, max);
        }

        public float Clamp(float value)
        {
            return Mathf.Clamp(value, min, max);
        }
    }
}