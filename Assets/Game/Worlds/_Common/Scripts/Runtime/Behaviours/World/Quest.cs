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
        [SerializeField] private TextMeshProUGUI questText;
        [SerializeField] private LookAtConstraint questLookAtConstraint;
        [SerializeField] private MinimapIcon minimapIcon;
        [SerializeField] private float disappearTime;
        [SerializeField] private QuestType type;
        [SerializeField] private string description;
        [SerializeField] private string id;
        [SerializeField] private int reward;
        [SerializeField] private QuestItem[] items;
        [SerializeField] public UnityEvent onComplete;

        private TrackRegion region;
        private AudioSource source;
        #endregion

        #region Properties
        public bool IsCompleted
        {
            get => PlayerPrefs.GetInt(id) == 1;
            set => PlayerPrefs.SetInt(id, value ? 1 : 0);
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

            if (!WorldManager.Instance.World.CreativeMode)
            {
                UpdateInfo();
                minimapIcon.enabled = !IsCompleted;

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
                if (!IsCompleted && (type == QuestType.All ? HasAll : HasAny))
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
            if (!IsCompleted)
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

                questText.text = $"<size=7>{LocalizationUtility.Localize(description)}: ${reward}</size><br><size=14>{TextUtility.FormatError(current, current < required)}/{required}</size>";
            }
            else
            {
                questText.text = "";
            }
        }
        private void Complete()
        {
            IsCompleted = true;

            // Items
            foreach (QuestItem item in items)
            {
                item.Snap();
            }

            // Reward
            ProgressManager.Data.Cash += reward;
            ProgressManager.Instance.Save();
            Player.Instance.Editor.Cash += reward;

            // Stats
#if USE_STATS
            StatsManager.Instance.CompletedQuests++;
#endif

            // Other
            NotificationsManager.Notify(LocalizationUtility.Localize("quest-complete", reward));
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