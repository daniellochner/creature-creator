using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TimedManager : MonoBehaviourSingleton<TimedManager>
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private GameObject timerRoot;

        private int time, myBestTime = -1;
        private Coroutine timerCoroutine;

        public List<string> UnlockedBodyParts { get; } = new List<string>();
        public List<string> UnlockedPatterns { get; } = new List<string>();
        public List<string> CompletedQuests { get; } = new List<string>();

        public string LeaderboardId
        {
            get => $"times_{SceneManager.GetActiveScene().name}".ToLower();
        }

        private async void Start()
        {
            if (WorldManager.Instance.IsTimed)
            {
                InformationDialog.Inform(LocalizationUtility.Localize("timed_begin_message"), LocalizationUtility.Localize("timed_begin_message"), LocalizationUtility.Localize("timed_begin_okay"), onOkay: delegate
                {
                    timerRoot.SetActive(true);
                    timerCoroutine = StartCoroutine(TimerRoutine());
                });

                try
                {
                    var myTime = await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
                    myBestTime = (int)myTime.Score;
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
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
        
        public void Complete()
        {
            StopCoroutine(timerCoroutine);
            timerRoot.SetActive(true);

            if (myBestTime == -1 || time < myBestTime)
            {
                myBestTime = time;
            }

            string message = LocalizationUtility.Localize("timed_complete_message", FormatTime(time), FormatTime(myBestTime));
            if (myBestTime == time)
            {
                message = message.ToColour(Color.green);
            }
            InformationDialog.Inform(LocalizationUtility.Localize("timed_complete_title"), message);

            try
            {
                LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, time);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private string FormatTime(int seconds)
        {
            int mins = seconds / 60;
            int secs = seconds % 60;
            return $"{mins:00}:{secs:00}";
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
    }
}