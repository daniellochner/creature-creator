// Simple Zoom - https://assetstore.unity.com/packages/tools/gui/simple-zoom-143625
// Copyright (c) Daniel Lochner

using UnityEditor;

namespace DanielLochner.Assets.SimpleZoom
{
    [CustomEditor(typeof(object), true)]
    [CanEditMultipleObjects]
    public class SZCopyrightEditor : CopyrightEditor
    {
        public override string Product => "Simple Zoom";
        public override string CopyrightHolder => "Daniel Lochner";
    }
}