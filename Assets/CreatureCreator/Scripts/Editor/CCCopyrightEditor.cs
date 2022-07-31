// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEditor;

namespace DanielLochner.Assets.CreatureCreator
{
    [CustomEditor(typeof(object), true)]
    [CanEditMultipleObjects]
    public class CCCopyrightEditor : CopyrightEditor
    {
        public override string Product => "Creature Creator";
        public override string CopyrightHolder => "Daniel Lochner";
    }
}