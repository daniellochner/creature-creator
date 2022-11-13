using System;
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
        [SerializeField] private Collider region;
        [SerializeField] private List<string> trackable;
        [SerializeField] private List<string> ignored;
        [ReadOnly] public List<Collider> tracked = new List<Collider>();

        private List<Collider> toBeRemoved = new List<Collider>();
        #endregion

        #region Properties
        public Action<Collider, Collider> OnTrack { get; set; }
        public Action<Collider, Collider> OnLoseTrackOf { get; set; }

        public Collider Nearest
        {
            get
            {
                Collider nearest = null;
                float min = Mathf.Infinity;

                foreach (Collider collider in tracked)
                {
                    float sqr = Vector3.SqrMagnitude(collider.transform.position - transform.position);
                    if (sqr < min)
                    {
                        nearest = collider;
                        min = sqr;
                    }
                }

                return nearest;
            }
        }
        #endregion

        #region Methods
        private void Awake()
        {
            if (region == null)
            {
                region = GetComponent<Collider>();
                region.isTrigger = true;
            }
        }
        private void OnEnable()
        {
            region.enabled = true;
        }
        private void OnDisable()
        {
            region.enabled = false;
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
            if (!ignored.Contains(other.name))
            {
                Track(other);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (!ignored.Contains(other.name))
            {
                LoseTrackOf(other);
            }
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
            if (trackable.Count == 0 || trackable.Contains(col.tag))
            {
                tracked.Add(col);
                OnTrack?.Invoke(col, col);
            }
        }
        private void LoseTrackOf(Collider col)
        {
            if (tracked.Contains(col))
            {
                tracked.Remove(col);
                OnLoseTrackOf?.Invoke(col, col);
            }
        }
        public void LoseTrackOfAll()
        {
            for (int i = 0; i < tracked.Count; ++i)
            {
                LoseTrackOf(tracked[i]);
            }
            tracked.Clear();
        }
        #endregion
    }
}