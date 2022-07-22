// Zones
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets
{
    public class Zone : MonoBehaviour
    {
        #region Methods
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(ZoneManager.Instance.PlayerTag))
            {
                ZoneManager.Instance.EnterZone(this);
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(ZoneManager.Instance.PlayerTag))
            {
                ZoneManager.Instance.CurrentZone = null;
            }
        }
        #endregion
    }
}