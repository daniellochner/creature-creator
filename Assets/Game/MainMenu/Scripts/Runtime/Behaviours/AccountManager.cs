using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.PlayerAccounts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AccountManager : MonoBehaviourSingleton<AccountManager>
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI usernameText;
        [SerializeField] private TextMeshProUGUI placeholderText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Image refreshImg;
        [SerializeField] private Image identityProviderImg;
        [Space]
        [SerializeField] private Sprite anonymousIcon;
        [SerializeField] private Sprite unityIcon;
        [SerializeField] private Sprite errorIcon;
        [Space]
        [SerializeField] private GameObject[] signInSettings;
        [SerializeField] private GameObject[] signOutSettings;
        #endregion

        #region Properties
        private bool HasCheckedUsername
        {
            get => PlayerPrefs.GetInt("HAS_CHECKED_USERNAME", 0) == 1;
            set => PlayerPrefs.SetInt("HAS_CHECKED_USERNAME", value ? 1 : 0);
        }

        private string AccessToken
        {
            get => PlayerPrefs.GetString("ACCESS_TOKEN");
            set => PlayerPrefs.SetString("ACCESS_TOKEN", value);
        }
        #endregion

        #region Methods
        public async void Setup()
        {
            string username = ParseUsername(SettingsManager.Data.OnlineUsername);
            if (!HasCheckedUsername && !string.IsNullOrEmpty(username))
            {
                await SignInAsync(username);
            }
            else
            {
                await SignInAsync();
            }
            HasCheckedUsername = true;

            OnSetup();
        }

        public async Task SignInAsync(string username = "")
        {
            SetRefreshing(true, "account_status_signing-in");

            try
            {
                await UnityServices.InitializeAsync();

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    if (string.IsNullOrEmpty(AccessToken))
                    {
                        await SignInAnonymouslyAsync();
                    }
                    else
                    {
                        await SignInWithUnityAsync();
                    }
                }

                if (!string.IsNullOrEmpty(username))
                {
                    SetUsername(await AuthenticationService.Instance.UpdatePlayerNameAsync(ParseUsername(username)));
                }
                else
                if (!string.IsNullOrEmpty(SettingsManager.Data.OnlineUsername))
                {
                    SetUsername(await AuthenticationService.Instance.GetPlayerNameAsync());
                }
            }
            catch (AuthenticationException ex) // AuthenticationErrorCodes
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex) // CommonErrorCodes
            {
                Debug.LogException(ex);
            }

            SetRefreshing(false);
        }
        public async Task SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                OnSignedInAnonymously();
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }
        public async Task SignInWithUnityAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUnityAsync(AccessToken);

                OnSignedInWithUnity();
            }
            catch (AuthenticationException ex)
            {
                AccessToken = null;

                await SignInAnonymouslyAsync();

                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }
        public async Task LinkWithUnityAsync()
        {
            SetRefreshing(true, "account_status_linking");

            try
            {
                await PlayerAccountService.Instance.StartSignInAsync();
                await UniTask.WaitUntil(() => PlayerAccountService.Instance.IsSignedIn); // Wait until player is signed in with Unity account

                AccessToken = PlayerAccountService.Instance.AccessToken;

                await AuthenticationService.Instance.LinkWithUnityAsync(AccessToken);

                OnLinkedWithUnity();
            }
            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                if (AuthenticationService.Instance.IsSignedIn)
                {
                    AuthenticationService.Instance.SignOut(); // Sign out of Anonymous account
                }
                await SignInAsync();

                FriendsMenu.Instance.Refresh();
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
            
            SetRefreshing(false);
        }
        public async void UpdateUsernameAsync(string username)
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await SignInAsync();
            }

            SetRefreshing(true, "account_status_updating");

            try
            {
                SetUsername(await AuthenticationService.Instance.UpdatePlayerNameAsync(ParseUsername(username)));
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }

            SetRefreshing(false);
        }

        public async void TrySignInWithUnity()
        {
            await LinkWithUnityAsync();
        }
        public void TrySignOutOfUnity()
        {
            AccessToken = null;

            PlayerAccountService.Instance.SignOut();

            OnSignedOutOfUnity();
        }
        public void TryDeleteAccount()
        {
            Application.OpenURL("https://player-account.unity.com/delete-account");
            TrySignOutOfUnity();
        }
        public void TryUpdateUsername()
        {
            InputDialog.Input(LocalizationUtility.Localize("mainmenu_username_title"), LocalizationUtility.Localize("account_status_enter-a-username"), onSubmit: UpdateUsernameAsync);
        }

        private string ParseUsername(string username)
        {
            username = username.Replace(" ", "");

            if (string.IsNullOrEmpty(username))
            {
                username = "Player";
            }

            return username;
        }

        private void SetRefreshing(bool isRefreshing, string status = null)
        {
            identityProviderImg.gameObject.SetActive(!isRefreshing);
            refreshImg.gameObject.SetActive(isRefreshing);

            usernameText.transform.parent.gameObject.SetActive(!isRefreshing);
            statusText.gameObject.SetActive(isRefreshing);

            if (!string.IsNullOrEmpty(status))
            {
                statusText.text = LocalizationUtility.Localize(status);
            }
            else
            {
                statusText.text = null;
            }
        }
        private void SetUsername(string username)
        {
            usernameText.text = username;

            bool isEmpty = string.IsNullOrEmpty(username);
            usernameText.gameObject.SetActive(!isEmpty);
            placeholderText.gameObject.SetActive(isEmpty);

            if (!isEmpty && username.Contains("#"))
            {
                SettingsManager.Data.OnlineUsername = username.Substring(0, username.LastIndexOf("#"));
            }
        }
        private void SetUnitySettingsVisibility(bool isSignedIn)
        {
            foreach (GameObject setting in signInSettings)
            {
                setting.SetActive(!isSignedIn);
            }
            foreach (GameObject setting in signOutSettings)
            {
                setting.SetActive(isSignedIn);
            }
        }

        private void OnSetup()
        {
            FriendsMenu.Instance.Setup();

            // TODO: Progress, Creatures
        }
        private void OnSignedInAnonymously()
        {
            SetUnitySettingsVisibility(false);
            identityProviderImg.sprite = anonymousIcon;
        }
        private void OnSignedInWithUnity()
        {
            SetUnitySettingsVisibility(true);
            identityProviderImg.sprite = unityIcon;
        }
        private void OnLinkedWithUnity()
        {
            SetUnitySettingsVisibility(true);
            identityProviderImg.sprite = unityIcon;
        }
        private void OnSignedOutOfUnity()
        {
            SetUnitySettingsVisibility(false);
            identityProviderImg.sprite = anonymousIcon;
        }
        #endregion
    }
}