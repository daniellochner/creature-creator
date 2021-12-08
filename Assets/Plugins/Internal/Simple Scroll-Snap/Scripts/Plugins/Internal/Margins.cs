using System;

namespace DanielLochner.Assets
{
    [Serializable]
    public class Margins
    {
        public float Left, Right, Top, Bottom;

        public Margins(float m)
        {
            Left = Right = Top = Bottom = m;
        }
        public Margins(float x, float y)
        {
            Left = Right = x;
            Top = Bottom = y;
        }
        public Margins(float l, float r, float t, float b)
        {
            Left = l;
            Right = r;
            Top = t;
            Bottom = b;
        }
    }
}