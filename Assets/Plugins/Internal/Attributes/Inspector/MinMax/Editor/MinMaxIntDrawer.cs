using UnityEditor;
using UnityEngine;

namespace DanielLochner.Assets
{
    [CustomPropertyDrawer(typeof(MinMaxInt))]
    public class MinMaxIntDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty minProperty = property.FindPropertyRelative("min");
            SerializedProperty maxProperty = property.FindPropertyRelative("max");

            // Label
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Min to Max
            Rect left = new Rect(position.x, position.y, position.width / 2 - 11f, position.height);
            Rect right = new Rect(position.x + position.width - left.width, position.y, left.width, position.height);
            Rect mid = new Rect(left.xMax, position.y, 22, position.height);

            int minValue = minProperty.intValue;
            int maxValue = maxProperty.intValue;

            minValue = EditorGUI.IntField(left, minValue);
            EditorGUI.LabelField(mid, " → ");
            maxValue = EditorGUI.IntField(right, maxValue);

            minProperty.floatValue = minValue;
            maxProperty.floatValue = maxValue;

            EditorGUI.EndProperty();
        }
    }
}