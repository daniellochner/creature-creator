using UnityEditor;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class DrawIfDrawerBase<A> : PropertyDrawer where A : DrawIfAttributeBase
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (CanDraw(property))
            {
                A drawIf = attribute as A;

                if (drawIf.indent) EditorGUI.indentLevel++;
                if (drawIf.readOnly) GUI.enabled = false;

                EditorGUI.PropertyField(position, property, label, true);

                if (drawIf.indent) EditorGUI.indentLevel--;
                if (drawIf.readOnly) GUI.enabled = true;
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (CanDraw(property))
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }

        protected virtual bool CanDraw(SerializedProperty property)
        {
            A drawIfAttribute = attribute as A;

            SerializedProperty comparedProperty = property.serializedObject.FindProperty(drawIfAttribute.propertyName);
            switch (comparedProperty.type)
            {
                case "bool":
                    return comparedProperty.boolValue == (bool)drawIfAttribute.propertyValue;
                case "Enum":
                    return comparedProperty.enumValueIndex == (int)drawIfAttribute.propertyValue;
                default:
                    return comparedProperty.objectReferenceValue == (Object)drawIfAttribute.propertyValue;
            }
        }
    }
}