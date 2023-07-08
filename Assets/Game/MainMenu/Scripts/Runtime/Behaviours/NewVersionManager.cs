using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class NewVersionManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI newUpdateText;
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
            Task task = RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
            yield return new WaitUntil(() => task.IsCompleted);

            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => !RewardsMenu.Instance.IsOpen);

            var v1 = GetVersion(RemoteConfigService.Instance.appConfig.GetString("latest_version"));
            var v2 = GetVersion(Application.version);
            if (v1.CompareTo(v2) > 0)
            {
                ConfirmationDialog.Confirm(LocalizationUtility.Localize("new-version_title"), LocalizationUtility.Localize("new-version_message", v1), onYes: () => Application.OpenURL(StoreURL));

                yield return new WaitUntil(() => !ConfirmationDialog.Instance.IsOpen);

                newUpdateText.gameObject.SetActive(true);
                newUpdateText.SetArguments(v1);
            }
        }

        #region Helper
        private System.Version GetVersion(string version)
        {
            return new System.Version(version.Replace("-beta", ""));
        }
        #endregion
        #endregion

        #region Nested
        private struct UserAttributes { }
        private struct AppAttributes { }
        #endregion
    }
}