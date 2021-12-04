using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace DanielLochner.Assets
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] public string sceneToLoad;

        private void Start()
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class LoadStartupScene
    {
        private static string PreviousScene
        {
            get => EditorPrefs.GetString("prevScene");
            set => EditorPrefs.SetString("prevScene", value);
        }

        static LoadStartupScene()
        {
            EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
            {
                if (SceneManager.GetActiveScene().name.Equals("Testing")) return;

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
#endif
}