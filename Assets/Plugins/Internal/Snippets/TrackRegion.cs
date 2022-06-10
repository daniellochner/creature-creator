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
        [SerializeField] private List<string> trackable;

        public List<Collider> tracked = new List<Collider>();
        private List<Collider> toBeRemoved = new List<Collider>();

        private new Collider collider;
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
            collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }
        private void OnEnable()
        {
            collider.enabled = true;
        }
        private void OnDisable()
        {
            collider.enabled = false;
            //LoseTrackOfAll();
        }
        private void OnDestroy()
        {
            //LoseTrackOfAll();
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
            if (trackable.Count == 0 || trackable.Contains(col.tag))
            {
                tracked.Add(col);
                OnTrack?.Invoke(collider, col);
            }
        }
        private void LoseTrackOf(Collider col)
        {
            if (tracked.Contains(col))
            {
                tracked.Remove(col);
                OnLoseTrackOf?.Invoke(collider, col);
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
        #endregion
    }
}