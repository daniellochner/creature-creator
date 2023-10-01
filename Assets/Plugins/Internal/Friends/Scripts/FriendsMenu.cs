using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Friends;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Notifications;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class FriendsMenu : MonoBehaviour
    {
        #region Fields
        [SerializeField] private LocalizedText titleText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private TextMeshProUGUI playerIdText;
        [SerializeField] private RectTransform contentRT;
        [Space]
        [SerializeField] private RequestUI requestPrefab;
        [SerializeField] private FriendUI friendPrefab;
        [SerializeField] private Toggle requestsToggle;
        [SerializeField] private Toggle friendsToggle;
        [Space]
        [SerializeField] private UnityEvent<string> onJoin;

        private Dictionary<string, RequestUI> requests = new Dictionary<string, RequestUI>();
        private Dictionary<string, FriendUI> friends = new Dictionary<string, FriendUI>();
        private CanvasGroup canvasGroup;
        #endregion

        #region Methods
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        private void Start()
        {
            Setup();
        }
        private void OnDestroy()
        {
            Shutdown();
        }

        private async void Setup()
        {
            await FriendsManager.Instance.Initialize();

            // Title
            RefreshOnline();

            // Player Id
            SetPlayerId(AuthenticationService.Instance.PlayerId);

            // Status
            SetStatus(Availability.Online);

            // Requests
            foreach (Relationship request in FriendsService.Instance.IncomingFriendRequests)
            {
                AddRequestUI(request);
            }

            // Friends
            foreach (Relationship friend in FriendsService.Instance.Friends)
            {
                AddFriendUI(friend);
            }

            StartCoroutine(canvasGroup.FadeRoutine(true, 0.25f));


            FriendsService.Instance.PresenceUpdated += OnPresenceUpdated;
            FriendsService.Instance.RelationshipAdded += OnRelationshipAdded;
            FriendsService.Instance.RelationshipDeleted += OnRelationshipDeleted;
        }
        private void Shutdown()
        {
            FriendsService.Instance.PresenceUpdated -= OnPresenceUpdated;
            FriendsService.Instance.RelationshipAdded -= OnRelationshipAdded;
            FriendsService.Instance.RelationshipDeleted -= OnRelationshipDeleted;
        }

        public void SetPlayerId(string playerId)
        {
            playerIdText.text = playerId;
        }
        public void SetStatus(Availability status)
        {
            FriendsManager.Instance.SetStatus(status);
            statusText.text = status.ToString();
        }

        private RelationshipUI AddRelationshipUI(Relationship relationship, RelationshipUI prefab, Toggle toggle)
        {
            RelationshipUI relationshipUI = Instantiate(prefab, contentRT);
            relationshipUI.Setup(this, relationship);
            relationshipUI.transform.SetSiblingIndex(toggle.transform.GetSiblingIndex() + 1);

            toggle.onValueChanged.AddListener(relationshipUI.gameObject.SetActive);

            return relationshipUI;
        }
        public FriendUI AddFriendUI(Relationship friend)
        {
            FriendUI friendUI = AddRelationshipUI(friend, friendPrefab, friendsToggle) as FriendUI;
            friends[friend.Member.Id] = friendUI;
            return friendUI;
        }
        public RequestUI AddRequestUI(Relationship request)
        {
            RequestUI requestUI = AddRelationshipUI(request, requestPrefab, requestsToggle) as RequestUI;
            requests[request.Member.Id] = requestUI;
            return requestUI;
        }

        public void Join(string lobbyId)
        {
            onJoin?.Invoke(lobbyId);
        }
        public void Request()
        {
            InputDialog.Input(LocalizationUtility.Localize("friends_request_title"), LocalizationUtility.Localize("friends_request_placeholder"), LocalizationUtility.Localize("friends_request_submit"), onSubmit: delegate (string playerId)
            {
                foreach (Relationship request in FriendsService.Instance.IncomingFriendRequests)
                {
                    if (request.Member.Id == playerId)
                    {
                        requests[playerId].Accept();
                        return;
                    }
                }
                FriendsManager.Instance.SendFriendRequest(playerId);
            });
        }
        public void RefreshOnline()
        {
            int onlineFriends = 0;
            foreach (Relationship friend in FriendsService.Instance.Friends)
            {
                if (friend.Member.Presence.Availability == Availability.Online)
                {
                    onlineFriends++;
                }
            }
            titleText.SetArguments(onlineFriends);
        }
        public async void Refresh()
        {
            await FriendsService.Instance.ForceRelationshipsRefreshAsync();

            foreach (Relationship friend in FriendsService.Instance.Friends)
            {
                friends[friend.Member.Id].Setup(this, friend);
            }

            RefreshOnline();
        }

        public void OnStatusSelected(int index)
        {
            if (!FriendsManager.Instance.Initialized) return;

            switch (index)
            {
                case 0:
                    SetStatus(Availability.Online);
                    break;
                case 1:
                    SetStatus(Availability.Busy);
                    break;
                case 2:
                    SetStatus(Availability.Invisible);
                    break;
            }
        }
        public void OnPresenceUpdated(IPresenceUpdatedEvent updateEvent)
        {
            if (friends.ContainsKey(updateEvent.ID))
            {
                friends[updateEvent.ID]?.SetPresence(updateEvent.Presence);
                RefreshOnline();
            }
        }
        public void OnRelationshipDeleted(IRelationshipDeletedEvent deleteEvent)
        {
            Relationship relationship = deleteEvent.Relationship;
            string playerId = relationship.Member.Id;
            switch (relationship.Type)
            {
                case RelationshipType.FriendRequest:
                    if (requests.ContainsKey(playerId))
                    {
                        Destroy(requests[playerId].gameObject);
                        requests.Remove(playerId);
                    }
                    break;
                case RelationshipType.Friend:
                    if (friends.ContainsKey(playerId))
                    {
                        Destroy(friends[playerId].gameObject);
                        friends.Remove(playerId);
                    }
                    RefreshOnline();
                    break;
            }
        }
        public void OnRelationshipAdded(IRelationshipAddedEvent addEvent)
        {
            Relationship relationship = addEvent.Relationship;
            switch (relationship.Type)
            {
                case RelationshipType.FriendRequest:
                    AddRequestUI(relationship);
                    break;
                case RelationshipType.Friend:
                    AddFriendUI(relationship);
                    RefreshOnline();
                    break;
            }
        }
        #endregion
    }
}