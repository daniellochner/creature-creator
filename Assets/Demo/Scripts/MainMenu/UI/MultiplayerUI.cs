// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using ProfanityDetector;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using System.Collections;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MultiplayerUI : MonoBehaviour
    {
        #region Fields
        [Header("Join")]
        [SerializeField] private RectTransform joinOffsetRT;
        [SerializeField] private WorldUI worldUIPrefab;
        [SerializeField] private RectTransform worldsRT;
        [SerializeField] private GameObject noneGO;
        [SerializeField] private GameObject refreshGO;

        [Header("Host")]
        [SerializeField] private TMP_InputField worldNameInputField;
        [SerializeField] private TMP_InputField onlineUsernameInputField;
        [SerializeField] private OptionSelector mapOS;
        [SerializeField] private OptionSelector visibilityOS;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private GameObject passwordGO;
        [SerializeField] private Slider maxPlayersSlider;
        
        [Header("General")]
        [SerializeField] private Menu multiplayerHintMenu;
        [SerializeField] private TextMeshProUGUI networkStatusText;
        [SerializeField] private BlinkingText networkStatusBT;
        [SerializeField] private GameObject cancelGO;
        [SerializeField] private GameObject createGO;
        [SerializeField] private Menu multiplayerMenu;

        private Coroutine updateNetStatusCoroutine;
        private ProfanityFilter filter = new ProfanityFilter();
        private bool isConnecting;
        #endregion

        #region Properties
        public bool IsConnectedToInternet
        {
            get
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    UpdateNetworkStatus("You are not connected to the internet.", Color.white);
                    return false;
                }
                return true;
            }
        }
        public bool IsValidPlayer
        {
            get
            {
                string username = onlineUsernameInputField.text;
                if (string.IsNullOrEmpty(username))
                {
                    UpdateNetworkStatus("A username must be provided.", Color.white);
                    return false;
                }
                if (filter.ContainsProfanity(username))
                {
                    UpdateNetworkStatus("Profanity detected in username.", Color.white);
                    return false;
                }
                if (username.Length > 16)
                {
                    UpdateNetworkStatus("Username cannot be longer than 16 characters.", Color.white);
                    return false;
                }
                PlayerData playerData = new PlayerData()
                {
                    username = username
                };
                NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(playerData));
                return true;
            }
        }
        public bool IsValidWorldName
        {
            get
            {
                string worldName = worldNameInputField.text;
                if (string.IsNullOrEmpty(worldName))
                {
                    UpdateNetworkStatus("A world name must be provided.", Color.white);
                    return false;
                }
                if (filter.ContainsProfanity(worldName))
                {
                    UpdateNetworkStatus("Profanity detected in world name.", Color.white);
                    return false;
                }
                return true;
            }
        }

        private bool IsConnecting
        {
            get => isConnecting;
            set
            {
                isConnecting = value;

                cancelGO.SetActive(isConnecting);
                createGO.SetActive(!isConnecting);

                joinOffsetRT.offsetMin = new Vector2(joinOffsetRT.offsetMin.x, isConnecting ? 75 : 0);
            }
        }
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }
        private void OnDestroy()
        {
            if (NetworkManager.Singleton) Shutdown();
        }

        private void Setup()
        {
            SetupHost();
            SetupJoin();

            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;

            Authenticate();
        }
        private void SetupJoin()
        {
        }
        private void SetupHost()
        {
            // Map
            mapOS.SetupUsingEnum<MapType>();
            mapOS.Select(MapType.Farm);

            // Lobby Type
            visibilityOS.SetupUsingEnum<VisibilityType>();
            visibilityOS.OnSelected.AddListener(delegate (int option)
            {
                passwordGO.gameObject.SetActive((VisibilityType)option == VisibilityType.Public);
            });
            visibilityOS.Select(VisibilityType.Public);
        }
        private void Shutdown()
        {
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnect;
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

        public void SP_Play()
        {
            SceneManager.LoadScene("Singleplayer");
        }
        public async void MP_Join(string joinCode)
        {
            if (!IsConnectedToInternet || !IsValidPlayer)
            {
                return;
            }

            await AuthenticateAsync();

            UpdateNetworkStatus("Connecting to Relay...", Color.yellow, -1);
            JoinAllocation join = await Relay.Instance.JoinAllocationAsync(worldNameInputField.text);

            UnityTransport relayTransport = NetworkTransportPicker.Instance.GetTransport<UnityTransport>("Relay");
            relayTransport.SetRelayServerData(join.RelayServer.IpV4, (ushort)join.RelayServer.Port, join.AllocationIdBytes, join.Key, join.ConnectionData, join.HostConnectionData);

            UpdateNetworkStatus("Starting Client...", Color.yellow, -1);
            NetworkManager.Singleton.StartClient();
        }
        public async void MP_Create()
        {
            if (!IsConnectedToInternet || !IsValidPlayer || !IsValidWorldName)
            {
                return;
            }

            await AuthenticateAsync();

            UpdateNetworkStatus("Allocating Relay...", Color.yellow, -1);
            Allocation host = await Relay.Instance.CreateAllocationAsync(10);

            UpdateNetworkStatus("Generating Join Code...", Color.yellow, -1);
            string joinCode = await Relay.Instance.GetJoinCodeAsync(host.AllocationId);

            UpdateNetworkStatus("Creating Lobby...", Color.yellow, -1);
            try
            {
                bool isPrivate = (VisibilityType)visibilityOS.SelectedOption == VisibilityType.Private;
                string mapName = ((MapType)mapOS.SelectedOption).ToString();
                string version = Application.version.ToString();
                bool isPasswordProtected = !string.IsNullOrEmpty(passwordInputField.text);

                CreateLobbyOptions options = new CreateLobbyOptions()
                {
                    IsPrivate = isPrivate,
                    Data = new Dictionary<string, DataObject>()
                    {
                        { "joinCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode) },
                        { "map", new DataObject(DataObject.VisibilityOptions.Public, mapName) },
                        { "version", new DataObject(DataObject.VisibilityOptions.Public, version) },
                        { "isPasswordProtected", new DataObject(DataObject.VisibilityOptions.Public, isPasswordProtected.ToString()) }
                    }
                };
                await LobbyCreationHandler.Instance.CreateLobbyAsync(worldNameInputField.text, (int)maxPlayersSlider.value, options);
            }
            catch (LobbyServiceException e)
            {
                UpdateNetworkStatus($"{e.Message}", Color.red);
                Debug.Log(e);
                return;
            }
            
            UnityTransport relayTransport = NetworkTransportPicker.Instance.GetTransport<UnityTransport>("Relay");
            relayTransport.SetRelayServerData(host.RelayServer.IpV4, (ushort)host.RelayServer.Port, host.AllocationIdBytes, host.Key, host.ConnectionData);

            UpdateNetworkStatus("Starting Host...", Color.yellow, -1);
            NetworkManager.Singleton.StartHost();
        }
        public void Cancel()
        {
            if (!IsConnecting) return;
            if (updateNetStatusCoroutine != null) StopCoroutine(updateNetStatusCoroutine);
            NetworkShutdownManager.Instance.Shutdown();
            HideNetworkStatus();
            IsConnecting = false;
        }

        private async void Authenticate()
        {
            await AuthenticateAsync();
        }
        private async Task AuthenticateAsync()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                UpdateNetworkStatus("Initializing...", Color.yellow, -1);
                await UnityServices.InitializeAsync();
            }
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                UpdateNetworkStatus("Authenticating...", Color.yellow, -1);
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            HideNetworkStatus();
        }

        public async void RefreshWorlds()
        {
            await AuthenticateAsync();

            worldsRT.transform.DestroyChildren();
            refreshGO.SetActive(true);
            noneGO.SetActive(false);
            try
            {
                QueryLobbiesOptions options = new QueryLobbiesOptions()
                {
                    Count = 25
                };
                List<Lobby> lobbies = (await Lobbies.Instance.QueryLobbiesAsync(options)).Results;
                foreach (Lobby lobby in lobbies)
                {
                    WorldUI worldUI = Instantiate(worldUIPrefab, worldsRT);
                    worldUI.Setup(lobby.Id, lobby.Players.Count, lobby.MaxPlayers, lobby.Name, lobby.Data["map"].Value, lobby.Data["version"].Value, bool.Parse(lobby.Data["isPasswordProtected"].Value));
                    worldUI.JoinButton.onClick.AddListener(async delegate
                    {
                        try
                        {
                            await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
                        }
                        catch (LobbyServiceException e)
                        {
                            UpdateNetworkStatus(e.Message, Color.red);
                            Debug.Log(e);
                        }
                    });
                }
                noneGO.SetActive(lobbies.Count == 0);
            }
            catch (LobbyServiceException e)
            {
                UpdateNetworkStatus(e.Message, Color.red);
                noneGO.SetActive(true);
                Debug.Log(e);
            }
            refreshGO.SetActive(false);
        }

        public void ToggleMenu()
        {
            if (!multiplayerMenu.IsOpen)
            {
                RefreshWorlds();
            }
            multiplayerMenu.Toggle();
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
        #endregion

        #region Enum
        public enum MapType
        {
            Farm,
            Island,
            ObstacleCourse
        }
        public enum VisibilityType
        {
            Public,
            Private
        }
        public enum ConnectionType
        {
            Ip,
            Relay
        }
        #endregion
    }
}