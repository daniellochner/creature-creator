using UnityEngine;

namespace DanielLochner.Assets
{
    public static class MathfUtility
    {
        public static float QuadraticEquation(float a, float b, float c, int d)
        {
            return (-b + d * Mathf.Sqrt(Mathf.Pow(b, 2) - 4 * a * c)) / (2 * a);
        }
    }
}