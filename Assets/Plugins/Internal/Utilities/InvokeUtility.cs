using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public static class InvokeUtility
    {
        public static Coroutine Invoke(this MonoBehaviour monoBehaviour, UnityAction invokeFunction, float delay = 0)
        {
            return monoBehaviour.StartCoroutine(InvokeRoutine(invokeFunction, delay));
        }
        public static Coroutine InvokeAtEndOfFrame(this MonoBehaviour monoBehaviour, UnityAction invokeFunction, float delay = 0)
        {
            return monoBehaviour.StartCoroutine(InvokeAtEndOfFrameRoutine(invokeFunction));
        }

        public static IEnumerator InvokeRoutine(UnityAction invokeFunction, float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            invokeFunction();
        }
        public static IEnumerator InvokeAtEndOfFrameRoutine(UnityAction invokeFunction)
        {
            yield return new WaitForEndOfFrame();
            invokeFunction();
        }
    }
}