// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    [System.Flags]
    public enum Transformation
    {
        Pivot = 1 << 0,
        PivotXY = 1 << 1,
        Rotate = 1 << 2,
        Scale = 1 << 3,
        StretchX = 1 << 4,
        StretchY = 1 << 5,
        StretchZ = 1 << 6
    }
}