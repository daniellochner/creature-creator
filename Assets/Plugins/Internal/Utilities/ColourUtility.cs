using UnityEngine;

namespace DanielLochner.Assets
{
    public static class ColourUtility
    {
        public static float GetColourDistance(Color c1, Color c2)
        {
            Vector3 v1 = new Vector3(c1.r, c1.g, c1.b);
            Vector3 v2 = new Vector3(c2.r, c2.g, c2.b);
            return Vector3.Distance(v1, v2);
        }
    }
}