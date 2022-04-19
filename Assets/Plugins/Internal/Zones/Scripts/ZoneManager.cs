// Zones
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets
{
    public class ZoneManager : MonoBehaviourSingleton<ZoneManager>
    {
        #region Fields
        [SerializeField] private Zone currentZone;

        private Zone previousZone;
        #endregion

        #region Properties
        public Zone CurrentZone
        {
            get => currentZone;
            set => currentZone = value;
        }
        #endregion

        #region Methods
        public void EnterZone(Zone zone)
        {
            if (currentZone == this || previousZone == this) return;

            NotificationsManager.Notify($"You entered {zone.name}.");
            currentZone = previousZone = zone;
        }
        #endregion
    }
}