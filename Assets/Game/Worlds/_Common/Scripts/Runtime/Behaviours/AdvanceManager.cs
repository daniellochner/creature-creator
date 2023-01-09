using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AdvanceManager : MonoBehaviourSingleton<AdvanceManager>
    {
        #region Fields
        [SerializeField] private UnlockableBodyPart[] bodyParts;
        [SerializeField] private UnlockablePattern[] patterns;
        [SerializeField] private Quest[] quests;
        [Space]
        [SerializeField] private GameObject progress;
        [SerializeField] private TextMeshProUGUI bodyPartsText;
        [SerializeField] private TextMeshProUGUI patternsText;
        [SerializeField] private TextMeshProUGUI questsText;

        private int unlockedBodyParts, unlockedPatterns, completedQuests;
        private bool allowAdvance = false;
        #endregion

        #region Properties
        private int UnlockedBodyParts
        {
            get => unlockedBodyParts;
            set
            {
                bodyPartsText.text = $"Body Parts: {unlockedBodyParts = value}/{bodyParts.Length}";
                TryAdvance();
            }
        }
        private int UnlockedPatterns
        {
            get => unlockedPatterns;
            set
            {
                patternsText.text = $"Patterns: {unlockedPatterns = value}/{patterns.Length}";
                TryAdvance();
            }
        }
        private int CompletedQuests
        {
            get => completedQuests;
            set
            {
                questsText.text = $"Quests: {completedQuests = value}/{quests.Length}";
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

                progress.SetActive(true);
                allowAdvance = true;
            }
        }

        private void TryAdvance()
        {
            if (allowAdvance && CanAdvance)
            {
                ConfirmationDialog.Confirm("Ready To Advance?", "You've unlocked all the parts and patterns, and completed all quests on this map! When you're ready, head back to the main menu and create a new world on a different map.", "Let's Go!", "Not Yet...", onYes: delegate
                {
                    PauseMenu.Instance.Leave();
                });
            }
        }
        #endregion
    }
}