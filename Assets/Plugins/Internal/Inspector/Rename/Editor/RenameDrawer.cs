using UnityEngine;
using UnityEditor;

namespace DanielLochner.Assets
{
    [CustomPropertyDrawer(typeof(RenameAttribute))]
    public class RenameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, new GUIContent((attribute as RenameAttribute).NewName));
        }
    }
}