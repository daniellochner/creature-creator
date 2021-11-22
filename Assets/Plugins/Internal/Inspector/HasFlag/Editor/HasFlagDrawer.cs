using UnityEditor;
using UnityEngine;

namespace DanielLochner.Assets
{
    [CustomPropertyDrawer(typeof(HasFlagAttribute))]
    public class HasFlagDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (HasFlag(property))
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(position, property, label, true);
                EditorGUI.indentLevel--;
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (HasFlag(property))
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }

        private bool HasFlag(SerializedProperty property)
        {
            HasFlagAttribute hasFlagAttribute = attribute as HasFlagAttribute;

            int enumFieldValue = property.serializedObject.FindProperty(hasFlagAttribute.enumPropertyName).intValue;
            int enumFlagValue = (int)hasFlagAttribute.enumFlagValue;

            return (enumFieldValue & enumFlagValue) != 0;
        }
    }
}