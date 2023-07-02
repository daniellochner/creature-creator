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
        [SerializeField, DrawIf("isFinalMap", false)] private Teleport[] teleports;
        [SerializeField, DrawIf("isFinalMap", false)] private Map nextMap;
        [SerializeField] private bool isFinalMap;
        [Space]
        [SerializeField] private GameObject progress;
        [SerializeField] private TextMeshProUGUI bodyPartsText;
        [SerializeField] private TextMeshProUGUI patternsText;
        [SerializeField] private TextMeshProUGUI questsText;
        [SerializeField] private WorldHint worldHintPrefab;

        private int unlockedBodyParts, unlockedPatterns, completedQuests;
        private bool allowAdvance = false;
        #endregion

        #region Properties
        private int UnlockedBodyParts
        {
            get => unlockedBodyParts;
            set
            {
                unlockedBodyParts = value;
                bodyPartsText.SetArguments(unlockedBodyParts, bodyParts.Length);
                TryAdvance();
            }
        }
        private int UnlockedPatterns
        {
            get => unlockedPatterns;
            set
            {
                unlockedPatterns = value;
                patternsText.SetArguments(unlockedPatterns, patterns.Length);
                TryAdvance();
            }
        }
        private int CompletedQuests
        {
            get => completedQuests;
            set
            {
                completedQuests = value;
                questsText.SetArguments(completedQuests, quests.Length);
                TryAdvance();
            }
        }

        private bool CanAdvance
        {
            get => (bodyParts.Length + patterns.Length + quests.Length) == (UnlockedBodyParts + UnlockedPatterns + CompletedQuests);
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
                    if (quest.IsCompleted)
                    {
                        CompletedQuests++;
                    }
                    else
                    {
                        quest.onComplete.AddListener(() => CompletedQuests++);
                    }
                }

                allowAdvance = true;
            }
        }

        private void TryAdvance()
        {
            if (allowAdvance && CanAdvance)
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