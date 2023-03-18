using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FastTravelReminder : MonoBehaviourSingleton<FastTravelReminder>
    {
        #region Properties
        public bool Remind { get; set; } = true;
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            if (SettingsManager.Data.Map && PlayerPrefs.GetInt("FAST_TRAVEL_REMINDER") == 0)
            {
                yield return new WaitUntil(() => EditorManager.Instance.IsPlaying);
                yield return new WaitForSeconds(1f);

                Show();
                PlayerPrefs.SetInt("FAST_TRAVEL_REMINDER", 1);

                yield return new WaitForSeconds(300f);
                if (Remind)
                {
                    Show();
                }
            }
        }

        private void Show()
        {
            string title = LocalizationUtility.Localize("cc_fast-travel_title");
            string message = LocalizationUtility.Localize("cc_fast-travel_message") + $"_{SystemUtility.DeviceType}".ToLower();
            InformationDialog.Inform(title, message);
        }
        #endregion
    }
}