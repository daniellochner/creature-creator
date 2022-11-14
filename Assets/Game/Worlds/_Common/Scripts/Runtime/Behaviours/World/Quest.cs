using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(TrackRegion))]
    [RequireComponent(typeof(AudioSource))]
    public class Quest : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshPro questText;
        [SerializeField] private float disappearTime;
        [SerializeField] private QuestType type;
        [SerializeField] private string description;
        [SerializeField] private string id;
        [SerializeField] private int reward;
        [SerializeField] private Holdable[] items;
        [SerializeField] private UnityEvent onComplete;

        private TrackRegion region;
        private AudioSource source;
        #endregion

        #region Properties
        private bool IsCompleted
        {
            get => PlayerPrefs.GetInt(id) == 1;
            set => PlayerPrefs.SetInt(id, value ? 1 : 0);
        }

        private bool HasAny
        {
            get
            {
                foreach (Holdable item in items)
                {
                    if (region.tracked.Contains(item.Col))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        private bool HasAll
        {
            get
            {
                foreach (Holdable item in items)
                {
                    if (!region.tracked.Contains(item.Col))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        #endregion

        #region Methods
        private void Awake()
        {
            region = GetComponent<TrackRegion>();
            source = GetComponent<AudioSource>();
        }
        private void Start()
        {
            UpdateInfo();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player/Local") || other.CompareTag("Quest"))
            {
                UpdateInfo();
                questText.GetComponent<LookAtConstraint>().AddSource(new ConstraintSource() { sourceTransform = Camera.main.transform, weight = 1f });
                questText.gameObject.SetActive(true);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (!IsCompleted && other.CompareTag("Player/Local") && (type == QuestType.All ? HasAll : HasAny))
            {
                ProgressManager.Data.Cash += reward;
                ProgressManager.Instance.Save();

                NotificationsManager.Notify($"Quest Complete: \"{description}\"! You earned ${reward}.");
                source.Play();
                onComplete.Invoke();

                IsCompleted = true;

#if USE_STATS
                StatsManager.Instance.CompletedQuests++;
#endif
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player/Local"))
            {
                questText.gameObject.SetActive(false);
            }
        }

        private void UpdateInfo()
        {
            if (!IsCompleted)
            {
                int required = (type == QuestType.All) ? items.Length : 1;
                int current = 0;
                foreach (Holdable item in items)
                {
                    if (region.tracked.Contains(item.Col))
                    {
                        current++;
                    }
                }

                questText.text = $"{description} ({current}/{required})";
            }
            else
            {
                questText.text = "";
            }
        }
        #endregion

        #region Nested
        public enum QuestType
        {
            All,
            Any
        }
        #endregion
    }
}