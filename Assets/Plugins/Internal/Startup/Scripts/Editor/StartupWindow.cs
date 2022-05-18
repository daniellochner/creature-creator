// Startup
// Copyright (c) Daniel Lochner

using UnityEditor;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class StartupWindow : EditorWindow
    {
        #region Fields
        [SerializeField] private string[] ignoredScenes;

        private SerializedProperty _ignoredScenes;
        private SerializedObject target;
        #endregion

        #region Properties
        public string[] IgnoredScenes => ignoredScenes;

        public static StartupWindow Window => GetWindow<StartupWindow>("Startup", false);
        #endregion

        #region Methods
        private void OnEnable()
        {
            target = new SerializedObject(this);

            _ignoredScenes = target.FindProperty("ignoredScenes");
        }
        private void OnGUI()
        {
            target.Update();

            EditorGUILayout.PropertyField(_ignoredScenes);

            target.ApplyModifiedProperties();
        }

        [MenuItem("Window/Startup")]
        public static void ShowWindow()
        {
            Window.Show();
        }
        #endregion
    }
}