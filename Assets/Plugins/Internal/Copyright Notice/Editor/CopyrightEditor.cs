// Copyright Notice
// Copyright (c) Daniel Lochner

using System;
using UnityEditor;
using UnityEngine;

namespace DanielLochner.Assets
{
    /// <summary>
    /// Derive from to display a copyright notice above scripts in the inspector.
    /// Relevant scripts must be in the same namespace as the derived editor.
    /// </summary>
    public abstract class CopyrightEditor : Editor
    {
        public abstract string Product { get; }
        public abstract string CopyrightHolder { get; }

        public override void OnInspectorGUI()
        {
            ShowCopyrightNotice();
            base.OnInspectorGUI();
        }
        public void ShowCopyrightNotice()
        {
            if (GetType().Namespace != null)
            {
                Type type = target.GetType();
                if (type.Namespace != null && type.Namespace.Equals(GetType().Namespace))
                {
                    GUILayout.BeginVertical("HelpBox");
                    GUILayout.Space(10);
                    GUILayout.Label(type.Name.SplitCamelCase(), new GUIStyle() { fontSize = 28, alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold });
                    GUILayout.Label(Product, new GUIStyle() { fontSize = 14, alignment = TextAnchor.MiddleCenter });
                    GUILayout.Label("Copyright © " + CopyrightHolder, new GUIStyle() { fontSize = 14, alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Italic });
                    GUILayout.Space(10);
                    GUILayout.EndVertical();
                }
            }
        }
    }
}