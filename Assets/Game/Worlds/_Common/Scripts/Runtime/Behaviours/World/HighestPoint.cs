using MoreMountains.NiceVibrations;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class HighestPoint : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Map map;
        [SerializeField] private GameObject flag;
        private AudioSource source;

        private bool? hasReached;
        #endregion

        #region Properties
        private bool HasReached
        {
            get
            {
                if (hasReached == null)
                {
                    hasReached = ProgressManager.Data.ReachedPeaks.Contains(map);
                }
                return (bool)hasReached;
            }
            set
            {
                hasReached = value;
            }
        }
        #endregion

        #region Methods
        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }
        private void Start()
        {
            if (HasReached)
            {
                flag.SetActive(false);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player/Local") && !HasReached)
            {
                Reach();
            }
        }

        public void Reach()
        {
            ProgressManager.Data.ReachedPeaks.Add(map);
            ProgressManager.Instance.Save();
            HasReached = true;

            // Flag
            flag.SetActive(false);

            // Stats
#if USE_STATS
            StatsManager.Instance.ReachedPeaks++;
#endif

            // Other
            source.Play();
            MMVibrationManager.Haptic(HapticTypes.Success);
        }
        #endregion
    }
}