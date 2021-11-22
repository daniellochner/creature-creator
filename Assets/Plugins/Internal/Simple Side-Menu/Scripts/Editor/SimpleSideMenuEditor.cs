// Simple Side-Menu - https://assetstore.unity.com/packages/tools/gui/simple-side-menu-143623
// Version: 1.0.3
// Author: Daniel Lochner

using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    [CustomEditor(typeof(SimpleSideMenu))]
    public class SimpleSideMenuEditor : Editor
    {
        #region Fields
        private bool showBasicSettings = true, showDragSettings = true, showOverlaySettings = true, showEvents = true;
        private SerializedProperty placement, defaultState, transitionSpeed, thresholdDragSpeed, thresholdDragDistance, thresholdDraggedFraction, handle, handleDraggable, handleToggleStateOnPressed, menuDraggable, useOverlay, overlayColour, useBlur, blurMaterial, blurRadius, overlaySwipe, overlayRetractOnPressed, onStateChanged, onStateSelected, onStateChanging, onStateSelecting;
        private SimpleSideMenu.State editorState;
        private SimpleSideMenu simpleSideMenu;
        #endregion

        #region Methods
        private void OnEnable()
        {
            simpleSideMenu = target as SimpleSideMenu;

            //Serialized Properties
            placement = serializedObject.FindProperty("placement");
            defaultState = serializedObject.FindProperty("defaultState");
            transitionSpeed = serializedObject.FindProperty("transitionSpeed");
            thresholdDragSpeed = serializedObject.FindProperty("thresholdDragSpeed");
            thresholdDraggedFraction = serializedObject.FindProperty("thresholdDraggedFraction");
            handle = serializedObject.FindProperty("handle");
            handleDraggable = serializedObject.FindProperty("handleDraggable");
            handleToggleStateOnPressed = serializedObject.FindProperty("handleToggleStateOnPressed");
            menuDraggable = serializedObject.FindProperty("menuDraggable");
            useOverlay = serializedObject.FindProperty("useOverlay");
            overlayColour = serializedObject.FindProperty("overlayColour");
            useBlur = serializedObject.FindProperty("useBlur");
            blurMaterial = serializedObject.FindProperty("blurMaterial");
            blurRadius = serializedObject.FindProperty("blurRadius");
            overlayRetractOnPressed = serializedObject.FindProperty("overlayCloseOnPressed");
            onStateSelected = serializedObject.FindProperty("onStateSelected");
            onStateSelecting = serializedObject.FindProperty("onStateSelecting");
            onStateChanging = serializedObject.FindProperty("onStateChanging");
            onStateChanged = serializedObject.FindProperty("onStateChanged");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            HeaderInformation();
            CurrentStateSettings();

            BasicSettings();
            DragSettings();
            OverlaySettings();
            EventHandlers();

            serializedObject.ApplyModifiedProperties();
            PrefabUtility.RecordPrefabInstancePropertyModifications(simpleSideMenu);
        }

        private void HeaderInformation()
        {
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Simple Side-Menu", new GUIStyle() { fontSize = 30, alignment = TextAnchor.MiddleCenter });
            GUILayout.Label("Version: 1.0.3", new GUIStyle() { fontSize = 14, alignment = TextAnchor.MiddleCenter });
            GUILayout.Label("Author: Daniel Lochner", new GUIStyle() { fontSize = 14, alignment = TextAnchor.MiddleCenter });
            GUILayout.EndVertical();
        }
        private void CurrentStateSettings()
        {
            editorState = (Application.isPlaying) ? simpleSideMenu.TargetState : simpleSideMenu.defaultState;
            #region Close
            EditorGUILayout.BeginHorizontal();
            using (new EditorGUI.DisabledScope(editorState == SimpleSideMenu.State.Closed))
            {
                if (GUILayout.Button("Close"))
                {
                    simpleSideMenu.Close();
                    if (!Application.isPlaying) simpleSideMenu.defaultState = SimpleSideMenu.State.Closed;
                }
            }
            #endregion
            #region Toggle State
            if (GUILayout.Button("Toggle State"))
            {
                simpleSideMenu.ToggleState();
                if (!Application.isPlaying)
                {
                    switch (simpleSideMenu.defaultState)
                    {
                        case SimpleSideMenu.State.Closed:
                            simpleSideMenu.defaultState = SimpleSideMenu.State.Open;
                            break;
                        case SimpleSideMenu.State.Open:
                            simpleSideMenu.defaultState = SimpleSideMenu.State.Closed;
                            break;
                    }
                }
            }
            #endregion
            #region Open
            using (new EditorGUI.DisabledScope(editorState == SimpleSideMenu.State.Open))
            {
                if (GUILayout.Button("Open"))
                {
                    simpleSideMenu.Open();
                    if (!Application.isPlaying) simpleSideMenu.defaultState = SimpleSideMenu.State.Open;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            #endregion
        }

        private void BasicSettings()
        {
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            showBasicSettings = EditorGUILayout.Foldout(showBasicSettings, "Basic Settings", true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (showBasicSettings)
            {
                EditorGUILayout.PropertyField(placement, new GUIContent("Placement", "The position at which the menu will be placed, which determines how the menu will be opened and closed."));
                EditorGUILayout.PropertyField(defaultState, new GUIContent("Default State", "Determines whether the menu will be open or closed by default."));
                EditorGUILayout.PropertyField(transitionSpeed, new GUIContent("Transition Speed", "The speed at which the menu will snap into position when transitioning to the next state."));
            }

            EditorGUILayout.Space();
        }
        private void DragSettings()
        {
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            showDragSettings = EditorGUILayout.Foldout(showDragSettings, "Drag Settings", true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (showDragSettings)
            {
                EditorGUILayout.PropertyField(thresholdDragSpeed, new GUIContent("Threshold Drag Speed", "The minimum speed required when dragging that will allow a transition to the next state to occur."));
                EditorGUILayout.Slider(thresholdDraggedFraction, 0f, 1f, new GUIContent("Threshold Dragged Fraction", "The fraction of the fully opened menu that must be dragged before a transition will occur to the next state if the current drag speed does not exceed the threshold drag speed set."));
                EditorGUILayout.ObjectField(handle, typeof(GameObject), new GUIContent("Handle", "(Optional) GameObject used to open and close the side menu by dragging or pressing (when a \"Button\" component has been added)."));
                if (simpleSideMenu.handle != null)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(handleDraggable, new GUIContent("Draggable", "Should the handle be able to be used to drag the Side-Menu?"));
                    EditorGUILayout.PropertyField(handleToggleStateOnPressed, new GUIContent("Toggle State on Pressed", "Should the Side-Menu toggle its state (open/close) when the handle is pressed?"));
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.PropertyField(menuDraggable, new GUIContent("Menu Draggable", "Should the Side-Menu (itself) be able to be used to drag the Side-Menu?"));
            }

            EditorGUILayout.Space();
        }
        private void OverlaySettings()
        {
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            showOverlaySettings = EditorGUILayout.Foldout(showOverlaySettings, "Overlay Settings", true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (showOverlaySettings)
            {
                EditorGUILayout.PropertyField(useOverlay, new GUIContent("Use Overlay", "Should an overlay be used when the Side-Menu is opened/closed?"));
                if (simpleSideMenu.useOverlay)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(overlayColour, new GUIContent("Colour", "The colour of the overlay when fully opened."));
                    EditorGUILayout.PropertyField(useBlur, new GUIContent("Blur", "Should a blur effect be applied to the overlay?"));
                    if (simpleSideMenu.useBlur)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(blurMaterial, new GUIContent("Material", "The material applied to the background blur. For the default render pipeline, please use the material provided."));
                        EditorGUILayout.IntSlider(blurRadius, 0, 20, new GUIContent("Radius", "Set the radius of the blur (Warning: The larger the radius, the poorer the performance)."));
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.PropertyField(overlayRetractOnPressed, new GUIContent("Close on Pressed", "Should the Side-Menu be closed when the overlay is pressed?"));
                    EditorGUI.indentLevel--;
                }
            }

            EditorGUILayout.Space();
        }

        private void EventHandlers()
        {
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            showEvents = EditorGUILayout.Foldout(showEvents, "Event Handlers", true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (showEvents)
            {
                EditorGUILayout.PropertyField(onStateSelecting, new GUIContent("On State Selecting"));
                EditorGUILayout.PropertyField(onStateSelected, new GUIContent("On State Selected"));
                EditorGUILayout.PropertyField(onStateChanging, new GUIContent("On State Changing"));
                EditorGUILayout.PropertyField(onStateChanged, new GUIContent("On State Changed"));
            }
        }

        [MenuItem("GameObject/UI/Simple Side-Menu/Left Side-Menu", false)]
        private static void CreateLeftSideMenu()
        {
            // Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObject = new GameObject("Canvas");
                canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.gameObject.AddComponent<GraphicRaycaster>();
                Undo.RegisterCreatedObjectUndo(canvasObject, "Create " + canvasObject.name);
            }

            // Side-Menu
            GameObject sideMenu = new GameObject("Left Side-Menu");
            RectTransform sideMenuRectTransform = sideMenu.AddComponent<RectTransform>();
            sideMenuRectTransform.anchorMin = new Vector2(0, 0);
            sideMenuRectTransform.anchorMax = new Vector2(0, 1);
            sideMenuRectTransform.pivot = new Vector2(1, 0.5f);
            sideMenuRectTransform.sizeDelta = new Vector2(500, 0);
            sideMenu.AddComponent<Image>();
            SimpleSideMenu sideMenuSimpleSideMenu = sideMenu.AddComponent<SimpleSideMenu>();
            UnityEditor.GameObjectUtility.SetParentAndAlign(sideMenu, canvas.gameObject);

            // Side-Menu Handle
            GameObject sideMenuHandle = new GameObject("Handle");
            RectTransform sideMenuHandleRectTransform = sideMenuHandle.AddComponent<RectTransform>();
            sideMenuHandleRectTransform.anchorMin = new Vector2(1, 0.5f);
            sideMenuHandleRectTransform.anchorMax = new Vector2(1, 0.5f);
            sideMenuHandleRectTransform.pivot = new Vector2(0, 0.5f);
            sideMenuHandleRectTransform.offsetMin = Vector2.zero;
            sideMenuHandleRectTransform.offsetMax = Vector2.zero;
            sideMenuHandleRectTransform.anchoredPosition = Vector2.zero;
            sideMenuHandleRectTransform.sizeDelta = new Vector2(75, 200);
            sideMenuHandle.AddComponent<Image>();
            sideMenuHandle.AddComponent<Button>();
            sideMenuSimpleSideMenu.handle = sideMenuHandle;
            UnityEditor.GameObjectUtility.SetParentAndAlign(sideMenuHandle, sideMenu);

            // Event System
            if (!FindObjectOfType<EventSystem>())
            {
                GameObject eventObject = new GameObject("EventSystem", typeof(EventSystem));
                eventObject.AddComponent<StandaloneInputModule>();
                Undo.RegisterCreatedObjectUndo(eventObject, "Create " + eventObject.name);
            }

            // Editor
            Selection.activeGameObject = sideMenu;
            Undo.RegisterCreatedObjectUndo(sideMenu, "Create " + sideMenu.name);
        }
        [MenuItem("GameObject/UI/Simple Side-Menu/Right Side-Menu", false)]
        private static void CreateRightSideMenu()
        {
            // Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObject = new GameObject("Canvas");
                canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.gameObject.AddComponent<GraphicRaycaster>();
                Undo.RegisterCreatedObjectUndo(canvasObject, "Create " + canvasObject.name);
            }

            // Side-Menu
            GameObject sideMenu = new GameObject("Right Side-Menu");
            RectTransform sideMenuRectTransform = sideMenu.AddComponent<RectTransform>();
            sideMenuRectTransform.anchorMin = new Vector2(1, 0);
            sideMenuRectTransform.anchorMax = new Vector2(1, 1);
            sideMenuRectTransform.pivot = new Vector2(0, 0.5f);
            sideMenuRectTransform.sizeDelta = new Vector2(500, 0);
            sideMenu.AddComponent<Image>();
            SimpleSideMenu sideMenuSimpleSideMenu = sideMenu.AddComponent<SimpleSideMenu>();
            sideMenuSimpleSideMenu.placement = SimpleSideMenu.Placement.Right;
            UnityEditor.GameObjectUtility.SetParentAndAlign(sideMenu, canvas.gameObject);

            // Side-Menu Handle
            GameObject sideMenuHandle = new GameObject("Handle");
            RectTransform sideMenuHandleRectTransform = sideMenuHandle.AddComponent<RectTransform>();
            sideMenuHandleRectTransform.anchorMin = new Vector2(0, 0.5f);
            sideMenuHandleRectTransform.anchorMax = new Vector2(0, 0.5f);
            sideMenuHandleRectTransform.pivot = new Vector2(1, 0.5f);
            sideMenuHandleRectTransform.offsetMin = Vector2.zero;
            sideMenuHandleRectTransform.offsetMax = Vector2.zero;
            sideMenuHandleRectTransform.anchoredPosition = Vector2.zero;
            sideMenuHandleRectTransform.sizeDelta = new Vector2(75, 200);
            sideMenuHandle.AddComponent<Image>();
            sideMenuHandle.AddComponent<Button>();
            sideMenuSimpleSideMenu.handle = sideMenuHandle;
            UnityEditor.GameObjectUtility.SetParentAndAlign(sideMenuHandle, sideMenu);

            // Event System
            if (!FindObjectOfType<EventSystem>())
            {
                GameObject eventObject = new GameObject("EventSystem", typeof(EventSystem));
                eventObject.AddComponent<StandaloneInputModule>();
                Undo.RegisterCreatedObjectUndo(eventObject, "Create " + eventObject.name);
            }

            // Editor
            Selection.activeGameObject = sideMenu;
            Undo.RegisterCreatedObjectUndo(sideMenu, "Create " + sideMenu.name);
        }
        [MenuItem("GameObject/UI/Simple Side-Menu/Top Side-Menu", false)]
        private static void CreateTopSideMenu()
        {
            // Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObject = new GameObject("Canvas");
                canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.gameObject.AddComponent<GraphicRaycaster>();
                Undo.RegisterCreatedObjectUndo(canvasObject, "Create " + canvasObject.name);
            }

            // Side-Menu
            GameObject sideMenu = new GameObject("Top Side-Menu");
            RectTransform sideMenuRectTransform = sideMenu.AddComponent<RectTransform>();
            sideMenuRectTransform.anchorMin = new Vector2(0, 1);
            sideMenuRectTransform.anchorMax = new Vector2(1, 1);
            sideMenuRectTransform.pivot = new Vector2(0.5f, 0);
            sideMenuRectTransform.sizeDelta = new Vector2(0, 500);
            sideMenu.AddComponent<Image>();
            SimpleSideMenu sideMenuSimpleSideMenu = sideMenu.AddComponent<SimpleSideMenu>();
            sideMenuSimpleSideMenu.placement = SimpleSideMenu.Placement.Top;
            UnityEditor.GameObjectUtility.SetParentAndAlign(sideMenu, canvas.gameObject);

            // Side-Menu Handle
            GameObject sideMenuHandle = new GameObject("Handle");
            RectTransform sideMenuHandleRectTransform = sideMenuHandle.AddComponent<RectTransform>();
            sideMenuHandleRectTransform.anchorMin = new Vector2(0.5f, 0);
            sideMenuHandleRectTransform.anchorMax = new Vector2(0.5f, 0);
            sideMenuHandleRectTransform.pivot = new Vector2(0.5f, 1);
            sideMenuHandleRectTransform.offsetMin = Vector2.zero;
            sideMenuHandleRectTransform.offsetMax = Vector2.zero;
            sideMenuHandleRectTransform.anchoredPosition = Vector2.zero;
            sideMenuHandleRectTransform.sizeDelta = new Vector2(200, 75);
            sideMenuHandle.AddComponent<Image>();
            sideMenuHandle.AddComponent<Button>();
            sideMenuSimpleSideMenu.handle = sideMenuHandle;
            UnityEditor.GameObjectUtility.SetParentAndAlign(sideMenuHandle, sideMenu);

            // Event System
            if (!FindObjectOfType<EventSystem>())
            {
                GameObject eventObject = new GameObject("EventSystem", typeof(EventSystem));
                eventObject.AddComponent<StandaloneInputModule>();
                Undo.RegisterCreatedObjectUndo(eventObject, "Create " + eventObject.name);
            }

            // Editor
            Selection.activeGameObject = sideMenu;
            Undo.RegisterCreatedObjectUndo(sideMenu, "Create " + sideMenu.name);
        }
        [MenuItem("GameObject/UI/Simple Side-Menu/Bottom Side-Menu", false)]
        private static void CreateBottomSideMenu()
        {
            // Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObject = new GameObject("Canvas");
                canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.gameObject.AddComponent<GraphicRaycaster>();
                Undo.RegisterCreatedObjectUndo(canvasObject, "Create " + canvasObject.name);
            }

            // Side-Menu
            GameObject sideMenu = new GameObject("Bottom Side-Menu");
            RectTransform sideMenuRectTransform = sideMenu.AddComponent<RectTransform>();
            sideMenuRectTransform.anchorMin = new Vector2(0, 0);
            sideMenuRectTransform.anchorMax = new Vector2(1, 0);
            sideMenuRectTransform.pivot = new Vector2(0.5f, 1);
            sideMenuRectTransform.sizeDelta = new Vector2(0, 500);
            sideMenu.AddComponent<Image>();
            SimpleSideMenu sideMenuSimpleSideMenu = sideMenu.AddComponent<SimpleSideMenu>();
            sideMenuSimpleSideMenu.placement = SimpleSideMenu.Placement.Bottom;
            UnityEditor.GameObjectUtility.SetParentAndAlign(sideMenu, canvas.gameObject);

            // Side-Menu Handle
            GameObject sideMenuHandle = new GameObject("Handle");
            RectTransform sideMenuHandleRectTransform = sideMenuHandle.AddComponent<RectTransform>();
            sideMenuHandleRectTransform.anchorMin = new Vector2(0.5f, 1);
            sideMenuHandleRectTransform.anchorMax = new Vector2(0.5f, 1);
            sideMenuHandleRectTransform.pivot = new Vector2(0.5f, 0);
            sideMenuHandleRectTransform.offsetMin = Vector2.zero;
            sideMenuHandleRectTransform.offsetMax = Vector2.zero;
            sideMenuHandleRectTransform.anchoredPosition = Vector2.zero;
            sideMenuHandleRectTransform.sizeDelta = new Vector2(200, 75);
            sideMenuHandle.AddComponent<Image>();
            sideMenuHandle.AddComponent<Button>();
            sideMenuSimpleSideMenu.handle = sideMenuHandle;
            UnityEditor.GameObjectUtility.SetParentAndAlign(sideMenuHandle, sideMenu);

            // Event System
            if (!FindObjectOfType<EventSystem>())
            {
                GameObject eventObject = new GameObject("EventSystem", typeof(EventSystem));
                eventObject.AddComponent<StandaloneInputModule>();
                Undo.RegisterCreatedObjectUndo(eventObject, "Create " + eventObject.name);
            }

            // Editor
            Selection.activeGameObject = sideMenu;
            Undo.RegisterCreatedObjectUndo(sideMenu, "Create " + sideMenu.name);
        }
        #endregion
    }
}