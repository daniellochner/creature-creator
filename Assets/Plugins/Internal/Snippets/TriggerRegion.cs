using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class TriggerRegion : MonoBehaviour
    {
        #region Fields
        [SerializeField] private List<string> triggerable;
        [Space]
        [SerializeField] private UnityEvent<Collider> onEnter;
        [SerializeField] private UnityEvent<Collider> onExit;
        #endregion

        #region Methods
        private void OnTriggerEnter(Collider other)
        {
            if (triggerable.Contains(other.tag))
            {
                onEnter.Invoke(other);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (triggerable.Contains(other.tag))
            {
                onExit.Invoke(other);
            }
        }
        #endregion
    }
}