// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using ProfanityDetector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MultiplayerUI : MonoBehaviour
    {
        #region Fields
        [Header("Host")]
        [SerializeField] private TMP_InputField worldNameInputField;
        [SerializeField] private OptionSelector mapOS;
        [SerializeField] private OptionSelector lobbyTypeOS;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private GameObject passwordGO;
        [SerializeField] private Slider maxPlayersSlider;
        
        [Header("General")]
        [SerializeField] private Menu multiplayerHintMenu;
        [SerializeField] private TextMeshProUGUI networkStatusText;
        [SerializeField] private BlinkingText networkStatusBT;
        [SerializeField] private TextMeshProUGUI connectButtonText;

        private Coroutine updateNetStatusCoroutine;
        private ProfanityFilter filter = new ProfanityFilter();
        private bool isConnecting;
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
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;

            // Map
            mapOS.SetupUsingEnum<MapType>();
            mapOS.Select((int)MapType.Farm, false);

            // Lobby Type
            lobbyTypeOS.SetupUsingEnum<LobbyType>();
            lobbyTypeOS.Select((int)LobbyType.Public, false);
            lobbyTypeOS.OnSelected.AddListener(delegate (int option)
            {
                passwordGO.gameObject.SetActive(option == 1);
            });
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
            isConnecting = false;
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

        public void PlaySingleplayer()
        {
            SceneManager.LoadScene("Singleplayer");
        }
        //public async void PlayMultiplayer(RoleType role, ConnectionType connection)
        //{
        //    // Check Internet Connection
        //    if (Application.internetReachability == NetworkReachability.NotReachable)
        //    {
        //        UpdateNetworkStatus("You are not connected to the internet.", Color.white);
        //        return;
        //    }

        //    // Set Connection Data
        //    string usernameText = usernameInputField.text;
        //    if (string.IsNullOrEmpty(usernameText))
        //    {
        //        UpdateNetworkStatus("A username must be provided.", Color.white);
        //        return;
        //    }
        //    if (filter.ContainsProfanity(usernameText))
        //    {
        //        UpdateNetworkStatus("Profanity detected in username.", Color.white);
        //        return;
        //    }
        //    if (usernameText.Length > 16)
        //    {
        //        UpdateNetworkStatus("Username cannot be longer than 16 characters.", Color.white);
        //        return;
        //    }
        //    PlayerData playerData = new PlayerData()
        //    {
        //        username = usernameText
        //    };
        //    NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(playerData));

        //    // Authenticate
        //    if (connection == ConnectionType.Relay)
        //    {
        //        UpdateNetworkStatus("Authenticating...", Color.yellow, -1);
        //        await UnityServices.InitializeAsync();
        //        if (!AuthenticationService.Instance.IsSignedIn)
        //        {
        //            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //        }
        //    }

        //    //// World
        //    //string worldNameText = worldNameInputField.text;
        //    //if (string.IsNullOrEmpty(worldNameText))
        //    //{
        //    //    UpdateNetworkStatus("A world name must be provided.", Color.white);
        //    //    return;
        //    //}
        //    //if (filter.ContainsProfanity(worldNameText))
        //    //{
        //    //    UpdateNetworkStatus("Profanity detected in world name.", Color.white);
        //    //    return;
        //    //}

        //    // Connect
        //    switch (connection)
        //    {
        //        case ConnectionType.Ip:
        //            UnityTransport ipTransport = NetworkTransportPicker.Instance.GetTransport<UnityTransport>("IP");

        //            string ipAddressText = ipAddressInputField.text;
        //            string portText = portInputField.text;

        //            // IP Address
        //            if (string.IsNullOrEmpty(ipAddressText))
        //            {
        //                ipAddressText = "127.0.0.1";
        //            }
        //            if (!IPAddress.TryParse(ipAddressText, out IPAddress address))
        //            {
        //                UpdateNetworkStatus("The address must be in the correct format.", Color.white);
        //                return;
        //            }

        //            // Port
        //            if (string.IsNullOrEmpty(portText))
        //            {
        //                portText = "1337";
        //            }
        //            portText.Replace(",", "");
        //            if (!int.TryParse(portText, out int port) || port < 0 || port > 65353)
        //            {
        //                UpdateNetworkStatus("The port must be a number between 0 and 65,353.", Color.white);
        //                return;
        //            }

        //            ipTransport.SetConnectionData(ipAddressText, (ushort)port);

        //            NetworkManager.Singleton.NetworkConfig.NetworkTransport = ipTransport;
        //            break;

        //        case ConnectionType.Relay:
        //            UnityTransport relayTransport = NetworkTransportPicker.Instance.GetTransport<UnityTransport>("Relay");
        //            switch (role)
        //            {
        //                case RoleType.Client:
        //                    UpdateNetworkStatus("Connecting to Relay...", Color.yellow, -1);
        //                    JoinAllocation join = await Relay.Instance.JoinAllocationAsync(worldNameInputField.text);
        //                    relayTransport.SetRelayServerData(join.RelayServer.IpV4, (ushort)join.RelayServer.Port, join.AllocationIdBytes, join.Key, join.ConnectionData, join.HostConnectionData);
        //                    break;
        //                case RoleType.Host:
        //                    UpdateNetworkStatus("Allocating Relay...", Color.yellow, -1);
        //                    Allocation host = await Relay.Instance.CreateAllocationAsync(10);
        //                    UpdateNetworkStatus("Generating Join Code...", Color.yellow, -1);
        //                    string joinCode = await Relay.Instance.GetJoinCodeAsync(host.AllocationId);
        //                    relayTransport.SetRelayServerData(host.RelayServer.IpV4, (ushort)host.RelayServer.Port, host.AllocationIdBytes, host.Key, host.ConnectionData);
        //                    break;
        //            }
        //            NetworkManager.Singleton.NetworkConfig.NetworkTransport = relayTransport;
        //            break;
        //    }









        //    // Start
        //    switch (role)
        //    {
        //        case RoleType.Client:
        //            UpdateNetworkStatus("Starting Client...", Color.yellow, -1);
        //            NetworkManager.Singleton.StartClient();
        //            break;
        //        case RoleType.Host:
        //            UpdateNetworkStatus("Starting Host...", Color.yellow, -1);
        //            NetworkManager.Singleton.StartHost();
        //            break;
        //    }
        //}

        public void JoinWorld(string joinCode)
        {
        }
        public void CreateWorld()
        {
        }
        public void Cancel()
        {
            if (!isConnecting) return;
            if (updateNetStatusCoroutine != null) StopCoroutine(updateNetStatusCoroutine);
            NetworkShutdownManager.Instance.Shutdown();
            HideNetworkStatus();
            isConnecting = false;
        }
        #endregion

        #region Enum
        public enum MapType
        {
            Farm,
            Island,
            ObstacleCourse
        }
        public enum LobbyType
        {
            Public,
            Private
        }
        #endregion
    }
}