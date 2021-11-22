// Loading
// Copyright (c) Daniel Lochner

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class LoadingManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Slider loadingBarSlider;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI hintText;
        [SerializeField] private TextMeshProUGUI progressText;
        [Space]
        [SerializeField] private Sprite[] backgroundImages = new Sprite[] { null };
        [SerializeField] private string[] hints = new string[] { "Default Hint" };

        private static string targetScene, currentScene;
        private static float fadeDuration = 0.5f;
        private static UnityAction onTargetSceneLoaded;
        #endregion

        #region Properties
        public static bool IsLoading { get; private set; }
        #endregion

        #region Methods
        private void Start()
        {
            // [2] FadeIn: LoadingScreen (from Current)
            Fader.Fade(false, fadeDuration, delegate { StartCoroutine(LoadSceneRoutine(targetScene)); });

            backgroundImage.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
            hintText.text = "<b>Hint:</b> " + hints[Random.Range(0, hints.Length)];
        }
        private IEnumerator LoadSceneRoutine(string scene)
        {
            AsyncOperation loadingAsyncOperation = SceneManager.LoadSceneAsync(scene);

            loadingAsyncOperation.allowSceneActivation = false;
            loadingAsyncOperation.completed += delegate
            {
                onTargetSceneLoaded?.Invoke();

                // [4] FadeIn: Target (from LoadingScreen)
                Fader.Fade(false, fadeDuration, delegate
                {
                    IsLoading = false;
                });
            };

            while (!loadingAsyncOperation.isDone)
            {
                float loadProgress = Mathf.Clamp01(loadingAsyncOperation.progress / 0.9f);
                loadingBarSlider.value = loadProgress;
                progressText.text = Mathf.RoundToInt(loadProgress * 100) + "%";

                if (loadProgress >= 1f)
                {
                    break;
                }
                else { yield return null; }
            }

            // [3] FadeOut: LoadingScreen (to Target)
            Fader.Fade(true, fadeDuration, delegate 
            {
                loadingAsyncOperation.allowSceneActivation = true;
            });
        }

        public static void LoadScene(string targetScene, UnityAction onLoadingSceneLoaded = default, UnityAction onTargetSceneLoaded = default)
        {
            IsLoading = true;

            currentScene = SceneManager.GetActiveScene().name;
            LoadingManager.targetScene = targetScene;
            LoadingManager.onTargetSceneLoaded = onTargetSceneLoaded;

            // [1] FadeOut: Current (to LoadingScreen)
            Fader.Fade(true, fadeDuration, delegate
            {
                SceneManager.LoadScene("LoadingScreen");
                onLoadingSceneLoaded?.Invoke();
            });
        }
        #endregion
    }
}