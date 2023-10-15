using Unity.Services.Friends;
using Unity.Services.Friends.Models;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class RequestUI : RelationshipUI
    {
        #region Methods
        public void Accept()
        {
            FriendsManager.Instance.AcceptFriendRequest(relationship.Member.Id, delegate (Relationship friend)
            {
                menu.AddFriendUI(friend);
                menu.CountOnline();
            });
            Destroy(gameObject);
        }
        public void Reject()
        {
            FriendsManager.Instance.RejectFriendRequest(relationship.Member.Id);
            Destroy(gameObject);
        }
        public void Block()
        {
            ConfirmationDialog.Confirm(LocalizationUtility.Localize("friends_block_title"), LocalizationUtility.Localize("friends_block_message", relationship.Member.Profile.Name), onYes: delegate
            {
                FriendsManager.Instance.BlockPlayer(relationship.Member.Id);
                Destroy(gameObject);
            });
        }
        #endregion
    }
}