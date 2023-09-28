using UnityEngine;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class RelationshipsView : MonoBehaviour, IRelationshipsView
    {
        [SerializeField] LocalPlayerViewUGUI m_LocalPlayerViewUGUI;
        [SerializeField] AddFriendViewUGUI m_AddFriendViewUGUI;
        [SerializeField] NavBarViewUGUI m_NavBarViewUGUI;
        [SerializeField] FriendsViewUGUI m_FriendsViewUGUI;
        [SerializeField] RequestsViewUGUI m_RequestsViewUGUI;
        [SerializeField] BlocksViewUGUI m_BlocksViewUGUI;
        
        public ILocalPlayerView LocalPlayerView => m_LocalPlayerViewUGUI;
        public IRelationshipBarView RelationshipBarView => m_NavBarViewUGUI;
        public IAddFriendView AddFriendView => m_AddFriendViewUGUI;
        public IFriendsListView FriendsListView => m_FriendsViewUGUI;
        public IRequestListView RequestListView => m_RequestsViewUGUI;
        public IBlockedListView BlockListView => m_BlocksViewUGUI;

        public void Init()
        {
            m_AddFriendViewUGUI.Init();
            m_NavBarViewUGUI.Init(new IListView[] { FriendsListView, RequestListView, BlockListView });
            m_NavBarViewUGUI.onShowAddFriend += () => { m_AddFriendViewUGUI.Show(); };
        }
    }
}