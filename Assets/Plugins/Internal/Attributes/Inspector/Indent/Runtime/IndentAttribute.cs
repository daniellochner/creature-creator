using UnityEngine;

namespace DanielLochner.Assets
{
    public class IndentAttribute : PropertyAttribute
    {
        public int NumLevels { get; set; }
        public IndentAttribute(int numLevels = 1)
        {
            NumLevels = numLevels;
        }
    }
}