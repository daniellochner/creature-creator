using System.Collections.Generic;
using TMPro;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Notifications;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class FriendsMenu : MonoBehaviourSingleton<FriendsMenu>
    {
        #region Fields
        [SerializeField] private LocalizedText titleText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private TextMeshProUGUI connectionText;
        [SerializeField] private RectTransform contentRT;
        [SerializeField] private RequestUI requestPrefab;
        [SerializeField] private FriendUI friendPrefab;
        [SerializeField] private Toggle requestsToggle;
        [SerializeField] private Toggle friendsToggle;
        [SerializeField] private GameObject listGO;
        [SerializeField] private GameObject refreshGO;
        [SerializeField] private GameObject titleOnlineGO;
        [SerializeField] private GameObject titleNoneOnlineGO;
        [Space]
        [SerializeField] private UnityEvent<string> onJoin;

        private Dictionary<string, RequestUI> requests = new Dictionary<string, RequestUI>();
        private Dictionary<string, FriendUI> friends = new Dictionary<string, FriendUI>();
        private SimpleSideMenu simpleSideMenu;
        private CanvasGroup canvasGroup;
        #endregion

        #region Properties
        public Availability OnlineStatus
        {
            get => (Availability)PlayerPrefs.GetInt("ONLINE_STATUS", (int)Availability.Online);
            set => PlayerPrefs.SetInt("ONLINE_STATUS", (int)value);
        }
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();

            simpleSideMenu = GetComponent<SimpleSideMenu>();
            canvasGroup = GetComponent<CanvasGroup>();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            Shutdown();
        }

        public async void Setup()
        {
            StartCoroutine(canvasGroup.FadeRoutine(true, 0.25f));

            await FriendsManager.Instance.Initialize();

            Refresh();
            SetStatus(OnlineStatus);

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

        public void Hint()
        {
            InformationDialog.Inform(LocalizationUtility.Localize("friends_hint_title"), LocalizationUtility.Localize("friends_hint_message"));
        }
        public void Join(string lobbyId)
        {
            onJoin?.Invoke(lobbyId);
        }
        public void Request()
        {
            InputDialog.Input(LocalizationUtility.Localize("friends_request_title"), LocalizationUtility.Localize("friends_request_placeholder"), LocalizationUtility.Localize("friends_request_submit"), onSubmit: delegate (string playerName)
            {
                foreach (Relationship request in FriendsService.Instance.IncomingFriendRequests)
                {
                    if (request.Member.Profile.Name == playerName)
                    {
                        requests[request.Member.Id].Accept();
                        return;
                    }
                }
                FriendsManager.Instance.SendFriendRequestByName(playerName);
            });
        }
        public void CountOnline()
        {
            int online = 0;
            foreach (Relationship friend in FriendsService.Instance.Friends)
            {
                if (friend.Member.Presence.Availability == Availability.Online)
                {
                    online++;
                }
            }
            SetOnline(online);
        }
        public async void Refresh()
        {
            SetRefreshing(true);
            
            // Clear
            List<RelationshipUI> relationships = new List<RelationshipUI>();
            relationships.AddRange(requests.Values);
            relationships.AddRange(friends.Values);

            for (int i = 0; i < relationships.Count; i++)
            {
                Destroy(relationships[i].gameObject);
            }
            requests.Clear();
            friends.Clear();

            // Refresh
            try
            {
                await FriendsService.Instance.ForceRelationshipsRefreshAsync();
            }
            catch (FriendsServiceException e)
            {
                connectionText.text = e.Message;
                Debug.Log(e);
            }

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

            // Count Online
            CountOnline();

            SetRefreshing(false);
        }

        private void SetRefreshing(bool isRefreshing)
        {
            refreshGO.SetActive(isRefreshing);
            listGO.SetActive(!isRefreshing);
        }
        private void SetOnline(int online)
        {
            titleText.SetArguments(online);

            titleNoneOnlineGO.SetActive(online == 0);
            titleOnlineGO.SetActive(online > 0);
        }
        private void SetStatus(Availability status)
        {
            OnlineStatus = status;
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
                CountOnline();
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
                    CountOnline();
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
                    CountOnline();
                    break;
            }
        }
        #endregion
    }
}