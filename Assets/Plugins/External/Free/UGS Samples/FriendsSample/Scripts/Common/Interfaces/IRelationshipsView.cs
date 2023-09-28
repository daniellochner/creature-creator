
namespace Unity.Services.Samples.Friends
{
    public interface IRelationshipsView
    {
        ILocalPlayerView LocalPlayerView { get; }
        IRelationshipBarView RelationshipBarView { get; }
        IAddFriendView AddFriendView { get; }
        IFriendsListView FriendsListView { get; }
        IRequestListView RequestListView { get; }
        IBlockedListView BlockListView { get; }

        void Init();
    }
}
