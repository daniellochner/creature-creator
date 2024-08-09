using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TimedManager : MonoBehaviourSingleton<TimedManager>
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private GameObject timerRoot;

        private int time;
        private Coroutine timerCoroutine;

        public List<string> UnlockedBodyParts { get; } = new();
        public List<string> UnlockedPatterns { get; } = new();
        public List<string> CompletedQuests { get; } = new();

        public string BestTimeId
        {
            get => $"best_time_{SceneManager.GetActiveScene().name}".ToUpper();
        }
        public int BestTime
        {
            get => PlayerPrefs.GetInt(BestTimeId, -1);
            set => PlayerPrefs.SetInt(BestTimeId, value);
        }


        private void Start()
        {
            if (WorldManager.Instance.IsTimed)
            {
                timerRoot.SetActive(true);
                timerCoroutine = StartCoroutine(TimerRoutine());
            }
        }

        private IEnumerator TimerRoutine()
        {
            while (true)
            {
                timerText.text = FormatTime(time);

                yield return new WaitForSecondsRealtime(1f);
                time++;
            }
        }

        public void UnlockBodyPart(string bodyPartId)
        {
            if (!IsBodyPartUnlocked(bodyPartId))
            {
                UnlockedBodyParts.Add(bodyPartId);
            }
        }
        public bool IsBodyPartUnlocked(string bodyPartId)
        {
            return UnlockedBodyParts.Contains(bodyPartId);
        }

        public void UnlockPattern(string patternId)
        {
            if (!IsPatternUnlocked(patternId))
            {
                UnlockedPatterns.Add(patternId);
            }
        }
        public bool IsPatternUnlocked(string patternId)
        {
            return UnlockedPatterns.Contains(patternId);
        }

        public void CompleteQuest(string questId)
        {
            if (!IsQuestCompleted(questId))
            {
                CompletedQuests.Add(questId);
            }
        }
        public bool IsQuestCompleted(string questId)
        {
            return CompletedQuests.Contains(questId);
        }


        public void RecordTime()
        {
            timerRoot.SetActive(true);
            StopCoroutine(timerCoroutine);

            if (BestTime == -1 || time < BestTime)
            {
                BestTime = time;
            }

            string title = LocalizationUtility.Localize("timed_complete_title");
            string message = LocalizationUtility.Localize("timed_complete_message", FormatTime(time), FormatTime(BestTime));
            if (BestTime == time)
            {
                message = message.ToColour(Color.green);
            }
            InformationDialog.Inform(title, message);;
        }

        private string FormatTime(int seconds)
        {
            int mins = seconds / 60;
            int secs = seconds % 60;
            return $"{mins:00}:{secs:00}";
        }
    }
}