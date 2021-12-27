// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
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