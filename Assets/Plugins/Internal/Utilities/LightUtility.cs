using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public static class LightUtility
    {
        public static IEnumerator FadeRoutine(this Light light, float duration, float targetIntensity)
        {
            float currentTime = 0;
            float start = light.intensity;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                light.intensity = Mathf.Lerp(start, targetIntensity, currentTime / duration);
                yield return null;
            }
            light.intensity = targetIntensity;
        }
    }
}