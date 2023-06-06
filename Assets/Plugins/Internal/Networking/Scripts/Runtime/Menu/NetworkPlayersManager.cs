using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkPlayersManager : NetworkSingleton<NetworkPlayersManager>
    {
        #region Fields
        [SerializeField] private NetworkPlayersMenu networkMenuPrefab;

        private NetworkPlayersMenu networkMenu;
        #endregion

        #region Properties
        public int NumPlayers => networkMenu.NumPlayers;
        #endregion

        #region Methods
        private void Start()
        {
            if (IsServer)
            {
                NetworkHostManager.Instance.OnPlayerAdd += PlayerJoinClientRpc;
                NetworkHostManager.Instance.OnPlayerRemove += PlayerLeaveClientRpc;
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();

            if (NetworkHostManager.Instance)
            {
                NetworkHostManager.Instance.OnPlayerAdd -= PlayerJoinClientRpc;
                NetworkHostManager.Instance.OnPlayerRemove -= PlayerLeaveClientRpc;
            }
            if (networkMenu != null)
            {
                Destroy(networkMenu.gameObject);
            }
        }

        public void Setup(string code)
        {
            networkMenu = Instantiate(networkMenuPrefab, Dynamic.OverlayCanvas);
            networkMenu.Setup(code);

            HandleExistingPlayers();
        }

        [ClientRpc]
        private void PlayerJoinClientRpc(PlayerData playerData)
        {
            if (NetworkUtils.IsPlayer(playerData.clientId)) return;
            OnPlayerJoin(playerData);
        }
        [ClientRpc]
        private void PlayerLeaveClientRpc(PlayerData playerData)
        {
            if (NetworkUtils.IsPlayer(playerData.clientId)) return;
            OnPlayerLeave(playerData);
        }
        private void OnPlayerJoin(PlayerData playerData)
        {
            NotificationsManager.Notify(LocalizationUtility.Localize("player-join", playerData.username));
            NetworkPlayersMenu.Instance.AddPlayer(playerData);
        }
        private void OnPlayerLeave(PlayerData playerData)
        {
            NotificationsManager.Notify(LocalizationUtility.Localize("player-leave", playerData.username));
            NetworkPlayersMenu.Instance.RemovePlayer(playerData.clientId);
        }

        [ServerRpc(RequireOwnership = false)]
        public void HandleExistingPlayersServerRpc(ulong clientId)
        {
            foreach (PlayerData data in NetworkHostManager.Instance.Players.Values)
            {
                HandleExistingPlayersClientRpc(data, NetworkUtils.SendTo(clientId));
            }
        }
        [ClientRpc]
        public void HandleExistingPlayersClientRpc(PlayerData data, ClientRpcParams clientRpcParams)
        {
            NetworkPlayersMenu.Instance.AddPlayer(data);
        }
        public void HandleExistingPlayers()
        {
            HandleExistingPlayersServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        #endregion
    }
}