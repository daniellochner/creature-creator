// Fader
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class Fader : MonoBehaviourSingleton<Fader>
    {
        #region Fields
        private static int maximumSortingOrder = 100;

        private CanvasGroup fadeCanvasGroup;
        #endregion

        #region Methods
        public static void Fade(bool targetVisibility, float fadeDuration, UnityAction endFadeEvent = null)
        {
            if (Instance == null)
            {
                // Instantiate GameObject
                GameObject fadeCanvasGO = new GameObject("Fade");
                fadeCanvasGO.transform.SetAsLastSibling();
                fadeCanvasGO.AddComponent<DoNotDestroy>();
                fadeCanvasGO.AddComponent<Fader>(); // Singleton set on Awake()

                // Add Canvas
                Canvas fadeCanvas = fadeCanvasGO.AddComponent<Canvas>();
                fadeCanvas.sortingOrder = maximumSortingOrder;
                fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

                // Add RawImage
                RawImage rawImage = fadeCanvasGO.AddComponent<RawImage>();
                rawImage.color = Color.black;

                // Add CanvasGroup
                Instance.fadeCanvasGroup = fadeCanvasGO.AddComponent<CanvasGroup>();
                Instance.fadeCanvasGroup.alpha = targetVisibility ? 0 : 1;
            }

            // Fade CanvasGroup In/Out
            Instance.StartCoroutine(FadeRoutine(targetVisibility, fadeDuration, endFadeEvent));
        }

        private static IEnumerator FadeRoutine(bool targetVisibility, float fadeDuration, UnityAction endFadeEvent = null)
        {
            yield return new WaitForEndOfFrame(); // Start fading on next frame.

            yield return Instance.StartCoroutine(Instance.fadeCanvasGroup.Fade(targetVisibility, fadeDuration, true, endFadeEvent));
        }
        #endregion
    }
}