// Zones
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets
{
    public class ZoneManager : MonoBehaviourSingleton<ZoneManager>
    {
        #region Fields
        [SerializeField] private string playerTag;
        private Zone currentZone;
        #endregion

        #region Properties
        public Zone CurrentZone
        {
            get => currentZone;
            set => currentZone = value;
        }

        public string PlayerTag => playerTag;
        #endregion

        #region Methods
        public void EnterZone(Zone zone)
        {
            if (zone == null || currentZone == zone) return;

            zone.onEnter?.Invoke();
            currentZone = zone;

            NotificationsManager.Notify($"You entered <b>{zone.name}</b>.");
        }
        public void ExitCurrentZone(Vector3 exitPosition)
        {
            Zone zoneToEnter = null;
            Collider[] cols = Physics.OverlapSphere(exitPosition, 0.01f, LayerMask.GetMask("Zone"), QueryTriggerInteraction.Collide);
            foreach (Collider col in cols)
            {
                Zone zone = col.GetComponent<Zone>();
                if (zone != null)
                {
                    zoneToEnter = zone;
                    break;
                }
            }
            
            // TODO: Tidy up!
            if (zoneToEnter != null)
            {
                if (zoneToEnter != currentZone)
                {
                    if (currentZone != null)
                    {
                        currentZone.onExit?.Invoke();
                    }
                    EnterZone(zoneToEnter);
                }
                else
                {
                    // do nothing...
                }
            }
            else
            {
                if (currentZone != null)
                {
                    currentZone.onExit?.Invoke();
                }
                currentZone = null;
            }
        }
        #endregion
    }
}