using MoreMountains.NiceVibrations;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class EarthquakeManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Transform creatures;

        [Header("Effects")]
        [SerializeField] private StressReceiver receiver;
        [SerializeField] private float stress;
        [Space]
        [SerializeField] private GameObject impactPrefab;
        [SerializeField] private float impactProbability;
        [SerializeField] private Transform[] impactSpawnPoints;
        [Space]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip rumble;

        [Header("Other")]
        [SerializeField] private GameObject nest;
        [SerializeField] private int targetRumbleCount;

        private int rumbleCount;
        #endregion

        #region Methods
        private void Update()
        {
            if (Input.GetMouseButtonDown(1) && !CanvasUtility.IsPointerOverUI)
            {
                receiver.InduceStress(stress);

                audioSource.PlayOneShot(rumble);
                foreach (Transform spawnPoint in impactSpawnPoints)
                {
                    if (Random.Range(0f, 1f) < impactProbability) Instantiate(impactPrefab, spawnPoint, false);
                }
                foreach (BoxCreature creature in creatures.GetComponentsInChildren<BoxCreature>())
                {
                    creature.ReplaceWithRagdoll();
                }

                rumbleCount++;
                if (rumbleCount >= targetRumbleCount && creatures.childCount == 0 && nest != null && !nest.activeSelf)
                {
                    nest.SetActive(true);
                }

                MMVibrationManager.Haptic(HapticTypes.LightImpact);
            }
        }
        #endregion
    }
}