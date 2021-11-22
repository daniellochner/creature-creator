using System;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class TriggerRegion : MonoBehaviour
    {
        public Triggerable[] triggerables;

        public void OnTriggerEnter(Collider other)
        {
            foreach (Triggerable triggerable in triggerables)
            {
                if (other.CompareTag(triggerable.tag))
                {
                    triggerable.onEnter.Invoke();
                }
            }
        }
        public void OnTriggerExit(Collider other)
        {
            foreach (Triggerable triggerable in triggerables)
            {
                if (other.CompareTag(triggerable.tag))
                {
                    triggerable.onExit.Invoke();
                }
            }
        }

        [Serializable]
        public class Triggerable
        {
            public string tag;
            public UnityEvent onEnter;
            public UnityEvent onExit;
        }
    }
}