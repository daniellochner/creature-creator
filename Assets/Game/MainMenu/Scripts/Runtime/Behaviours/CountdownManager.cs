// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CountdownManager : MonoBehaviourSingleton<CountdownManager>
    {
        #region Fields
        [SerializeField] private CountdownUI countdownUI;
        [SerializeField] private UnityEvent onComplete;
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            if (SettingsManager.Instance.ShowTutorial) yield break;

            yield return RemoteConfigUtility.FetchConfigRoutine();

            CountdownData countdownData = JsonUtility.FromJson<CountdownData>(RemoteConfigService.Instance.appConfig.GetJson("countdown_data"));
            if (countdownData.date != -1)
            {
                yield return new WaitUntil(() => WorldTimeManager.Instance.IsInitialized);

                DateTime date = new DateTime(countdownData.date);
                if ((date - (DateTime)WorldTimeManager.Instance.UtcNow).TotalSeconds > 0)
                {
                    countdownUI.Setup(countdownData.title, date, onComplete.Invoke);
                }
                else
                {
                    onComplete.Invoke();
                }
            }
        }
        #endregion

        #region Nested
        public class CountdownData
        {
            public string title;
            public long date;
        }
        #endregion
    }
}