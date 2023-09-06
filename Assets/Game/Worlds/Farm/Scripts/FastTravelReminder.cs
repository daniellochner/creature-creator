using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FastTravelReminder : MonoBehaviourSingleton<FastTravelReminder>
    {
        #region Properties
        public bool HasReminded
        {
            get => PlayerPrefs.GetInt("FAST_TRAVEL_REMINDER", 0) == 1;
            set => PlayerPrefs.SetInt("FAST_TRAVEL_REMINDER", value ? 1 : 0);
        }

        public bool Remind { get; set; } = true;
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            if (SettingsManager.Data.Map && !HasReminded)
            {
                yield return new WaitUntil(() => EditorManager.Instance.IsPlaying);
                yield return new WaitForSeconds(1f);

                HasReminded = true;

                while (Remind)
                {
                    string title = LocalizationUtility.Localize("cc_fast-travel_title");
                    string message = LocalizationUtility.Localize($"cc_fast-travel_message_{SystemUtility.DeviceType}".ToLower());
                    InformationDialog.Inform(title, message);

                    yield return new WaitForSeconds(300f);
                }
            }
        }
        #endregion
    }
}