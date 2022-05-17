using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    /// <summary>
    /// Unity doesn't handle OnTriggerExit for colliders that are disabled or destroyed. 
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TrackRegion : MonoBehaviour
    {
        #region Fields
        [SerializeField] private List<string> trackable;

        private List<Collider> tracked = new List<Collider>();
        private List<Collider> toBeRemoved = new List<Collider>();

        private new Collider collider;
        #endregion

        #region Methods
        private void Awake()
        {
            collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }
        private void OnDisable()
        {
            LoseTrackOfAll();
        }
        private void OnDestroy()
        {
            LoseTrackOfAll();
        }

        private void FixedUpdate()
        {
            HandleTracked();
            HandleRemoval();
        }
        private void OnTriggerEnter(Collider other)
        {
            Track(other);
        }
        private void OnTriggerExit(Collider other)
        {
            LoseTrackOf(other);
        }

        private void HandleTracked()
        {
            foreach (Collider col in tracked)
            {
                if (col == null || col.enabled == false || col.gameObject.activeSelf == false)
                {
                    toBeRemoved.Add(col);
                }
            }
        }
        private void HandleRemoval()
        {
            if (toBeRemoved.Count > 0)
            {
                foreach (Collider col in toBeRemoved)
                {
                    LoseTrackOf(col);
                }
                toBeRemoved.Clear();
            }
        }

        private void Track(Collider col)
        {
            if (trackable.Contains(col.tag))
            {
                tracked.Add(col);
                OnTrack(col);
            }
        }
        private void LoseTrackOf(Collider col)
        {
            if (tracked.Contains(col))
            {
                tracked.Remove(col);
                OnLoseTrackOf(col);
            }
        }
        private void LoseTrackOfAll()
        {
            foreach (Collider col in tracked)
            {
                LoseTrackOf(col);
            }
            tracked.Clear();
        }

        public virtual void OnTrack(Collider col)
        {
        }
        public virtual void OnLoseTrackOf(Collider col)
        {
        }
        #endregion
    }
}