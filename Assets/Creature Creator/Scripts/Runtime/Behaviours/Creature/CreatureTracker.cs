using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureTracker : MonoBehaviour
    {
        private CreatureBase creature;

        public Action<CreatureBase> OnTrack { get; set; }
        public Action<CreatureBase> OnLoseTrackOf { get; set; }

        public List<CreatureBase> Tracked { get; set; } = new List<CreatureBase>();

        private void Awake()
        {
            creature = GetComponent<CreatureBase>();
        }

        public void Track(Collider col)
        {
            CreatureBase other = col.GetComponent<CreatureBase>();
            if (other != null && other != creature)
            {
                Tracked.Add(other);
                OnTrack?.Invoke(other);
            }
        }
        public void LoseTrackOf(Collider col)
        {
            CreatureBase other = col.GetComponent<CreatureBase>();
            if (other != null && other != creature)
            {
                Tracked.Remove(other);
                OnLoseTrackOf?.Invoke(other);
            }
        }

        public void IgnoreAll()
        {
            Tracked.Clear();
        }
    }
}