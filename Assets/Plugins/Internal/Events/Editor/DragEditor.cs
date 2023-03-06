// Events
// Copyright (c) Daniel Lochner

using UnityEditor;
using UnityEngine;

namespace DanielLochner.Assets
{
    [CustomEditor(typeof(Drag)), CanEditMultipleObjects]
    public class DragEditor : Editor
    {
        #region Fields
        private Drag drag;
        private SerializedProperty touchOffset, customCollider, draggable, controlDrag, mouseButton, mousePlaneAlignment, localDirection, worldDirection, isBounded, world, boundsShape, boxSize, sphereRadius, cylinderRadius, cylinderHeight, boundsOffset, smoothing, maxDistance, localMovement, resetOnRelease, useOffsetPosition, updatePlaneOnPress, onPress, onRelease, onDrag, onBeginDrag, onEndDrag, dragThreshold;
        private bool showMovement = true, showBounds = true, showOther = true, showEvents = false;
        #endregion

        #region Methods
        private void OnEnable()
        {
            drag = target as Drag;

            // Serialized Properties
            draggable = serializedObject.FindProperty("draggable");
            controlDrag = serializedObject.FindProperty("controlDrag");
            mouseButton = serializedObject.FindProperty("mouseButton");
            mousePlaneAlignment = serializedObject.FindProperty("mousePlaneAlignment");
            localDirection = serializedObject.FindProperty("localDirection");
            worldDirection = serializedObject.FindProperty("worldDirection");
            isBounded = serializedObject.FindProperty("isBounded");
            world = serializedObject.FindProperty("world");
            boundsShape = serializedObject.FindProperty("boundsShape");
            boxSize = serializedObject.FindProperty("boxSize");
            sphereRadius = serializedObject.FindProperty("sphereRadius");
            cylinderRadius = serializedObject.FindProperty("cylinderRadius");
            cylinderHeight = serializedObject.FindProperty("cylinderHeight");
            boundsOffset = serializedObject.FindProperty("boundsOffset");
            smoothing = serializedObject.FindProperty("smoothing");
            maxDistance = serializedObject.FindProperty("maxDistance");
            localMovement = serializedObject.FindProperty("localMovement");
            resetOnRelease = serializedObject.FindProperty("resetOnRelease");
            useOffsetPosition = serializedObject.FindProperty("useOffsetPosition");
            updatePlaneOnPress = serializedObject.FindProperty("updatePlaneOnPress");
            onPress = serializedObject.FindProperty("onPress");
            onRelease = serializedObject.FindProperty("onRelease");
            onDrag = serializedObject.FindProperty("onDrag");
            onBeginDrag = serializedObject.FindProperty("onBeginDrag");
            onEndDrag = serializedObject.FindProperty("onEndDrag");
            dragThreshold = serializedObject.FindProperty("dragThreshold");
            customCollider = serializedObject.FindProperty("customCollider");
            touchOffset = serializedObject.FindProperty("touchOffset");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Movement();
            Bounds();
            Other();
            Events();

            serializedObject.ApplyModifiedProperties();
            PrefabUtility.RecordPrefabInstancePropertyModifications(drag);
        }
        
        private void Movement()
        {
            EditorGUILayout.Space();

            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            showMovement = EditorGUILayout.Foldout(showMovement, "Movement Settings", true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (showMovement)
            {
                EditorGUILayout.PropertyField(mouseButton, new GUIContent("Mouse Button", ""));

                EditorGUILayout.PropertyField(mousePlaneAlignment, new GUIContent("Mouse Plane Alignment", "The plane along which the dragged object is aligned."));

                switch (drag.mousePlaneAlignment)
                {
                    case Drag.MousePlaneAlignment.ToLocalDirection:
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(localDirection, new GUIContent("Local Direction", ""));
                        EditorGUI.indentLevel--;
                        break;
                    case Drag.MousePlaneAlignment.ToWorldDirection:
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(worldDirection, new GUIContent("World Direction", ""));
                        EditorGUI.indentLevel--;
                        break;
                }

                EditorGUILayout.PropertyField(smoothing, new GUIContent("Smoothing", ""));
                EditorGUILayout.PropertyField(maxDistance, new GUIContent("Max Distance", ""));
                EditorGUILayout.PropertyField(localMovement, new GUIContent("Local Movement", ""));
            }

            EditorGUILayout.Space();
        }
        private void Bounds()
        {
            EditorGUILayout.Space();

            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            showBounds = EditorGUILayout.Foldout(showBounds, "Bounds Settings", true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (showBounds)
            {
                EditorGUILayout.PropertyField(isBounded, new GUIContent("Bounded", ""));
                if (drag.isBounded)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.PropertyField(boundsShape, new GUIContent("Shape", ""));
                    EditorGUI.indentLevel++;
                    switch (drag.boundsShape)
                    {
                        case Drag.BoundsShape.Box:
                            EditorGUILayout.PropertyField(boxSize, new GUIContent("Size", ""));
                            break;
                        case Drag.BoundsShape.Cylinder:
                            EditorGUILayout.PropertyField(cylinderRadius, new GUIContent("Radius", ""));
                            EditorGUILayout.PropertyField(cylinderHeight, new GUIContent("Height", ""));
                            break;
                        case Drag.BoundsShape.Sphere:
                            EditorGUILayout.PropertyField(sphereRadius, new GUIContent("Radius", ""));
                            break;
                    }
                    EditorGUI.indentLevel--;

                    EditorGUILayout.PropertyField(boundsOffset, new GUIContent("Offset", ""));

                    EditorGUI.indentLevel--;
                }
            }
        }
        private void Other()
        {
            EditorGUILayout.Space();

            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            showOther = EditorGUILayout.Foldout(showOther, "Other Settings", true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (showOther)
            {
                EditorGUILayout.PropertyField(world, new GUIContent("World", ""));
                EditorGUILayout.PropertyField(draggable, new GUIContent("Draggable", ""));
                EditorGUILayout.PropertyField(controlDrag, new GUIContent("Control Drag", ""));
                EditorGUILayout.PropertyField(resetOnRelease, new GUIContent("Reset On Release", ""));
                EditorGUILayout.PropertyField(useOffsetPosition, new GUIContent("Use Offset Position", ""));
                EditorGUILayout.PropertyField(updatePlaneOnPress, new GUIContent("Update Plane On Press", ""));
                EditorGUILayout.PropertyField(dragThreshold, new GUIContent("Drag Threshold", ""));
                EditorGUILayout.PropertyField(customCollider, new GUIContent("Custom Collider", ""));
                EditorGUILayout.PropertyField(touchOffset, new GUIContent("Touch Offset", ""));
            }
        }
        private void Events()
        {
            EditorGUILayout.Space();

            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            showEvents = EditorGUILayout.Foldout(showEvents, "Events", true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (showEvents)
            {
                EditorGUILayout.PropertyField(onPress, new GUIContent("On Press", ""));
                EditorGUILayout.PropertyField(onRelease, new GUIContent("On Release", ""));
                EditorGUILayout.PropertyField(onBeginDrag, new GUIContent("On Begin Drag", ""));
                EditorGUILayout.PropertyField(onDrag, new GUIContent("On Drag", ""));
                EditorGUILayout.PropertyField(onEndDrag, new GUIContent("On End Drag", ""));
            }
        }
        #endregion
    }
}