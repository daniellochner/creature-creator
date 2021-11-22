using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public static class CanvasGroupUtility
    {
        public static IEnumerator Fade(this CanvasGroup canvasGroup, bool visible, float duration, bool setInteractable = true, UnityAction endFadeEvent = null)
        {
            if (setInteractable) { canvasGroup.interactable = canvasGroup.blocksRaycasts = visible; }

            if (visible)
            {
                for (float i = canvasGroup.alpha; i < 1; i += Time.unscaledDeltaTime / duration)
                {
                    canvasGroup.alpha = i;
                    yield return null;
                }
                canvasGroup.alpha = 1;
            }
            else
            {
                for (float i = canvasGroup.alpha; i > 0; i -= Time.unscaledDeltaTime / duration)
                {
                    canvasGroup.alpha = i;
                    yield return null;
                }
                canvasGroup.alpha = 0;
            }

            endFadeEvent?.Invoke();
        }
    }
}