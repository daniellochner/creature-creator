// Startup
// Copyright (c) Daniel Lochner

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets
{
    [InitializeOnLoad]
    public static class StartupSceneLoader
    {
        private static string PreviousScene
        {
            get => EditorPrefs.GetString("prevScene");
            set => EditorPrefs.SetString("prevScene", value);
        }

        static StartupSceneLoader()
        {
            EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
            {
                foreach (string ignoredScene in StartupWindow.Window.IgnoredScenes)
                {
                    if (SceneManager.GetActiveScene().name.Equals(ignoredScene)) return;
                }

                switch (state)
                {
                    case PlayModeStateChange.ExitingEditMode:
                        ExitEditMode();
                        break;
                    case PlayModeStateChange.EnteredEditMode:
                        EnterEditMode();
                        break;
                }
            };
        }

        private static void EnterEditMode()
        {
            if (!string.IsNullOrEmpty(PreviousScene))
            {
                EditorSceneManager.OpenScene(PreviousScene);
            }
        }
        private static void ExitEditMode()
        {
            PreviousScene = SceneManager.GetActiveScene().path;

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorBuildSettingsScene startupScene = System.Array.Find(EditorBuildSettings.scenes, scene => scene.path.EndsWith("Startup.unity"));
                if (startupScene != null)
                {
                    EditorSceneManager.OpenScene(startupScene.path);
                }
            }
            else
            {
                EditorApplication.isPlaying = false;
            }
        }
    }
}