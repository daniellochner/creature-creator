using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace DanielLochner.Assets
{
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (attribute as ButtonAttribute).buttonHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            ButtonAttribute buttonAttribute = attribute as ButtonAttribute;

            if (GUI.Button(position, label) && !string.IsNullOrEmpty(buttonAttribute.methodName))
            {
                object targetObject = serializedProperty.serializedObject.targetObject;
                MethodInfo method = targetObject.GetType().GetMethod(buttonAttribute.methodName);

                method.Invoke(targetObject, buttonAttribute.methodArguments);
            }
        }
    }
}