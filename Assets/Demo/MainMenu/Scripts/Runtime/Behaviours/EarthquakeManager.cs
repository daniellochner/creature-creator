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
        [SerializeField] private GameObject nestGO;
        [SerializeField] private int rumblesToSpawn;
        [SerializeField] private float timeToSpawn;

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

                #region N.E.S.T Easter Egg
                rumbleCount++;
                if ((rumbleCount >= rumblesToSpawn) && (Time.time <= timeToSpawn) && !nestGO.activeSelf)
                {
                    nestGO.SetActive(true);
                }
                #endregion
            }
        }
        #endregion
    }
}