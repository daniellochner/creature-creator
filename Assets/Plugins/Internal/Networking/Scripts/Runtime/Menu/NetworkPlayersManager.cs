using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkPlayersManager : NetworkSingleton<NetworkPlayersManager>
    {
        #region Fields
        [SerializeField] private NetworkPlayersMenu networkMenuPrefab;
        [SerializeField] private Sprite friendReqIcon;
        [SerializeField] private Color friendColour;

        private NetworkPlayersMenu networkMenu;
        private bool hasHandledExistingPlayers;
        #endregion

        #region Properties
        public Action<PlayerData> OnConfirmFriendRequest { get; set; }

        public Dictionary<ulong, PlayerData> Players { get; private set; } = new Dictionary<ulong, PlayerData>();
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
            if (hasHandledExistingPlayers)
            {
                string notification = LocalizationUtility.Localize("player-join", playerData.username.NoParse());
                if (FriendsManager.Instance.IsFriended(playerData.playerId))
                {
                    notification = notification.ToColour(friendColour);
                }
                NotificationsManager.Notify(notification);

                Players.Add(playerData.clientId, playerData);

                NetworkPlayersMenu.Instance.AddPlayer(playerData);
            }
        }
        private void OnPlayerLeave(PlayerData playerData)
        {
            if (hasHandledExistingPlayers)
            {
                string notification = LocalizationUtility.Localize("player-leave", playerData.username.NoParse());
                if (FriendsManager.Instance.IsFriended(playerData.playerId))
                {
                    notification = notification.ToColour(friendColour);
                }
                NotificationsManager.Notify(notification);

                Players.Remove(playerData.clientId);

                NetworkPlayersMenu.Instance.RemovePlayer(playerData.clientId);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void HandleExistingPlayersServerRpc(ulong clientId)
        {
            HandleExistingPlayersClientRpc(NetworkHostManager.Instance.Players.Values.ToArray(), NetworkUtils.SendTo(clientId));
        }
        [ClientRpc]
        public void HandleExistingPlayersClientRpc(PlayerData[] players, ClientRpcParams clientRpcParams)
        {
            foreach (PlayerData player in players)
            {
                NetworkPlayersMenu.Instance.AddPlayer(player);
                Players.Add(player.clientId, player);
            }
            hasHandledExistingPlayers = true;
        }
        public void HandleExistingPlayers()
        {
            HandleExistingPlayersServerRpc(NetworkManager.Singleton.LocalClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendFriendRequestServerRpc(ulong fromClientId, ulong toClientId)
        {
            SendFriendRequestClientRpc(NetworkHostManager.Instance.Players[fromClientId], NetworkUtils.SendTo(toClientId));
        }
        [ClientRpc]
        public void SendFriendRequestClientRpc(PlayerData playerData, ClientRpcParams sendTo)
        {
            if (!FriendsManager.Instance.IsFriended(playerData.playerId) && !FriendsManager.Instance.IsBlocked(playerData.playerId))
            {
                NotificationsManager.Notify(friendReqIcon, LocalizationUtility.Localize("friends_notification_request_title", playerData.username), LocalizationUtility.Localize("friends_notification_request_description"), delegate
                {
                    FriendsManager.Instance.AcceptFriendRequest(playerData.playerId);

                    OnConfirmFriendRequest?.Invoke(playerData);

                    ConfirmFriendRequest(playerData);
                });
            }
        }
        public void SendFriendRequest(PlayerData playerData)
        {
            SendFriendRequestServerRpc(NetworkManager.Singleton.LocalClientId, playerData.clientId);

            NotificationsManager.Notify(LocalizationUtility.Localize("friends_notification_send", playerData.username).ToColour(friendColour));
        }

        [ServerRpc(RequireOwnership = false)]
        public void ConfirmFriendRequestServerRpc(ulong fromClientId, ulong toClientId)
        {
            ConfirmFriendRequestClientRpc(NetworkHostManager.Instance.Players[fromClientId], NetworkUtils.SendTo(toClientId));
        }
        [ClientRpc]
        public void ConfirmFriendRequestClientRpc(PlayerData playerData, ClientRpcParams sendTo)
        {
            NotificationsManager.Notify(LocalizationUtility.Localize("friends_notification_accept", playerData.username).ToColour(friendColour));

            OnConfirmFriendRequest?.Invoke(playerData);
        }
        public void ConfirmFriendRequest(PlayerData playerData)
        {
            ConfirmFriendRequestServerRpc(NetworkManager.Singleton.LocalClientId, playerData.clientId);
        }
        #endregion
    }
}