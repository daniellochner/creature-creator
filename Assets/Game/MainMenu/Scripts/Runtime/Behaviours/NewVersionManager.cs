// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using TMPro;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class NewVersionManager : MonoBehaviourSingleton<NewVersionManager>
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI newVersionText;
        #endregion

        #region Properties
        private string StoreURL
        {
            get
            {
#if UNITY_STANDALONE
                return "https://store.steampowered.com/app/1990050/Creature_Creator/";
#elif UNITY_IOS
                return "https://apps.apple.com/us/app/creature-creator/id1564115819";
#elif UNITY_ANDROID
                return "https://play.google.com/store/apps/details?id=com.daniellochner.creature_creator";
#else
                return "https://creaturecreator.daniellochner.com";
#endif
            }
        }
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f);

            if (SettingsManager.Instance.ShowTutorial || EducationManager.Instance.IsEducational) yield break;

            yield return RemoteConfigUtility.FetchConfigRoutine();

            var v1 = VersionUtility.GetVersion(RemoteConfigService.Instance.appConfig.GetString("latest_version"));
            var v2 = VersionUtility.GetVersion(Application.version);
            if (v1.CompareTo(v2) > 0)
            {
                yield return new WaitUntil(() => !RewardsMenu.Instance.IsOpen);

                ConfirmationDialog.Confirm(LocalizationUtility.Localize("new-version_title"), LocalizationUtility.Localize("new-version_message", v1), onYes: () => Application.OpenURL(StoreURL));

                yield return new WaitUntil(() => !ConfirmationDialog.Instance.IsOpen);

                newVersionText.gameObject.SetActive(true);
                newVersionText.SetArguments(v1);
            }
        }
        #endregion
    }
}