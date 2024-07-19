using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets
{
    public static class CoroutineUtility
    {
        public static void StopStartCoroutine(this MonoBehaviour monoBehaviour, IEnumerator enumerator, ref Coroutine current)
        {
            if (current != null)
            {
                monoBehaviour.StopCoroutine(current);
            }
            current = monoBehaviour.StartCoroutine(enumerator);
        }

        public static void TryStopCoroutine(this MonoBehaviour monoBehaviour, Coroutine coroutine)
        {
            if (coroutine != null)
            {
                monoBehaviour.StopCoroutine(coroutine);
            }
        }
    }
}