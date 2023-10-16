using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.PlayerAccounts;
using UnityEngine;
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
        [SerializeField] private GameObject errorGO;
        [SerializeField] private GameObject nonErrorGO;
        [Space]
        [SerializeField] private Sprite anonymousIcon;
        [SerializeField] private Sprite unityIcon;
        [Space]
        [SerializeField] private GameObject[] signInSettings;
        [SerializeField] private GameObject[] signOutSettings;

        private string errorMessage;
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
        private void Start()
        {
            if (!SettingsManager.Instance.ShowTutorial)
            {
                Setup();
            }
        }

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
            SetError(null);
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
                else
                {
                    if (string.IsNullOrEmpty(AccessToken))
                    {
                        OnSignedInAnonymously();
                    }
                    else
                    {
                        OnSignedInWithUnity();
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
                SetError(ex.Message);
            }
            catch (RequestFailedException ex) // CommonErrorCodes
            {
                Debug.LogException(ex);
                SetError(ex.Message);
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
                SetError(ex.Message);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
                SetError(ex.Message);
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
                SetError(ex.Message);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
                SetError(ex.Message);
            }
        }
        public async Task LinkWithUnityAsync()
        {
            SetError(null);
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
                // Replace Anonymous account with Unity account

                if (AuthenticationService.Instance.IsSignedIn)
                {
                    AuthenticationService.Instance.SignOut(); // Sign out of Anonymous account
                }
                await SignInAsync();

                Refresh();
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
                SetError(ex.Message);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
                SetError(ex.Message);
            }

            SetRefreshing(false);
        }
        public async void UpdateUsernameAsync(string username)
        {
            SetError(null);
            SetRefreshing(true, "account_status_updating");

            try
            {
                SetUsername(await AuthenticationService.Instance.UpdatePlayerNameAsync(ParseUsername(username)));
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
                SetError(ex.Message);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
                SetError(ex.Message);
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
        public async void TryUpdateUsername()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await SignInAsync();
                Refresh();
            }

            if (errorMessage != null)
            {
                InformationDialog.Inform(LocalizationUtility.Localize("account_status_error"), errorMessage);
            }
            else
            {
                InputDialog.Input(LocalizationUtility.Localize("mainmenu_username_title"), LocalizationUtility.Localize("account_status_enter-a-username"), onSubmit: UpdateUsernameAsync);
            }
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

        private void SetError(string message = null)
        {
            bool isError = message != null;

            if (!isError || errorMessage == null)
            {
                errorMessage = message;
            }

            errorGO.SetActive(isError);
            nonErrorGO.SetActive(!isError);

            if (isError)
            {
                SetUnitySettingsVisibility(false); // Can't be signed in if there is an error
            }
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

            Refresh();
        }

        private async void Refresh()
        {
            FriendsManager.Instance.Initialized = false;
            await FriendsManager.Instance.Initialize();
            await FriendsMenu.Instance.Refresh();
        }
        #endregion
    }
}