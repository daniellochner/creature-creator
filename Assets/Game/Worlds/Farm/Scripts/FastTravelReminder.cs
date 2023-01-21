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
            if (PlayerPrefs.GetInt("FAST_TRAVEL_REMINDER") == 0)
            {
                yield return new WaitUntil(() => EditorManager.Instance.IsPlaying);
                yield return new WaitForSeconds(1f);

                InformationDialog.Inform(LocalizationUtility.Localize("cc_fast-travel_title"), LocalizationUtility.Localize("cc_fast-travel_message"));
                PlayerPrefs.SetInt("FAST_TRAVEL_REMINDER", 1);

                yield return new WaitForSeconds(300f);
                if (Remind)
                {
                    InformationDialog.Inform(LocalizationUtility.Localize("cc_fast-travel_title"), LocalizationUtility.Localize("cc_fast-travel_message"));
                }
            }
        }
        #endregion
    }
}