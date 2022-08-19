// Zones
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    public class Zone : MonoBehaviour
    {
        #region Fields
        public UnityEvent onEnter;
        public UnityEvent onExit;
        #endregion

        #region Methods
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(ZoneManager.Instance.PlayerTag))
            {
                Debug.Log("ENTER" + name);
                ZoneManager.Instance.SetZone(this);
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(ZoneManager.Instance.PlayerTag))
            {
                Debug.Log("EXIT" + name);

                ZoneManager.Instance.SetZone(null);
            }
        }
        #endregion
    }
}