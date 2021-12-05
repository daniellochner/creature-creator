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
using System.Collections;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MultiplayerUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TMP_InputField onlineUsernameInputField;
        [SerializeField] private TextMeshProUGUI networkStatusText;
        [SerializeField] private BlinkingText networkStatusBT;
        [SerializeField] private GameObject cancelGO;
        [SerializeField] private GameObject createGO;
        [SerializeField] private Menu multiplayerMenu;
        [SerializeField] private Menu multiplayerHintMenu;

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

        private Coroutine updateNetStatusCoroutine;
        private ProfanityFilter filter = new ProfanityFilter();
        private bool isConnecting, isRefreshing;
        private UnityTransport relayTransport;
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

                cancelGO.SetActive(isConnecting);
                createGO.SetActive(!isConnecting);

                joinOffsetRT.offsetMin = new Vector2(joinOffsetRT.offsetMin.x, isConnecting ? 75 : 0);
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

            // Map
            mapOS.SetupUsingEnum<MapType>();
            mapOS.Select(MapType.Farm);

            // Visibility
            visibilityOS.SetupUsingEnum<VisibilityType>();
            visibilityOS.OnSelected.AddListener(delegate (int option)
            {
                passwordGO.gameObject.SetActive((VisibilityType)option == VisibilityType.Public);
            });
            visibilityOS.Select(VisibilityType.Public);

            NetworkShutdownManager.Instance.OnUncontrolledShutdown = OnUncontrolledShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledClientShutdown = OnUncontrolledClientShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledHostShutdown = OnUncontrolledHostShutdown;

            //NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            //NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            //NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
        }
        private void Shutdown()
        {
            //NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            //NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            //NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnect;
        }

        //private void OnServerStarted()
        //{
        //    OnMultiplayerSuccess("Created.");
        //}
        //private void OnClientDisconnect(ulong clientID)
        //{
        //    UpdateNetworkStatus("Connection failed.", Color.red);
        //    IsConnecting = false;
        //}
        //private void OnClientConnect(ulong clientID)
        //{
        //    OnMultiplayerSuccess("Connected.");
        //}
        //private void OnMultiplayerSuccess(string message)
        //{
        //    UpdateNetworkStatus(message, Color.green);
        //    NetworkManager.Singleton.SceneManager.LoadScene("Multiplayer", UnityEngine.SceneManagement.LoadSceneMode.Single);
        //}
        private void OnUncontrolledShutdown()
        {
            SceneManager.LoadScene("MainMenu");
        }
        private void OnUncontrolledClientShutdown()
        {
            InformationDialog.Inform("Disconnected!", "You lost connection.");
        }
        private void OnUncontrolledHostShutdown()
        {
            InformationDialog.Inform("Disconnected!", "The host lost connection.");
        }

        public void Play()
        {
            SceneManager.LoadScene("Singleplayer");
        }
        public async void Join(string lobbyCode, string password = "")
        {
            if (!IsConnectedToInternet || !IsValidPlayer)
            {
                return;
            }

            try
            {
                await Authenticate();

                UpdateNetworkStatus("Joining Lobby...", Color.yellow, -1);
                JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions()
                {
                    Player = new Unity.Services.Lobbies.Models.Player(AuthenticationService.Instance.PlayerId)
                };
                Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, options);

                UpdateNetworkStatus("Joining Via Relay...", Color.yellow, -1);
                string joinCode = lobby.Data["joinCode"].Value;
                JoinAllocation join = await Relay.Instance.JoinAllocationAsync(joinCode);
                await Lobbies.Instance.UpdatePlayerAsync(lobby.Id, options.Player.Id, new UpdatePlayerOptions()
                {
                    AllocationId = join.AllocationId.ToString(),
                    ConnectionInfo = joinCode
                });
                relayTransport.SetRelayServerData(join.RelayServer.IpV4, (ushort)join.RelayServer.Port, join.AllocationIdBytes, join.Key, join.ConnectionData, join.HostConnectionData);
                SetConnectionData(onlineUsernameInputField.text, password);

                UpdateNetworkStatus("Joined.", Color.green);
                await Task.Delay(500);
                LoadingManager.LoadScene("Multiplayer", null, () => NetworkManager.Singleton.StartClient());
            }
            catch (Exception e)
            {
                UpdateNetworkStatus(e.Message, Color.red);
                Debug.Log(e);
            }
        }
        public async void Create()
        {
            if (!IsConnectedToInternet || !IsValidPlayer || !IsValidWorldName)
            {
                return;
            }

            try
            {
                await Authenticate();

                UpdateNetworkStatus("Allocating Relay...", Color.yellow, -1);
                Allocation allocation = await Relay.Instance.CreateAllocationAsync(10);
                relayTransport.SetRelayServerData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);
                string username = onlineUsernameInputField.text;
                string password = NetworkHostManager.Instance.Password = passwordInputField.text;
                SetConnectionData(username, password);

                UpdateNetworkStatus("Generating Join Code...", Color.yellow, -1);
                string joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);

                UpdateNetworkStatus("Creating Lobby...", Color.yellow, -1);
                string version = Application.version;
                string mapName = ((MapType)mapOS.Selected).ToString();
                bool isPrivate = ((VisibilityType)visibilityOS.Selected) == VisibilityType.Private;
                bool isPasswordProtected = passwordToggle.isOn && !string.IsNullOrEmpty(password);
                CreateLobbyOptions options = new CreateLobbyOptions()
                {
                    IsPrivate = isPrivate,
                    Data = new Dictionary<string, DataObject>()
                    {
                        { "version", new DataObject(DataObject.VisibilityOptions.Public, version) },
                        { "mapName", new DataObject(DataObject.VisibilityOptions.Public, mapName) },
                        { "isPasswordProtected", new DataObject(DataObject.VisibilityOptions.Public, isPasswordProtected.ToString()) },
                        { "joinCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode) }
                    },
                    Player = new Unity.Services.Lobbies.Models.Player(AuthenticationService.Instance.PlayerId, joinCode, null, allocation.AllocationId.ToString())
                };
                Lobby lobby = await LobbyCreationHandler.Instance.CreateLobbyAsync(worldNameInputField.text, (int)maxPlayersSlider.value, options);

                UpdateNetworkStatus("Created.", Color.green);
                await Task.Delay(500);
                LoadingManager.LoadScene("Multiplayer", null, () => NetworkManager.Singleton.StartHost());
            }
            catch (Exception e)
            {
                UpdateNetworkStatus($"{e.Message}", Color.red);
                Debug.Log(e);
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
                QueryLobbiesOptions options = new QueryLobbiesOptions()
                {
                    // TODO: ordering and filters
                };
                List<Lobby> lobbies = (await Lobbies.Instance.QueryLobbiesAsync(options)).Results;
                foreach (Lobby lobby in lobbies)
                {
                    Instantiate(worldUIPrefab, worldsRT).Setup(this, lobby);
                }
                noneGO.SetActive(lobbies.Count == 0);
            }
            catch (LobbyServiceException e)
            {
                UpdateNetworkStatus(e.Message, Color.red);
                noneGO.SetActive(true);
                Debug.Log(e);
            }

            this.Invoke(delegate
            {
                refreshButton.interactable = true;
            }, 1f);
            IsRefreshing = false;
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
        public void ToggleMenu()
        {
            if (!multiplayerMenu.IsOpen && !IsRefreshing)
            {
                Refresh();
            }
            multiplayerMenu.Toggle();
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
            Island,
            ObstacleCourse
        }
        public enum VisibilityType
        {
            Public,
            Private
        }
        #endregion
    }
}