// Simple Zoom - https://assetstore.unity.com/packages/tools/gui/simple-zoom-143625
// Copyright (c) Daniel Lochner

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.SimpleZoom
{
    [CustomEditor(typeof(SimpleZoom))]
    public class SimpleZoomEditor : SZCopyrightEditor
    {
        #region Fields
        private bool showBasicSettings = true, showControlSettings = true, showPlatformSpecificSettings = true, showMobile = false, showDesktop = false, showScrollWheel = false;
        private SerializedProperty defaultZoom, minMaxZoom, zoomTarget, customPosition, zoomType, elasticLimit, elasticDamping, zoomMode, zoomInButton, zoomInPosition, zoomInIncrement, zoomInSmoothing, zoomOutButton, zoomOutPosition, zoomOutIncrement, zoomOutSmoothing, zoomSlider, zoomView, restrictZoomMovement, useDoubleTap, doubleTapTargetTime, scrollWheelIncrement, scrollWheelSmoothing;
        private SimpleZoom zoom;
        #endregion

        #region Methods
        private void OnEnable()
        {
            zoom = target as SimpleZoom;

            #region Serialized Properties
            defaultZoom = serializedObject.FindProperty("defaultZoom");
            minMaxZoom = serializedObject.FindProperty("minMaxZoom");
            zoomTarget = serializedObject.FindProperty("zoomTarget");
            customPosition = serializedObject.FindProperty("customPosition");
            zoomType = serializedObject.FindProperty("zoomType");
            elasticLimit = serializedObject.FindProperty("elasticLimit");
            elasticDamping = serializedObject.FindProperty("elasticDamping");
            zoomMode = serializedObject.FindProperty("zoomMode");
            zoomInButton = serializedObject.FindProperty("zoomInButton");
            zoomInPosition = serializedObject.FindProperty("zoomInPosition");
            zoomInIncrement = serializedObject.FindProperty("zoomInIncrement");
            zoomInSmoothing = serializedObject.FindProperty("zoomInSmoothing");
            zoomOutButton = serializedObject.FindProperty("zoomOutButton");
            zoomOutPosition = serializedObject.FindProperty("zoomOutPosition");
            zoomOutIncrement = serializedObject.FindProperty("zoomOutIncrement");
            zoomOutSmoothing = serializedObject.FindProperty("zoomOutSmoothing");
            zoomSlider = serializedObject.FindProperty("zoomSlider");
            zoomView = serializedObject.FindProperty("zoomView");
            restrictZoomMovement = serializedObject.FindProperty("restrictZoomMovement");
            useDoubleTap = serializedObject.FindProperty("useDoubleTap");
            doubleTapTargetTime = serializedObject.FindProperty("doubleTapTargetTime");
            scrollWheelIncrement = serializedObject.FindProperty("scrollWheelIncrement");
            scrollWheelSmoothing = serializedObject.FindProperty("scrollWheelSmoothing");
            #endregion
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ShowCopyrightNotice();
            ShowBasicSettings();
            ShowControlSettings();
            ShowPlatformSpecificSettings();

            serializedObject.ApplyModifiedProperties();
            PrefabUtility.RecordPrefabInstancePropertyModifications(zoom);
        }

        private void ShowBasicSettings()
        {
            EditorGUILayout.Space();
            EditorLayoutUtility.Header(ref showBasicSettings, new GUIContent("Basic Settings"));
            if (showBasicSettings)
            {
                zoom.DefaultZoom = EditorGUILayout.Slider(new GUIContent("Default Zoom", "The current value by which the scale/size is multiplied by when zooming."), zoom.DefaultZoom, zoom.MinMaxZoom.min, zoom.MinMaxZoom.max);
                EditorGUILayout.PropertyField(minMaxZoom, new GUIContent("Min Zoom", "The minimum and maximum values by which the scale/size could be multiplied by when zooming."));
                EditorGUILayout.PropertyField(zoomTarget, new GUIContent("Zoom Target", "Determines what value the content's pivot will be set to when zooming."));
                if (zoom.ZoomTarget == ZoomTarget.Custom)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(customPosition, new GUIContent("Position", "The custom value the pivot will be set to when zooming."));
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.PropertyField(zoomType, new GUIContent("Zoom Type", "Determines whether zooming may exceed the minimum/maximum zoom values and how it should be handled."));
                if (zoom.ZoomType == ZoomType.Elastic)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(elasticLimit, new GUIContent("Limit", "The amount by which the zoom can exceed the minimum/maximum zoom values."));
                    EditorGUILayout.PropertyField(elasticDamping, new GUIContent("Damping", "The amount by which the zoom is dampened when exceeding the minimum/maximum zoom values."));
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.PropertyField(zoomMode, new GUIContent("Zoom Mode", "Determines whether zooming transforms the scale or size (width/height) when zooming."));
                EditorGUILayout.PropertyField(useDoubleTap, new GUIContent("Use Double Tap", "Should users be able to double-tap to fully zoom in/out depending on the current zoom?"));
                if (zoom.UseDoubleTap)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(doubleTapTargetTime, new GUIContent("Target Time", "The amount of time users have in order for a double-tap to register."));
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.Space();
        }
        private void ShowControlSettings()
        {
            EditorLayoutUtility.Header(ref showControlSettings, new GUIContent("Control Settings"));
            if (showControlSettings)
            {
                EditorGUILayout.ObjectField(zoomInButton, typeof(Button), new GUIContent("Zoom In Button", "The button whose OnClick event listener will be assigned the \"ZoomIn\" method invocation on setup with values specified below."));
                if (zoom.ZoomInButton != null)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(zoomInPosition, new GUIContent("Position", "The custom value the pivot will be set to when zooming in."));
                    EditorGUILayout.PropertyField(zoomInIncrement, new GUIContent("Increment", "The amount by which the current zoom will be transformed by when zooming in."));
                    EditorGUILayout.PropertyField(zoomInSmoothing, new GUIContent("Smoothing", "The smoothing by which the current zoom will be transformed into the incremented zoom when zooming in."));
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.ObjectField(zoomOutButton, typeof(Button), new GUIContent("Zoom Out Button", "The button whose OnClick event listener will be assigned the \"ZoomOut\" method invocation on setup with values specified below."));
                if (zoom.ZoomOutButton != null)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(zoomOutPosition, new GUIContent("Position", "The custom value the pivot will be set to when zooming out."));
                    EditorGUILayout.PropertyField(zoomOutIncrement, new GUIContent("Increment", "The amount by which the current zoom will be transformed by when zooming out."));
                    EditorGUILayout.PropertyField(zoomOutSmoothing, new GUIContent("Smoothing", "The smoothing by which the current zoom will be transformed into the incremented zoom when zooming out."));
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.ObjectField(zoomSlider, typeof(Slider), new GUIContent("Zoom Slider", "A slider that displays the progress of the zoom represented by a number between 0 (minimum) and 1 (maximum)."));
                EditorGUILayout.ObjectField(zoomView, typeof(GameObject), new GUIContent("Zoom View", "A small scale representation of the zoom."));
            }
            EditorGUILayout.Space();
        }
        private void ShowPlatformSpecificSettings()
        {
            EditorLayoutUtility.Header(ref showPlatformSpecificSettings, new GUIContent("Platform-Specific Settings"));
            if (showPlatformSpecificSettings)
            {
                showMobile = EditorGUILayout.Foldout(showMobile, "Mobile", true);
                if (showMobile)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(restrictZoomMovement, new GUIContent("Restrict Zoom Movement", "Should the ScrollRect's movement be restricted when zooming?"));
                    EditorGUI.indentLevel--;
                }
                showDesktop = EditorGUILayout.Foldout(showDesktop, "Desktop", true);
                if (showDesktop)
                {
                    EditorGUI.indentLevel++;
                    showScrollWheel = EditorGUILayout.Foldout(showScrollWheel, "Scroll Wheel", true);
                    if (showScrollWheel)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(scrollWheelIncrement, new GUIContent("Increment", "The amount by which the current zoom will be transformed by when scrolling in/out."));
                        EditorGUILayout.PropertyField(scrollWheelSmoothing, new GUIContent("Smoothing", "The smoothing by which the current zoom will be transformed into the incremented zoom when scrolling in/out."));
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.Space();
        }
        #endregion
    }
}