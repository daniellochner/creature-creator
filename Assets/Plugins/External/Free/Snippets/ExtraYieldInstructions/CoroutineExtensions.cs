using System.Collections;
using UnityEngine;

namespace CustomYieldInstructions
{
    public static class CoroutineExtensions
    {
        public static IEnumerator WaitAll(this MonoBehaviour monoBehaviour, params IEnumerator[] coroutines)
        {
            return new WaitAll(monoBehaviour, coroutines);
        }
    
        public static IEnumerator WaitAny(this MonoBehaviour monoBehaviour, params IEnumerator[] coroutines)
        {
            return new WaitAny(monoBehaviour, coroutines);
        }
    }
}