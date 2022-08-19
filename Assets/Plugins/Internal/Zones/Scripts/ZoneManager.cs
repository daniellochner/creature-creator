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
        public void SetZone(Zone zone)
        {
            if (currentZone == zone) return;

            if (currentZone != null)
            {
                currentZone.onExit?.Invoke();
            }
            if (zone != null)
            {
                zone.onEnter?.Invoke();
            }
            currentZone = zone;

            if (zone != null)
            {
                NotificationsManager.Notify($"You entered <b>{zone.name}</b>.");
            }
        }
        #endregion
    }
}