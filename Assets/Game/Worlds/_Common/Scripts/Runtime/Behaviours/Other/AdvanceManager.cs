using TMPro;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AdvanceManager : MonoBehaviourSingleton<AdvanceManager>
    {
        #region Fields
        [SerializeField] private UnlockableBodyPart[] bodyParts;
        [SerializeField] private UnlockablePattern[] patterns;
        [SerializeField] private Quest[] quests;
        [SerializeField] private Battle[] battles;
        [SerializeField] private Teleport[] teleports;
        [SerializeField] private Map nextMap = Map.ComingSoon;
        [Space]
        [SerializeField] private GameObject progress;
        [SerializeField] private TextMeshProUGUI bodyPartsText;
        [SerializeField] private TextMeshProUGUI patternsText;
        [SerializeField] private TextMeshProUGUI questsText;
        [SerializeField] private WorldHint worldHintPrefab;

        private int unlockedBodyParts, unlockedPatterns, completedQuests;
        private bool isAllowedToAdvance = false;
        #endregion

        #region Properties
        public int BodyParts => bodyParts.Length;
        public int Patterns => patterns.Length;
        public int Quests => quests.Length;

        public int UnlockedBodyParts
        {
            get => unlockedBodyParts;
            set
            {
                unlockedBodyParts = value;
                bodyPartsText.SetArguments(unlockedBodyParts, bodyParts.Length);
                TryAdvance();
            }
        }
        public int UnlockedPatterns
        {
            get => unlockedPatterns;
            set
            {
                unlockedPatterns = value;
                patternsText.SetArguments(unlockedPatterns, patterns.Length);
                TryAdvance();
            }
        }
        public int CompletedQuests
        {
            get => completedQuests;
            set
            {
                completedQuests = value;
                questsText.SetArguments(completedQuests, quests.Length);
                TryAdvance();
            }
        }

        public bool CanAdvance
        {
            get => (nextMap != Map.ComingSoon) && (bodyParts.Length + patterns.Length + quests.Length) == (UnlockedBodyParts + UnlockedPatterns + CompletedQuests);
        }
        #endregion

        #region Methods
        private void Start()
        {
            if (!WorldManager.Instance.World.CreativeMode)
            {
                progress.SetActive(true);

                // Body Parts
                UnlockedBodyParts = 0;
                foreach (UnlockableBodyPart bodyPart in bodyParts)
                {
                    if (bodyPart.IsUnlocked)
                    {
                        UnlockedBodyParts++;
                    }
                    else
                    {
                        bodyPart.onUnlock.AddListener(() => UnlockedBodyParts++);
                    }
                }

                // Patterns
                UnlockedPatterns = 0;
                foreach (UnlockablePattern pattern in patterns)
                {
                    if (pattern.IsUnlocked)
                    {
                        UnlockedPatterns++;
                    }
                    else
                    {
                        pattern.onUnlock.AddListener(() => UnlockedPatterns++);
                    }
                }

                // Quests
                CompletedQuests = 0;
                foreach (Quest quest in quests)
                {
                    if (quest.HasCompleted)
                    {
                        CompletedQuests++;
                    }
                    else
                    {
                        quest.onComplete.AddListener(() => CompletedQuests++);
                    }
                }

                isAllowedToAdvance = true;
            }
        }

        private void TryAdvance()
        {
            if (isAllowedToAdvance && CanAdvance)
            {
                InformationDialog.Inform(LocalizationUtility.Localize("cc_ready-to-advance_title"), LocalizationUtility.Localize("cc_ready-to-advance_message"), onOkay: delegate
                {
                    foreach (Teleport teleport in teleports)
                    {
                        teleport.GetComponent<MinimapIcon>().MinimapIconUI.gameObject.AddComponent<BlinkingGraphic>();
                        Instantiate(worldHintPrefab, teleport.transform, false).transform.localScale *= 3f;
                    }
                });
                ProgressManager.Instance.UnlockMap(nextMap);
            }
        }
        #endregion
    }
}