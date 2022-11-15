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
using Steamworks;
using Netcode.Transports;

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
        [SerializeField] private OptionSelector relayServerOS;

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
        [SerializeField] private OptionSelector modeOS;
        [SerializeField] private OptionSelector visibilityOS;
        [SerializeField] private GameObject passwordGO;
        [SerializeField] private Toggle passwordToggle;
        [SerializeField] private Slider maxPlayersSlider;
        [SerializeField] private Toggle pvpToggle;
        [SerializeField] private Toggle pveToggle;
        [SerializeField] private Toggle npcToggle;
        [SerializeField] private Toggle profanityToggle;
        [SerializeField] private Image sortByIcon;

        private ProfanityFilter filter = new ProfanityFilter();
        private SHA256 sha256 = SHA256.Create();
        private bool isConnecting, isRefreshing, isSortedByAscending = true;
        private NetworkTransport relayTransport;
        private Coroutine updateNetStatusCoroutine;
        private int refreshCount;
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

        private bool UseSteam
        {
            get => relayTransport is SteamNetworkingTransport;
        }
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }

        private void OnEnable()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
        }
        private void OnDisable()
        {
            if (NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnect;
            }
        }
        
        private void Setup()
        {
            relayServerOS.SetupUsingEnum<RelayServer>();
            relayServerOS.OnSelected.AddListener(delegate (int option)
            {
                string relay = $"relay_{relayServerOS.Options[relayServerOS.Selected].Name.ToLower()}";
                relayTransport = NetworkTransportPicker.Instance.GetTransport<NetworkTransport>(relay);
            });
            relayServerOS.Select(RelayServer.Unity);

            mapOS.SetupUsingEnum<Map>();
            mapOS.OnSelected.AddListener(delegate (int option)
            {
                Map map = (Map)option;
                switch (map)
                {
                    case Map.Island:
                        maxPlayersSlider.value = maxPlayersSlider.maxValue = 8;
                        break;

                    case Map.Sandbox:
                        maxPlayersSlider.value = maxPlayersSlider.maxValue = 8;
                        break;

                    case Map.Farm:
                        maxPlayersSlider.value = maxPlayersSlider.maxValue = 16;
                        break;
                }
            });
            mapOS.Select(Map.Island);

            modeOS.SetupUsingEnum<Mode>();
            modeOS.Select(Mode.Adventure);

            visibilityOS.SetupUsingEnum<Visibility>();
            visibilityOS.OnSelected.AddListener(delegate (int option)
            {
                passwordGO.gameObject.SetActive((Visibility)option == Visibility.Public);
            });
            visibilityOS.Select(Visibility.Public);
        }

        private void OnClientDisconnect(ulong clientID)
        {
            UpdateNetworkStatus("Connection failed.", Color.red);
            IsConnecting = false;
        }
        private void OnClientConnect(ulong clientID)
        {
            UpdateNetworkStatus("Connected.", Color.green);
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

                // Version
                string version = lobby.Data["version"].Value;
                if (!version.Equals(Application.version))
                {
                    throw new Exception($"Version ({Application.version}) different to host ({version}).");
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
                NetworkManager.Singleton.NetworkConfig.NetworkTransport = relayTransport;
                string joinCode = lobby.Data["joinCode"].Value;
                string hostSteamId = lobby.Data["hostSteamId"].Value;
                JoinAllocation join = await Relay.Instance.JoinAllocationAsync(joinCode);

                await Lobbies.Instance.UpdatePlayerAsync(lobby.Id, player.Id, new UpdatePlayerOptions()
                {
                    AllocationId = (UseSteam ? hostSteamId.ToString() : join.AllocationId.ToString()),
                    ConnectionInfo = joinCode
                });
                if (!UseSteam)
                {
                    UnityTransport unityTransport = relayTransport as UnityTransport;
                    unityTransport.SetClientRelayData(join.RelayServer.IpV4, (ushort)join.RelayServer.Port, join.AllocationIdBytes, join.Key, join.ConnectionData, join.HostConnectionData);
                }
                else
                {
                    SteamNetworkingTransport steamTransport = relayTransport as SteamNetworkingTransport;
                    steamTransport.ConnectToSteamID = ulong.Parse(lobby.Data["hostSteamId"].Value);
                }

                // Start Client
                UpdateNetworkStatus("Starting Client...", Color.yellow, -1);
                Play();
                NetworkManager.Singleton.StartClient();
            }
            catch (NullReferenceException)
            {
                UpdateNetworkStatus("Lobby error.", Color.red); // TODO: Bug with Lobby returning NullReferenceException?
                IsConnecting = false;
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
                bool isPrivate = (Visibility)visibilityOS.Selected == Visibility.Private;
                bool usePassword = passwordToggle.isOn && !isPrivate && !string.IsNullOrEmpty(passwordInputField.text);
                string worldName = worldNameInputField.text;
                string mapName = ((Map)mapOS.Selected).ToString();
                string version = Application.version;
                int maxPlayers = (int)maxPlayersSlider.value;
                bool enablePVP = pvpToggle.isOn;
                bool spawnNPC = npcToggle.isOn;
                bool enablePVE = pveToggle.isOn;
                bool allowProfanity = profanityToggle.isOn;
                bool creativeMode = ((Mode)modeOS.Selected) == Mode.Creative;
                ulong hostSteamId = SteamUser.GetSteamID().m_SteamID;
                
                // Set Up Connection Data
                string username = onlineUsernameInputField.text;
                string password = NetworkHostManager.Instance.Password = (usePassword ? passwordInputField.text : "");
                string passwordHash = usePassword ? sha256.GetHash(password) : "";
                SetConnectionData(username, password);
                
                // Authenticate
                await Authenticate();

                // Allocate Relay
                UpdateNetworkStatus("Allocating Relay...", Color.yellow, -1);
                NetworkManager.Singleton.NetworkConfig.NetworkTransport = relayTransport;
                Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxPlayers);
                if (!UseSteam)
                {
                    UnityTransport unityTransport = relayTransport as UnityTransport;
                    unityTransport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);
                }
                else
                {
                    SteamNetworkingTransport steamTransport = relayTransport as SteamNetworkingTransport;
                    steamTransport.ConnectToSteamID = hostSteamId;
                }

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
                        { "enablePVP", new DataObject(DataObject.VisibilityOptions.Public, enablePVP.ToString()) },
                        { "enablePVE", new DataObject(DataObject.VisibilityOptions.Public, enablePVE.ToString()) },
                        { "spawnNPC", new DataObject(DataObject.VisibilityOptions.Public, spawnNPC.ToString()) },
                        { "allowProfanity", new DataObject(DataObject.VisibilityOptions.Public, allowProfanity.ToString()) },
                        { "creativeMode", new DataObject(DataObject.VisibilityOptions.Public, creativeMode.ToString()) },
                        { "hostSteamId", new DataObject(DataObject.VisibilityOptions.Public, hostSteamId.ToString()) }
                    },
                    Player = new LobbyPlayer(AuthenticationService.Instance.PlayerId, joinCode, null, (UseSteam ? hostSteamId.ToString() : allocation.AllocationId.ToString()))
                };
                await LobbyHelper.Instance.CreateLobbyAsync(worldName, maxPlayers, options);

                // Start Host
                UpdateNetworkStatus("Starting Host...", Color.yellow, -1);
                Play();
                NetworkManager.Singleton.StartHost();
            }
            catch (Exception e)
            {
                UpdateNetworkStatus(e.Message, Color.red);
                IsConnecting = false;
            }
        }
        public async Task<int> Refresh()
        {
            await Authenticate();

            IsRefreshing = true;
            refreshCount++;

            worldsRT.transform.DestroyChildren();
            noneGO.SetActive(false);
            refreshButton.interactable = false;

            try
            {
                List<Lobby> lobbies = (await Lobbies.Instance.QueryLobbiesAsync()).Results;
                foreach (Lobby lobby in lobbies)
                {
                    WorldMP world = new WorldMP(lobby);
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

            return worldsRT.childCount;
        }
        public async void TryRefresh()
        {
            if (multiplayerMenu.IsOpen && !IsRefreshing && multiplayerSSS.SelectedPanel == 0)
            {
                int numWorlds = await Refresh();
                if (numWorlds == 0 && refreshCount == 3)
                {
                    multiplayerHintMenu.Open();
                }
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
        public void Play()
        {
            WorldManager.Instance.World = new WorldMP(LobbyHelper.Instance.JoinedLobby);
        }

        public void SortBy()
        {
            isSortedByAscending = !isSortedByAscending;

            List<WorldUI> worlds = new List<WorldUI>(worldsRT.GetComponentsInChildren<WorldUI>());
            worlds.Sort((x, y) => x.Players.CompareTo(y.Players));

            for (int i = 0; i < worlds.Count; ++i)
            {
                int siblingIndex = isSortedByAscending ? i : (worlds.Count - 1) - i;
                worlds[i].transform.SetSiblingIndex(siblingIndex);
            }
            sortByIcon.transform.localScale = new Vector3(1f, isSortedByAscending ? 1f : -1f, 1f);
        }
        public void Filter()
        {
            InputDialog.Input("Filter Worlds", "Enter text to filter by...", onSubmit: delegate (string filterText)
            {
                filterText = filterText.ToLower();

                foreach (Transform world in worldsRT)
                {
                    bool filtered = false;
                    if (!string.IsNullOrEmpty(filterText))
                    {
                        filtered = true;
                        foreach (TextMeshProUGUI tmp in world.GetComponentsInChildren<TextMeshProUGUI>())
                        {
                            string text = tmp.text.ToLower();
                            if (text.Contains(filterText.ToLower()))
                            {
                                filtered = false;
                                break;
                            }
                        }
                    }
                    world.gameObject.SetActive(!filtered);
                }
            });
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
        public enum RelayServer
        {
            Unity,
            Steam
        }

        public enum Visibility
        {
            Public,
            Private
        }
        #endregion
    }
}