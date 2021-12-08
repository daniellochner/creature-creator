// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
// Copyright (c) Daniel Lochner

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    [CustomEditor(typeof(SimpleScrollSnap))]
    public class SimpleScrollSnapEditor : SSSCopyrightEditor
    {
        #region Fields
        private bool showMovementAndLayoutSettings = true, showNavigationSettings = true, showSnapSettings = true, showTransitionEffects = true, showEvents = true;
        private SerializedProperty movementType, movementAxis, useAutomaticLayout, sizeControl, size, automaticLayoutSpacing, automaticLayoutMargins, useInfiniteScrolling, infiniteScrollingSpacing, useOcclusionCulling, startingPanel, useSwipeGestures, minimumSwipeSpeed, previousButton, nextButton, pagination, useToggleNavigation, snapTarget, snapSpeed, thresholdSpeedToSnap, useHardSnapping, useUnscaledTime, onTransitionEffects, onPanelSelected, onPanelCentered;
        private SimpleScrollSnap scrollSnap;
        #endregion

        #region Methods
        private void OnEnable()
        {
            scrollSnap = target as SimpleScrollSnap;

            #region Serialized Properties
            // Movement and Layout Settings
            movementType = serializedObject.FindProperty("movementType");
            movementAxis = serializedObject.FindProperty("movementAxis");
            useAutomaticLayout = serializedObject.FindProperty("useAutomaticLayout");
            sizeControl = serializedObject.FindProperty("sizeControl");
            size = serializedObject.FindProperty("size");
            automaticLayoutSpacing = serializedObject.FindProperty("automaticLayoutSpacing");
            automaticLayoutMargins = serializedObject.FindProperty("automaticLayoutMargins");
            useInfiniteScrolling = serializedObject.FindProperty("useInfiniteScrolling");
            infiniteScrollingSpacing = serializedObject.FindProperty("infiniteScrollingSpacing");
            useOcclusionCulling = serializedObject.FindProperty("useOcclusionCulling");
            startingPanel = serializedObject.FindProperty("startingPanel");

            // Navigation Settings
            useSwipeGestures = serializedObject.FindProperty("useSwipeGestures");
            minimumSwipeSpeed = serializedObject.FindProperty("minimumSwipeSpeed");
            previousButton = serializedObject.FindProperty("previousButton");
            nextButton = serializedObject.FindProperty("nextButton");
            pagination = serializedObject.FindProperty("pagination");
            useToggleNavigation = serializedObject.FindProperty("useToggleNavigation");

            // Snap Settings
            snapTarget = serializedObject.FindProperty("snapTarget");
            snapSpeed = serializedObject.FindProperty("snapSpeed");
            thresholdSpeedToSnap = serializedObject.FindProperty("thresholdSpeedToSnap");
            useHardSnapping = serializedObject.FindProperty("useHardSnapping");
            useUnscaledTime = serializedObject.FindProperty("useUnscaledTime");

            // Events
            onTransitionEffects = serializedObject.FindProperty("onTransitionEffects");
            onPanelSelected = serializedObject.FindProperty("onPanelSelected");
            onPanelCentered = serializedObject.FindProperty("onPanelCentered");
            #endregion
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            CopyrightNotice();
            MovementAndLayoutSettings();
            NavigationSettings();
            SnapSettings();
            Events();

            serializedObject.ApplyModifiedProperties();
            PrefabUtility.RecordPrefabInstancePropertyModifications(scrollSnap);
        }
        
        private void MovementAndLayoutSettings()
        {
            EditorGUILayout.Space();

            EditorLayoutUtility.Header(ref showMovementAndLayoutSettings, new GUIContent("Movement and Layout Settings"));
            if (showMovementAndLayoutSettings)
            {
                StartingPanel();
                MovementType();
            }
            EditorGUILayout.Space();
        }
        private void StartingPanel()
        {
            EditorGUILayout.IntSlider(startingPanel, 0, scrollSnap.NumberOfPanels - 1, new GUIContent("Starting Panel", "The number of the panel that will be displayed first, based on a 0-indexed array."));
        }
        private void MovementType()
        {
            EditorGUILayout.PropertyField(movementType, new GUIContent("Movement Type", "Determines how users will be able to move between panels within the ScrollRect."));
            if (scrollSnap.MovementType == SimpleScrollSnap.SSS_MovementType.Fixed)
            {
                EditorGUI.indentLevel++;

                MovementAxis();
                UseAutomaticLayout();
                UseInfiniteScrolling();
                UseOcclusionCulling();

                EditorGUI.indentLevel--;
            }
        }
        private void MovementAxis()
        {
            EditorGUILayout.PropertyField(movementAxis, new GUIContent("Movement Axis", "Determines the axis the user's movement will be restricted to."));
        }
        private void UseAutomaticLayout()
        {
            EditorGUILayout.PropertyField(useAutomaticLayout, new GUIContent("Use Automatic Layout", "Should panels be automatically positioned and scaled according to the specified movement axis, spacing, margins and size?"));
            if (scrollSnap.UseAutomaticLayout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(sizeControl, new GUIContent("Size Control", "Determines how the panels' size should be controlled."));
                if (scrollSnap.SizeControl == SimpleScrollSnap.SSS_SizeControl.Manual)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(size, new GUIContent("Size", "The size (in pixels) that panels will be when automatically laid out."));
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.Slider(automaticLayoutSpacing, 0, 1, new GUIContent("Spacing", "The spacing between panels, calculated using a fraction of the panel’s width (if the movement axis is horizontal) or height (if the movement axis is vertical)."));
                EditorGUILayout.PropertyField(automaticLayoutMargins, new GUIContent("Margins"));
                EditorGUI.indentLevel--;
            }
        }
        private void UseInfiniteScrolling()
        {
            EditorGUILayout.PropertyField(useInfiniteScrolling, new GUIContent("Use Infinite Scrolling", "Should panels wrap around to the opposite end once passed, giving the illusion of an infinite list of elements?"));
            if (scrollSnap.UseInfiniteScrolling)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Slider(infiniteScrollingSpacing, 0, 1, new GUIContent("End Spacing", "The spacing maintained between panels once wrapped around to the opposite end."));
                EditorGUI.indentLevel--;
            }
        }
        private void UseOcclusionCulling()
        {
            EditorGUILayout.PropertyField(useOcclusionCulling, new GUIContent("Use Occlusion Culling", "Should panels not visible in the viewport be disabled?"));
        }

        private void NavigationSettings()
        {
            EditorLayoutUtility.Header(ref showNavigationSettings, new GUIContent("Navigation Settings"));
            if (showNavigationSettings)
            {
                UseSwipeGestures();
                PreviousButton();
                NextButton();
                Pagination();
            }
            EditorGUILayout.Space();
        }
        private void UseSwipeGestures()
        {
            EditorGUILayout.PropertyField(useSwipeGestures, new GUIContent("Use Swipe Gestures", "Should users are able to use swipe gestures to navigate between panels?"));
            if (scrollSnap.UseSwipeGestures)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(minimumSwipeSpeed, new GUIContent("Minimum Swipe Speed", "The speed at which the user must be swiping in order for a transition to occur to another panel."));
                EditorGUI.indentLevel--;
            }
        }
        private void PreviousButton()
        {
            EditorGUILayout.ObjectField(previousButton, typeof(Button), new GUIContent("Previous Button", "(Optional) Button used to transition to the previous panel."));
        }
        private void NextButton()
        {
            EditorGUILayout.ObjectField(nextButton, typeof(Button), new GUIContent("Next Button", "(Optional) Button used to transition to the next panel."));
        }
        private void Pagination()
        {
            EditorGUILayout.ObjectField(pagination, typeof(ToggleGroup), new GUIContent("Pagination", "(Optional) ToggleGroup containing Toggles that shows the current position of the user and can be used to transition to a selected panel."));
            if (scrollSnap.Pagination != null)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(useToggleNavigation, new GUIContent("Toggle Navigation", "Should users be able to transition to panels by clicking on their respective toggle."));
                int numberOfToggles = scrollSnap.Pagination.transform.childCount;
                if (numberOfToggles != scrollSnap.NumberOfPanels)
                {
                    EditorGUILayout.HelpBox("The number of toggles should be equivalent to the number of panels. There are currently " + numberOfToggles + " toggles and " + scrollSnap.NumberOfPanels + " panels.", MessageType.Warning);
                }
                EditorGUI.indentLevel--;
            }
        }

        private void SnapSettings()
        {
            EditorLayoutUtility.Header(ref showSnapSettings, new GUIContent("Snap Settings"));
            if (showSnapSettings)
            {
                SnapTarget();
                SnapSpeed();
                ThresholdSpeedToSnap();
                UseHardSnapping();
                UseUnscaledTime();
            }
            EditorGUILayout.Space();
        }
        private void SnapTarget()
        {
            using (new EditorGUI.DisabledScope(scrollSnap.MovementType == SimpleScrollSnap.SSS_MovementType.Free))
            {
                EditorGUILayout.PropertyField(snapTarget, new GUIContent("Snap Target", "Determines what panel should be targeted and snapped to once the threshold snapping speed has been reached."));
            }
            if (scrollSnap.MovementType == SimpleScrollSnap.SSS_MovementType.Free)
            {
                scrollSnap.SnapTarget = SimpleScrollSnap.SSS_SnapTarget.Nearest;
            }
        }
        private void SnapSpeed()
        {
            EditorGUILayout.PropertyField(snapSpeed, new GUIContent("Snap Speed", "The speed at which the targeted panel snaps into position."));
        }
        private void ThresholdSpeedToSnap()
        {
            EditorGUILayout.PropertyField(thresholdSpeedToSnap, new GUIContent("Threshold Speed To Snap", "The speed at which the ScrollRect will stop scrolling and begin snapping to the targeted panel (where -1 is used as infinity)."));
        }
        private void UseHardSnapping()
        {
            EditorGUILayout.PropertyField(useHardSnapping, new GUIContent("Use Hard Snapping", "Should the inertia of the ScrollRect be disabled once a panel has been selected? If enabled, the ScrollRect will not overshoot the targeted panel when snapping into position and instead Lerp precisely towards the targeted panel."));
        }
        private void UseUnscaledTime()
        {
            EditorGUILayout.PropertyField(useUnscaledTime, new GUIContent("Use Unscaled Time", "Should the scroll-snap update irrespective of the time scale?"));
        }

        private void Events()
        {
            EditorLayoutUtility.Header(ref showEvents, new GUIContent("Events"));
            if (showEvents)
            {
                EditorGUILayout.PropertyField(onTransitionEffects);
                EditorGUILayout.PropertyField(onPanelSelected);
                EditorGUILayout.PropertyField(onPanelCentered);
            }
        }
        #endregion
    }
}