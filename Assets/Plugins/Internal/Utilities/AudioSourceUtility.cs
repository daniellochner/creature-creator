using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets
{
    public static class AudioSourceUtility
    {
        public static IEnumerator FadeRoutine(this AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            audioSource.volume = targetVolume;
        }
    }
}