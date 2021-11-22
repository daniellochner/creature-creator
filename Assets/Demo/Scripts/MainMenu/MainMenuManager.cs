// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Net;
using ProfanityDetector;
using IngameDebugConsole;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MainMenuManager : MonoBehaviourSingleton<MainMenuManager>
    {
        #region Fields
        [Header("Play")]
        [SerializeField] private TMP_Dropdown roleDropdown;
        [SerializeField] private TMP_Dropdown connectionDropdown;
        [SerializeField] private TMP_InputField worldNameInputField;
        [SerializeField] private TMP_InputField ipAddressInputField;
        [SerializeField] private TMP_InputField portInputField;
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private TextMeshProUGUI networkStatusText;
        [SerializeField] private TextMeshProUGUI joinCreateText;
        [SerializeField] private BlinkingText networkStatusBT;
        [SerializeField] private Menu multiplayerHintMenu;
        [SerializeField] private GameObject relayConnectionGO;
        [SerializeField] private GameObject ipConnectionGO;

        [Header("Settings")]
        [SerializeField] private TMP_Dropdown displaySizeDropdown;
        [SerializeField] private TMP_Dropdown qualityLevelDropdown;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundEffectsSlider;
        [SerializeField] private Toggle debugModeToggle;
        [SerializeField] private Toggle hideChatToggle;
        [SerializeField] private Toggle previewFeaturesToggle;
        [SerializeField] private AudioMixer masterAudioMixer;

        private Coroutine updateNetStatusCoroutine, loadMultiplayerCoroutine;
        private ProfanityFilter filter = new ProfanityFilter();
        private bool isConnecting;
        #endregion

        #region Properties
        public bool IsConnecting
        {
            get => isConnecting;
            set
            {
                isConnecting = value;

                if (isConnecting)
                {
                    joinCreateText.text = "Cancel";
                }
                else
                {
                    joinCreateText.text = (roleDropdown.value == 0) ? "Join" : "Create";
                }
            }
        }

        private ConnectionType Connection => (ConnectionType)connectionDropdown.value;
        private RoleType Role => (RoleType)roleDropdown.value;
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }
        private void OnDestroy()
        {
            if (!NetworkManager.Singleton) return;

            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnect;
        }

        private void Setup()
        {
            SetupSettings();
            SetupMultiplayer();
        }
        private void SetupSettings()
        {
            displaySizeDropdown.onValueChanged.AddListener(delegate (int displaySize)
            {
                Screen.SetResolution(DemoManager.Instance.DisplaySize.x, DemoManager.Instance.DisplaySize.y, displaySize == 0);
                DemoManager.Instance.Data.DisplaySize = displaySize;
                DemoManager.Instance.Save();
            });
            displaySizeDropdown.value = DemoManager.Instance.Data.DisplaySize;

            qualityLevelDropdown.onValueChanged.AddListener(delegate (int qualityLevel)
            {
                QualitySettings.SetQualityLevel(qualityLevel);
                DemoManager.Instance.Data.QualityLevel = qualityLevel;
                DemoManager.Instance.Save();
            });
            qualityLevelDropdown.value = DemoManager.Instance.Data.QualityLevel;

            musicSlider.onValueChanged.AddListener(delegate (float musicVolume)
            {
                masterAudioMixer.SetFloat("MusicVolume", Mathf.Lerp(DemoManager.Instance.MinMaxMusicDB.min, DemoManager.Instance.MinMaxMusicDB.max, musicVolume));
                DemoManager.Instance.Data.MusicVolume = musicVolume;
                DemoManager.Instance.Save();
            });
            musicSlider.value = DemoManager.Instance.Data.MusicVolume;

            soundEffectsSlider.onValueChanged.AddListener(delegate (float soundEffectsVolume)
            {
                masterAudioMixer.SetFloat("SoundEffectsVolume", Mathf.Lerp(DemoManager.Instance.MinMaxSoundEffectsDB.min, DemoManager.Instance.MinMaxSoundEffectsDB.max, soundEffectsVolume));
                DemoManager.Instance.Data.SoundEffectsVolume = soundEffectsVolume;
                DemoManager.Instance.Save();
            });
            soundEffectsSlider.value = DemoManager.Instance.Data.SoundEffectsVolume;

            debugModeToggle.onValueChanged.AddListener(delegate (bool debugMode)
            {
                DebugLogManager.Instance.gameObject.SetActive(debugMode);

                DemoManager.Instance.Data.DebugMode = debugMode;
                DemoManager.Instance.Save();
            });
            debugModeToggle.isOn = DemoManager.Instance.Data.DebugMode;

            hideChatToggle.onValueChanged.AddListener(delegate (bool hideChat)
            {
                DemoManager.Instance.Data.HideChat = hideChat;
                DemoManager.Instance.Save();
            });
            hideChatToggle.isOn = DemoManager.Instance.Data.HideChat;

            previewFeaturesToggle.onValueChanged.AddListener(delegate (bool previewFeatures)
            {
                DemoManager.Instance.Data.PreviewFeatures = previewFeatures;
                DemoManager.Instance.Save();
            });
            previewFeaturesToggle.isOn = DemoManager.Instance.Data.PreviewFeatures;
        }
        private void SetupMultiplayer()
        {
            roleDropdown.onValueChanged.AddListener((role) =>
            {
                switch ((RoleType)role)
                {
                    case RoleType.Client:
                        joinCreateText.text = "Join";
                        break;
                    case RoleType.Host:
                        joinCreateText.text = "Create";
                        break;
                }
            });
            connectionDropdown.onValueChanged.AddListener((connection) =>
            {
                switch ((ConnectionType)connection)
                {
                    case ConnectionType.Relay:
                        relayConnectionGO.SetActive(true);
                        ipConnectionGO.SetActive(false);
                        break;
                    case ConnectionType.Ip:
                        relayConnectionGO.SetActive(false);
                        ipConnectionGO.SetActive(true);
                        break;
                }
            });

            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
        }

        private void OnServerStarted()
        {
            OnMultiplayerSuccess("Created.");
        }
        private void OnClientDisconnect(ulong clientID)
        {
            UpdateNetworkStatus("Connection failed.", Color.red);
            IsConnecting = false;
        }
        private void OnClientConnect(ulong clientID)
        {
            OnMultiplayerSuccess("Connected.");
        }
        private void OnMultiplayerSuccess(string message)
        {
            UpdateNetworkStatus(message, Color.green);
            NetworkManager.Singleton.SceneManager.LoadScene("Multiplayer", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        private void UpdateNetworkStatus(string status, Color color, float duration = 5)
        {
            if (updateNetStatusCoroutine != null)
            {
                StopCoroutine(updateNetStatusCoroutine);
            }

            networkStatusText.CrossFadeAlpha(0f, 0f, true);
            networkStatusText.CrossFadeAlpha(1f, 0.25f, true);
            networkStatusText.text = status;
            networkStatusText.color = color;
            networkStatusBT.IsBlinking = false;

            if (duration == -1)
            {
                networkStatusBT.IsBlinking = true;
            }
            else
            {
                updateNetStatusCoroutine = this.Invoke(HideNetworkStatus, duration);
            }
        }
        private void HideNetworkStatus()
        {
            networkStatusText.CrossFadeAlpha(0, 0.25f, true);
        }

        private bool IsValidInput()
        {
            UpdateNetworkStatus("Validating User Input...", Color.yellow, -1);

            // Player
            string usernameText = usernameInputField.text;
            if (string.IsNullOrEmpty(usernameText))
            {
                UpdateNetworkStatus("A username must be provided.", Color.white);
                return false;
            }
            if (filter.ContainsProfanity(usernameText))
            {
                UpdateNetworkStatus("Profanity detected in username.", Color.white);
                return false;
            }
            if (usernameText.Length > 16)
            {
                UpdateNetworkStatus("Username cannot be longer than 16 characters.", Color.white);
                return false;
            }
            PlayerData playerData = new PlayerData()
            {
                username = usernameText
            };
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(playerData));

            // Connection
            switch (Connection)
            {
                case ConnectionType.Relay:
                    UnityTransport relayTransport = NetworkTransportPicker.Instance.GetTransport<UnityTransport>("Relay");

                    // Internet Connection
                    if (Application.internetReachability == NetworkReachability.NotReachable)
                    {
                        UpdateNetworkStatus("You are not connected to the internet.", Color.white);
                        return false;
                    }

                    string worldNameText = worldNameInputField.text;

                    // World Name
                    if (string.IsNullOrEmpty(worldNameText))
                    {
                        UpdateNetworkStatus("A world name must be provided.", Color.white);
                        return false;
                    }
                    if (filter.ContainsProfanity(worldNameText))
                    {
                        UpdateNetworkStatus("Profanity detected in world name.", Color.white);
                        return false;
                    }

                    NetworkManager.Singleton.NetworkConfig.NetworkTransport = relayTransport;
                    break;

                case ConnectionType.Ip:
                    UnityTransport ipTransport = NetworkTransportPicker.Instance.GetTransport<UnityTransport>("IP");

                    string ipAddressText = ipAddressInputField.text;
                    string portText = portInputField.text;

                    // IP Address
                    if (string.IsNullOrEmpty(ipAddressText))
                    {
                        ipAddressText = "127.0.0.1";
                    }
                    if (!IPAddress.TryParse(ipAddressText, out IPAddress address))
                    {
                        UpdateNetworkStatus("The address must be in the correct format.", Color.white);
                        return false;
                    }

                    // Port
                    if (string.IsNullOrEmpty(portText))
                    {
                        portText = "1337";
                    }
                    portText.Replace(",", "");
                    if (!int.TryParse(portText, out int port) || port < 0 || port > 65353)
                    {
                        UpdateNetworkStatus("The port must be a number between 0 and 65,353.", Color.white);
                        return false;
                    }

                    ipTransport.SetConnectionData(ipAddressText, (ushort)port);

                    NetworkManager.Singleton.NetworkConfig.NetworkTransport = ipTransport;
                    break;
            }

            UpdateNetworkStatus("Validated User Input.", Color.green);

            return true;
        }
        private async void ConnectUsingInput()
        {
            IsConnecting = true;

            if (Connection == ConnectionType.Relay)
            {
                // Authenticate
                UpdateNetworkStatus("Authenticating...", Color.yellow, -1);
                await UnityServices.InitializeAsync();
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }

                // Allocate
                UnityTransport relayTransport = NetworkTransportPicker.Instance.GetTransport<UnityTransport>("Relay");
                switch (Role)
                {
                    case RoleType.Client:
                        JoinAllocation join = await Relay.Instance.JoinAllocationAsync(worldNameInputField.text);
                        relayTransport.SetRelayServerData(join.RelayServer.IpV4, (ushort)join.RelayServer.Port, join.AllocationIdBytes, join.Key, join.ConnectionData, join.HostConnectionData);
                        break;

                    case RoleType.Host:
                        UpdateNetworkStatus("Allocating...", Color.yellow, -1);
                        Allocation host = await Relay.Instance.CreateAllocationAsync(10);
                        UpdateNetworkStatus("Generating Join Code...", Color.yellow, -1);
                        string joinCode = await Relay.Instance.GetJoinCodeAsync(host.AllocationId);
                        Debug.Log(joinCode);
                        relayTransport.SetRelayServerData(host.RelayServer.IpV4, (ushort)host.RelayServer.Port, host.AllocationIdBytes, host.Key, host.ConnectionData);
                        break;
                }
            }

            // Start
            switch (Role)
            {
                case RoleType.Client:
                    UpdateNetworkStatus("Starting Client...", Color.yellow, -1);
                    NetworkManager.Singleton.StartClient();
                    break;
                case RoleType.Host:
                    UpdateNetworkStatus("Starting Host...", Color.yellow, -1);
                    NetworkManager.Singleton.StartHost();
                    break;
            }
        }

        public void PlaySingleplayer()
        {
            SceneManager.LoadScene("Singleplayer");
        }
        public void PlayMultiplayer()
        {
            // Cancel
            if (IsConnecting && !LoadingManager.IsLoading)
            {
                if (loadMultiplayerCoroutine != null) StopCoroutine(loadMultiplayerCoroutine);
                if (updateNetStatusCoroutine != null) StopCoroutine(updateNetStatusCoroutine);

                NetworkShutdownManager.Instance.Shutdown();
                HideNetworkStatus();
                IsConnecting = false;

                return;
            }

            // Connect
            if (IsValidInput())
            {
                ConnectUsingInput();
            }
        }
        public void Quit()
        {
            ConfirmationDialog.Confirm("Quit", "Are you sure you want to exit this application?", yesEvent: Application.Quit);
        }
        public void ResetData()
        {
            ConfirmationDialog.Confirm("Reset Data", "Are you sure your want to reset your data? (Note: This will not remove your creatures).", noEvent: DemoManager.Instance.Data.Revert);
        }

        public void SubscribeToYouTubeChannel()
        {
            Application.OpenURL("https://www.youtube.com/channel/UCGLR3v7NaV1t92dnzWZNSKA?sub_confirmation=1");
        }
        public void FollowTwitterAccount()
        {
            Application.OpenURL("https://twitter.com/daniellochner");
        }
        public void JoinDiscordServer()
        {
            Application.OpenURL("https://discord.gg/sJysbdu");
        }
        public void ViewGitHubSourceCode()
        {
            Application.OpenURL("https://github.com/daniellochner/Creature-Creator");
        }
        #endregion

        #region Enums
        private enum ConnectionType
        {
            Relay = 0,
            Ip = 1
        }
        private enum RoleType
        {
            Client = 0,
            Host = 1
        }
        #endregion
    }
}