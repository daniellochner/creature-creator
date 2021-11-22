using UnityEngine;
using UnityEditor;

namespace DanielLochner.Assets
{
    [CustomPropertyDrawer(typeof(IndentAttribute))]
    public class IndentDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            IndentAttribute indentAttribute = attribute as IndentAttribute;

            EditorGUI.indentLevel += indentAttribute.NumLevels;
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.indentLevel -= indentAttribute.NumLevels;
        }
    }
}