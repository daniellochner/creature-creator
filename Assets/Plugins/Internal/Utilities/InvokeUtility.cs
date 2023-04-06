using System;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets
{
    public static class InvokeUtility
    {
        public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action invokeFunction, float delay = 0f)
        {
            return monoBehaviour.StartCoroutine(InvokeRoutine(invokeFunction, delay));
        }
        public static Coroutine InvokeAtEndOfFrame(this MonoBehaviour monoBehaviour, Action invokeFunction)
        {
            return monoBehaviour.StartCoroutine(InvokeAtEndOfFrameRoutine(invokeFunction));
        }
        public static Coroutine InvokeOverTime(this MonoBehaviour monoBehaviour, Action<float> invokeFunction, float time, float timeScale = 1f)
        {
            return monoBehaviour.StartCoroutine(InvokeOverTimeRoutine(invokeFunction, time, timeScale));
        }
        public static Coroutine InvokeUntil(this MonoBehaviour monoBehaviour, Func<bool> predicate, Action onComplete)
        {
            return monoBehaviour.StartCoroutine(InvokeUntil(predicate, onComplete));
        }

        public static IEnumerator InvokeRoutine(Action invokeFunction, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);
            invokeFunction();
        }
        public static IEnumerator InvokeAtEndOfFrameRoutine(Action invokeFunction)
        {
            yield return new WaitForEndOfFrame();
            invokeFunction();
        }
        public static IEnumerator InvokeOverTimeRoutine(Action<float> onProgress, float timeToMove, float timeScale = 1f)
        {
            float timeElapsed = 0f, progress = 0f;
            while (progress < 1f)
            {
                timeElapsed += Time.deltaTime * timeScale;
                progress = timeElapsed / timeToMove;

                onProgress(progress);
                
                yield return null;
            }
        }
        public static IEnumerator InvokeUntil(Func<bool> predicate, Action onComplete)
        {
            yield return new WaitUntil(predicate);
            onComplete();
        }
    }
}