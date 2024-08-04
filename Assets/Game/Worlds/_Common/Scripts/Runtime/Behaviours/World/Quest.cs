using MoreMountains.NiceVibrations;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(TrackRegion))]
    [RequireComponent(typeof(AudioSource))]
    public class Quest : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private string id;
        [SerializeField] private string description;
        [SerializeField] private QuestType type;
        [SerializeField] private int experience;
        [SerializeField] private float disappearTime;
        [SerializeField] private QuestItem[] items;
        [SerializeField] public UnityEvent onComplete;
        [Space]
        [SerializeField] private TextMeshProUGUI questText;
        [SerializeField] private LookAtConstraint questLookAtConstraint;
        [SerializeField] private MinimapIcon minimapIcon;

        private TrackRegion region;
        private AudioSource source;
        private bool? hasCompleted;
        #endregion

        #region Properties
        public bool HasCompleted
        {
            get
            {
                if (hasCompleted == null)
                {
                    hasCompleted = ProgressManager.Instance.IsQuestCompleted(id);
                }
                return (bool)hasCompleted;
            }
            set
            {
                hasCompleted = value;
            }
        }

        private bool HasAny
        {
            get
            {
                foreach (QuestItem item in items)
                {
                    if (region.tracked.Contains(item.Holdable.Col))
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
                foreach (QuestItem item in items)
                {
                    if (!region.tracked.Contains(item.Holdable.Col))
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
        private IEnumerator Start()
        {
            yield return new WaitUntilSetup(GameSetup.Instance);

            if (!WorldManager.Instance.IsCreative)
            {
                UpdateInfo();

                minimapIcon.enabled = !HasCompleted;

                yield return new WaitUntil(() => Player.Instance.IsSetup);
                questLookAtConstraint.AddSource(new ConstraintSource() { sourceTransform = Player.Instance.Camera.MainCamera.transform, weight = 1f });
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Quest"))
            {
                UpdateInfo();
            }
            if (other.CompareTag("Player/Local"))
            {
                UpdateInfo();
                questText.gameObject.SetActive(true);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player/Local"))
            {
                if (!HasCompleted && (type == QuestType.All ? HasAll : HasAny))
                {
                    Complete();
                }
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
            if (!HasCompleted)
            {
                int required = (type == QuestType.All) ? items.Length : 1;
                int current = 0;
                foreach (QuestItem item in items)
                {
                    if (region.tracked.Contains(item.Holdable.Col))
                    {
                        current++;
                    }
                }

                questText.text = $"<size=7>{LocalizationUtility.Localize(description)}: ${experience}</size><br><size=14>{TextUtility.FormatError(current, current < required)}/{required}</size>";
            }
            else
            {
                questText.text = "";
            }
        }
        private void Complete()
        {
            ProgressManager.Instance.CompleteQuest(id);
            HasCompleted = true;

            // Items
            foreach (QuestItem item in items)
            {
                item.Snap();
            }

            // Reward
            ProgressManager.Data.Experience += experience;
            ProgressManager.Instance.Save();

            // Stats
            StatsManager.Instance.CompletedQuests++;
            StatsManager.Instance.ExperienceEarned += experience;

            // Other
            NotificationsManager.Notify(LocalizationUtility.Localize("experience-earned", experience));
            source.Play();
            minimapIcon.enabled = false;
            onComplete.Invoke();
            MMVibrationManager.Haptic(HapticTypes.Success);
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