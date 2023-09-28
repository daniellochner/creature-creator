using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.Samples.Friends.UIToolkit
{
    public class RelationshipsViewUIToolkit : MonoBehaviour, IRelationshipsView
    {
        [SerializeField] UIDocument m_SocialUIDoc;

        [SerializeField] VisualTreeAsset m_FriendEntryTemplate;

        [SerializeField] VisualTreeAsset m_RequestEntryTemplate;

        [SerializeField] VisualTreeAsset m_BlockedEntryTemplate;

        public ILocalPlayerView LocalPlayerView { get; private set; }
        public IRelationshipBarView RelationshipBarView { get; private set; }
        public IAddFriendView AddFriendView { get; private set; }
        public IFriendsListView FriendsListView { get; private set; }
        public IRequestListView RequestListView { get; private set; }
        public IBlockedListView BlockListView { get; private set; }

        const string k_LocalPlayerViewName = "local-player-entry";

        public void Init()
        {
            if (m_SocialUIDoc == null)
                m_SocialUIDoc = GetComponent<UIDocument>();
            var root = m_SocialUIDoc.rootVisualElement;

            var localPlayerControlView = root.Q(k_LocalPlayerViewName);

            LocalPlayerView = new LocalPlayerViewUIToolkit(localPlayerControlView);

            AddFriendView = new AddFriendViewUIToolkit(root);

            FriendsListView = new FriendsListViewUIToolkit(root, m_FriendEntryTemplate);
            RequestListView = new RequestListViewUIToolkit(root, m_RequestEntryTemplate);
            BlockListView = new BlockedListViewUIToolkit(root, m_BlockedEntryTemplate);

            var listViews = new IListView[] { FriendsListView, RequestListView, BlockListView };
            RelationshipBarView = new NavBarViewUIToolkit(root, listViews);
            RelationshipBarView.onShowAddFriend += () =>
            {
                AddFriendView.Show();
            };
        }
    }
}
