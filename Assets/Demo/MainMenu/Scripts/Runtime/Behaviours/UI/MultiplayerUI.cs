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
using System;
using System.Text;
using System.Security.Cryptography;
using Unity.Netcode.Transports.UTP;
using LobbyPlayer = Unity.Services.Lobbies.Models.Player;
using System.Linq;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MultiplayerUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TMP_InputField onlineUsernameInputField;
        [SerializeField] private TextMeshProUGUI networkStatusText;
        [SerializeField] private BlinkingText networkStatusBT;
        [SerializeField] private Button createButton;
        [SerializeField] private Menu multiplayerMenu;
        [SerializeField] private Menu multiplayerHintMenu;
        [SerializeField] private SimpleScrollSnap.SimpleScrollSnap multiplayerSSS;

        [Header("Join")]
        [SerializeField] private WorldUI worldUIPrefab;
        [SerializeField] private RectTransform joinOffsetRT;
        [SerializeField] private RectTransform worldsRT;
        [SerializeField] private GameObject noneGO;
        [SerializeField] private GameObject refreshGO;
        [SerializeField] private Button refreshButton;
        [SerializeField] private TMP_InputField lobbyCodeInputField;

        [Header("Create")]
        [SerializeField] private TMP_InputField worldNameInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private OptionSelector mapOS;
        [SerializeField] private OptionSelector visibilityOS;
        [SerializeField] private GameObject passwordGO;
        [SerializeField] private Toggle passwordToggle;
        [SerializeField] private Slider maxPlayersSlider;
        [SerializeField] private Toggle pvpToggle;
        [SerializeField] private Toggle pveToggle;
        [SerializeField] private Toggle npcToggle;

        private ProfanityFilter filter = new ProfanityFilter();
        private SHA256 sha256 = SHA256.Create();
        private bool isConnecting, isRefreshing;
        private UnityTransport relayTransport;
        private Coroutine updateNetStatusCoroutine;
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
                createButton.interactable = !isConnecting;

                foreach (Transform worldRT in worldsRT)
                {
                    worldRT.GetComponent<WorldUI>().JoinButton.interactable = !isConnecting;
                }
            }
        }
        private bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                refreshGO.SetActive(isRefreshing);
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
            relayTransport = NetworkTransportPicker.Instance.GetTransport<UnityTransport>("Relay");

            mapOS.SetupUsingEnum<MapType>();
            mapOS.Select(MapType.Farm);

            visibilityOS.SetupUsingEnum<VisibilityType>();
            visibilityOS.OnSelected.AddListener(delegate (int option)
            {
                passwordGO.gameObject.SetActive((VisibilityType)option == VisibilityType.Public);
            });
            visibilityOS.Select(VisibilityType.Public);

            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
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
            NetworkManager.Singleton.SceneManager.LoadScene(mapOS.Options[mapOS.Selected].Name, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        public void Play()
        {
            SceneManager.LoadScene("Island");
        }
        public async void Join(string id)
        {
            if (!IsConnectedToInternet || !IsValidPlayer)
            {
                return;
            }
            IsConnecting = true;

            try
            {
                // Authenticate
                await Authenticate();

                // Confirm Password
                Lobby lobby = await Lobbies.Instance.GetLobbyAsync(id);
                string passwordHash = lobby.Data["passwordHash"].Value;
                string password = "";
                if (!string.IsNullOrEmpty(passwordHash))
                {
                    password = await InputDialog.InputAsync("Password Required", "Enter the password...", error: "No password was provided.");
                    bool isValidPasswordHash = string.IsNullOrEmpty(passwordHash) || sha256.VerifyHash(password, passwordHash);
                    if (!isValidPasswordHash)
                    {
                        throw new Exception("Invalid password.");
                    }
                }

                // Set Up Connection Data
                string username = onlineUsernameInputField.text;
                SetConnectionData(username, password);

                // Join Lobby
                UpdateNetworkStatus("Joining Lobby...", Color.yellow, -1);
                LobbyPlayer player = new LobbyPlayer(AuthenticationService.Instance.PlayerId);
                JoinLobbyByIdOptions options = new JoinLobbyByIdOptions()
                {
                    Player = player
                };
                lobby = await LobbyHelper.Instance.JoinLobbyByIdAsync(id, options);

                // Join Relay
                UpdateNetworkStatus("Joining Via Relay...", Color.yellow, -1);
                string joinCode = lobby.Data["joinCode"].Value;
                JoinAllocation join = await Relay.Instance.JoinAllocationAsync(joinCode);
                await Lobbies.Instance.UpdatePlayerAsync(lobby.Id, player.Id, new UpdatePlayerOptions()
                {
                    AllocationId = join.AllocationId.ToString(),
                    ConnectionInfo = joinCode
                });
                relayTransport.SetClientRelayData(join.RelayServer.IpV4, (ushort)join.RelayServer.Port, join.AllocationIdBytes, join.Key, join.ConnectionData, join.HostConnectionData);

                // Start Client
                UpdateNetworkStatus("Starting Client...", Color.yellow, -1);
                NetworkManager.Singleton.StartClient();
            }
            catch (Exception e)
            {
                UpdateNetworkStatus(e.Message, Color.red);
                IsConnecting = false;
            }
        }
        public async void Create()
        {
            if (!IsConnectedToInternet || !IsValidPlayer || !IsValidWorldName)
            {
                return;
            }
            IsConnecting = true;

            try
            {
                // Set Up World
                bool isPrivate = (VisibilityType)visibilityOS.Selected == VisibilityType.Private;
                bool usePassword = passwordToggle.isOn && !isPrivate && !string.IsNullOrEmpty(passwordInputField.text);
                string worldName = worldNameInputField.text;
                string mapName = ((MapType)mapOS.Selected).ToString();
                string version = Application.version;
                int maxPlayers = (int)maxPlayersSlider.value;
                bool allowPVP = pvpToggle.isOn;
                bool spawnNPC = npcToggle.isOn;
                bool allowPVE = pveToggle.isOn;

                // Set Up Connection Data
                string username = onlineUsernameInputField.text;
                string password = NetworkHostManager.Instance.Password = (usePassword ? passwordInputField.text : "");
                string passwordHash = usePassword ? sha256.GetHash(password) : "";
                SetConnectionData(username, password);

                // Authenticate
                await Authenticate();

                // Allocate Relay
                UpdateNetworkStatus("Allocating Relay...", Color.yellow, -1);
                Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxPlayers);
                relayTransport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);

                // Generate Join Code
                UpdateNetworkStatus("Generating Join Code...", Color.yellow, -1);
                string joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);

                // Create Lobby
                UpdateNetworkStatus("Creating Lobby...", Color.yellow, -1);
                CreateLobbyOptions options = new CreateLobbyOptions()
                {
                    IsPrivate = false,
                    Data = new Dictionary<string, DataObject>()
                    {
                        { "isPrivate", new DataObject(DataObject.VisibilityOptions.Public, isPrivate.ToString())},
                        { "joinCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode) },
                        { "version", new DataObject(DataObject.VisibilityOptions.Public, version) },
                        { "mapName", new DataObject(DataObject.VisibilityOptions.Public, mapName) },
                        { "passwordHash", new DataObject(DataObject.VisibilityOptions.Public, passwordHash) },
                        { "allowPVP", new DataObject(DataObject.VisibilityOptions.Public, allowPVP.ToString()) },
                        { "allowPVE", new DataObject(DataObject.VisibilityOptions.Public, allowPVE.ToString()) },
                        { "spawnNPC", new DataObject(DataObject.VisibilityOptions.Public, spawnNPC.ToString()) }
                    },
                    Player = new LobbyPlayer(AuthenticationService.Instance.PlayerId, joinCode, null, allocation.AllocationId.ToString())
                };
                await LobbyHelper.Instance.CreateLobbyAsync(worldName, maxPlayers, options);

                // Start Host
                UpdateNetworkStatus("Starting Host...", Color.yellow, -1);
                NetworkManager.Singleton.StartHost();
            }
            catch (Exception e)
            {
                UpdateNetworkStatus($"{e.Message}", Color.red);
                IsConnecting = false;
            }
        }
        public async void Refresh()
        {
            await Authenticate();

            IsRefreshing = true;

            worldsRT.transform.DestroyChildren();
            noneGO.SetActive(false);
            refreshButton.interactable = false;
            
            try
            {
                List<Lobby> lobbies = (await Lobbies.Instance.QueryLobbiesAsync()).Results;
                foreach (Lobby lobby in lobbies)
                {
                    World world = new World(lobby);
                    if (!world.IsPrivate)
                    {
                        Instantiate(worldUIPrefab, worldsRT).Setup(this, lobby);
                    }
                }
                noneGO.SetActive(worldsRT.childCount == 0);
            }
            catch (LobbyServiceException e)
            {
                UpdateNetworkStatus(e.Message, Color.red);
                noneGO.SetActive(true);
            }

            this.Invoke(delegate
            {
                refreshButton.interactable = true;
            }, 1f);
            IsRefreshing = false;
        }
        public void TryRefresh()
        {
            if (multiplayerMenu.IsOpen && !IsRefreshing && multiplayerSSS.SelectedPanel == 0)
            {
                Refresh();
            }
        }
        public void Cancel()
        {
            if (!IsConnecting) return;
            if (updateNetStatusCoroutine != null) StopCoroutine(updateNetStatusCoroutine);
            NetworkShutdownManager.Instance.Shutdown();
            HideNetworkStatus();
            IsConnecting = false;
        }
        public void Join()
        {
            Join(lobbyCodeInputField.text);
        }

        private async Task Authenticate()
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                UpdateNetworkStatus("Authenticating...", Color.yellow, -1);
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                HideNetworkStatus();
            }
        }
        private void SetConnectionData(string username, string password)
        {
            ConnectionData data = new ConnectionData(username, password);
            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
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
            Island
        }
        public enum VisibilityType
        {
            Public,
            Private
        }
        #endregion
    }
}