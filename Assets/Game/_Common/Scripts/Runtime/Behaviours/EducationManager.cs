using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace DanielLochner.Assets.CreatureCreator
{
    public class EducationManager : MonoBehaviourSingleton<EducationManager>
    {
        #region Fields
        [SerializeField] private SecretKey eduConnUsername;
        [SerializeField] private SecretKey eduConnPassword;
        #endregion

        #region Properties
        public string InstitutionId
        {
            get => PlayerPrefs.GetString("INSTITUTION_ID");
            set => PlayerPrefs.SetString("INSTITUTION_ID", value);
        }

        public bool IsEducational => Application.version.EndsWith("-edu");

        public bool IsLinked => !string.IsNullOrEmpty(InstitutionId);
        #endregion

        #region Methods
        public IEnumerator LinkRoutine(string institutionId, Action<bool, string> onLinked)
        {
            string username = eduConnUsername.Value;
            string password = eduConnPassword.Value;
            string deviceId = SystemInfo.deviceUniqueIdentifier;

            Regex r = new Regex("^[a-zA-Z0-9]+");
            if (!r.IsMatch(institutionId))
            {
                onLinked(false, "Invalid institution ID...");
                yield break;
            }

            string url = $"https://playcreature.com/api/linkDevice.php?username={username}&password={password}&institutionId={institutionId}&deviceId={deviceId}";
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    onLinked(false, webRequest.error);
                }
                else
                {
                    if (webRequest.responseCode == 201)
                    {
                        InstitutionId = institutionId;
                    }
                    onLinked(IsLinked, webRequest.downloadHandler.text);
                }
            }
        }

        public IEnumerator VerifyRoutine(Action<bool> onVerified)
        {
            string username = eduConnUsername.Value;
            string password = eduConnPassword.Value;
            string deviceId = SystemInfo.deviceUniqueIdentifier;
            string institutionId = InstitutionId;

            if (string.IsNullOrEmpty(institutionId))
            {
                onVerified(false);
            }

            string url = $"https://playcreature.com/api/verifyDevice.php?username={username}&password={password}&institutionId={institutionId}&deviceId={deviceId}";
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    onVerified(false);
                }
                else
                {
                    onVerified(bool.Parse(webRequest.downloadHandler.text));
                }
            }
        }
        #endregion
    }
}