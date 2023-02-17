using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class HighestPoint : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject flag;
        private AudioSource audioSource;
        #endregion

        #region Properties
        private string SceneId => $"HP_{SceneManager.GetActiveScene().name.ToUpper()}";

        private bool Reached
        {
            get => PlayerPrefs.GetInt(SceneId) == 1;
            set => PlayerPrefs.SetInt(SceneId, value ? 1 : 0);
        }
        #endregion

        #region Methods
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        private void Start()
        {
            if (Reached)
            {
                flag.SetActive(false);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player/Local") && !Reached)
            {
#if USE_STATS
                StatsManager.Instance.ReachedPeaks++;
#endif
                Reached = true;
                flag.SetActive(false);

                audioSource.Play();
            }
        }
        #endregion
    }
}