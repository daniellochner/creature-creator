using MoreMountains.NiceVibrations;
using UnityEngine;

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
                    hasReached = ProgressManager.Instance.IsPeakReached(map);
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
            ProgressManager.Instance.ReachPeak(map);
            HasReached = true;

            // Flag
            flag.SetActive(false);

            // Stats
            StatsManager.Instance.ReachedPeaks++;

            // Other
            source.Play();
            MMVibrationManager.Haptic(HapticTypes.Success);
        }
        #endregion
    }
}