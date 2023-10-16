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
            if (!Initialized)
            {
                return false;
            }

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

        public async void SendFriendRequestByName(string playerName, Action<Relationship> onSent = default)
        {
            Relationship friend = await FriendsService.Instance.AddFriendByNameAsync(playerName);
            onSent?.Invoke(friend);
        }
        public async void SendFriendRequest(string playerId, Action<Relationship> onSent = default)
        {
            Relationship friend = await FriendsService.Instance.AddFriendAsync(playerId);
            onSent?.Invoke(friend);
        }
        public async void AcceptFriendRequest(string playerId, Action<Relationship> onAccepted = default)
        {
            Relationship friend = await FriendsService.Instance.AddFriendAsync(playerId);
            onAccepted?.Invoke(friend);
        }
        public async void RejectFriendRequest(string playerId, Action onRejected = default)
        {
            await FriendsService.Instance.DeleteIncomingFriendRequestAsync(playerId);
            onRejected?.Invoke();
        }

        public bool IsBlocked(string playerId)
        {
            if (!Initialized)
            {
                return false;
            }

            foreach (var player in FriendsService.Instance.Blocks)
            {
                if (player.Member.Id == playerId)
                {
                    return true;
                }
            }
            return false;
        }
        public async void BlockPlayer(string playerId, Action<Relationship> onBlocked = default)
        {
            Relationship player = await FriendsService.Instance.AddBlockAsync(playerId);
            onBlocked?.Invoke(player);
        }

        public async void SetStatus(Availability status, string lobbyId = default, Action onSet = default)
        {
            if (!Initialized)
            {
                return;
            }

            await FriendsService.Instance.SetPresenceAsync(status, new FriendData()
            {
                LobbyId = lobbyId
            });
            onSet?.Invoke();
        }
        #endregion
    }
}