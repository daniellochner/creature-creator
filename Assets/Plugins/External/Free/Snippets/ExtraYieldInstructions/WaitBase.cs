using System.Collections;
using UnityEngine;

namespace CustomYieldInstructions
{
    public abstract class WaitBase : CustomYieldInstruction
    {
        protected readonly bool[] _wait;

        protected WaitBase(MonoBehaviour monoBehaviour, params IEnumerator[] coroutines)
        {
            _wait = new bool[coroutines.Length];
            for(int i = 0; i < coroutines.Length; i++)
            {
                monoBehaviour.StartCoroutine(Wrapper(coroutines[i], i));
            }
        }

        private IEnumerator Wrapper(IEnumerator e, int index)
        {
            while (true)
            {
                if(e != null && e.MoveNext())
                {
                    _wait[index] = true; 
                    yield return e.Current;
                }
                else
                {
                    _wait[index] = false;
                    break;
                }
            }
        }
    }
}