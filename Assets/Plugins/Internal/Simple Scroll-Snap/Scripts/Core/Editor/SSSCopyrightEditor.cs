// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEditor;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    [CustomEditor(typeof(object), true)]
    [CanEditMultipleObjects]
    public class SSSCopyrightEditor : CopyrightEditor
    {
        public override string Product => "Simple Scroll-Snap";
        public override string CopyrightHolder => "Daniel Lochner";
    }
}