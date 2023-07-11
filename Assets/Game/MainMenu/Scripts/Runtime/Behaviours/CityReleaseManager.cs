using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CityReleaseManager : MonoBehaviourSingleton<CityReleaseManager>
    {
        #region Fields
        [SerializeField] private CountdownUI countdownUI;

        [SerializeField] private BodyPart[] bodyParts;
        [SerializeField] private Pattern[] patterns;
        #endregion

        #region Properties
        public static bool IsCityReleased { get; private set; } = false;
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            if (SettingsManager.Instance.ShowTutorial) yield break;

            yield return new WaitUntil(() => WorldTimeManager.Instance.IsInitialized);

            Task task = RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
            yield return new WaitUntil(() => task.IsCompleted);

            DateTime releaseDateTime = new DateTime(RemoteConfigService.Instance.appConfig.GetLong("city_release_date"));
            if ((releaseDateTime - (DateTime)WorldTimeManager.Instance.UtcNow).TotalSeconds > 0)
            {
                countdownUI.Setup(releaseDateTime, OnComplete);
            }
            else
            {
                OnComplete();
            }
        }

        private void OnComplete()
        {
            foreach (BodyPart bodyPart in bodyParts)
            {
                bodyPart.released = true;
            }
            foreach (Pattern pattern in patterns)
            {
                pattern.released = true;
            }
            IsCityReleased = true;
        }
        #endregion

        #region Nested
        private struct UserAttributes { }
        private struct AppAttributes { }
        #endregion
    }
}