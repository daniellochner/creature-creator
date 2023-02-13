// Zones
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class Zone : MonoBehaviour
    {
        #region Fields
        [SerializeField] public bool notify = true;
        [SerializeField] private float entryDelay;
        public UnityEvent onEnter;
        public UnityEvent onExit;
        #endregion

        #region Methods
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(ZoneManager.Instance.PlayerTag))
            {
                ZoneManager.Instance.EnterZone(this, notify);
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(ZoneManager.Instance.PlayerTag))
            {
                ZoneManager.Instance.ExitCurrentZone(other.transform.position);
            }
        }
        #endregion
    }
}