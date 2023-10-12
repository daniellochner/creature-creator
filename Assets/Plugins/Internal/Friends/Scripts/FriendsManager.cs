using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Friends;
using Unity.Services.Friends.Models;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class FriendsManager : MonoBehaviourSingleton<FriendsManager>
    {
        #region Properties
        public bool Initialized { get; set; }
        #endregion

        #region Methods
        public async Task Initialize()
        {
            if (!Initialized)
            {
                await FriendsService.Instance.InitializeAsync();
                Initialized = true;
            }
        }

        public bool IsFriended(string playerId)
        {
            foreach (var friend in FriendsService.Instance.Friends)
            {
                if (friend.Member.Id == playerId)
                {
                    return true;
                }
            }
            return false;
        }

        public async void DeleteFriend(string playerId, Action onRemoved = default)
        {
            await FriendsService.Instance.DeleteFriendAsync(playerId);
            onRemoved?.Invoke();
        }

        public async void SendFriendRequestByName(string playerName, Action onSent = default)
        {
            await FriendsService.Instance.AddFriendByNameAsync(playerName);
            onSent?.Invoke();
        }
        public async void SendFriendRequest(string playerId, Action onSent = default)
        {
            await FriendsService.Instance.AddFriendAsync(playerId);
            onSent?.Invoke();
        }
        public async void AcceptFriendRequest(string playerId, Action onAccepted = default)
        {
            await FriendsService.Instance.AddFriendAsync(playerId);
            onAccepted?.Invoke();
        }
        public async void RejectFriendRequest(string playerId, Action onRejected = default)
        {
            await FriendsService.Instance.DeleteIncomingFriendRequestAsync(playerId);
            onRejected?.Invoke();
        }

        public bool IsBlocked(string playerId)
        {
            foreach (var player in FriendsService.Instance.Blocks)
            {
                if (player.Member.Id == playerId)
                {
                    return true;
                }
            }
            return false;
        }
        public async void BlockPlayer(string playerId, Action onBlocked = default)
        {
            await FriendsService.Instance.AddBlockAsync(playerId);
            onBlocked?.Invoke();
        }

        public async void SetStatus(Availability status, string lobbyId = default, Action onSet = default)
        {
            await FriendsService.Instance.SetPresenceAsync(status, new FriendData()
            {
                LobbyId = lobbyId
            });
            onSet?.Invoke();
        }
        #endregion
    }
}