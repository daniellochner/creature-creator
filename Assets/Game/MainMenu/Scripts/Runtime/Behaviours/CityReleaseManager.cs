using System;
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
        private void Start()
        {
            if (SettingsManager.Instance.ShowTutorial) return;

            DateTime releaseDateTime = new DateTime(2023, 7, 21);
            if ((releaseDateTime - DateTime.UtcNow).Seconds > 0)
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
    }
}