using UnityEditor;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class EditorLayoutUtility
    {
        public static void Header(ref bool show, GUIContent content)
        {
            GUIStyle style = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold
            };
            show = EditorGUILayout.Foldout(show, content, true, style);
        }
    }
}