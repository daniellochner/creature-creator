using System;

namespace DanielLochner.Assets
{
    [Serializable]
    public struct MinMaxInt
    {
        public int min;
        public int max;

        public MinMaxInt(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public int Random
        {
            get => UnityEngine.Random.Range(min, max);
        }
    }
}