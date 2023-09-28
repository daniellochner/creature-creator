using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.Samples.Friends.UIToolkit
{
    public class FriendsListViewUIToolkit : IFriendsListView, IListView
    {
        public Action<string> onRemove { get; set; }
        public Action<string> onBlock { get; set; }

        const string k_FriendsListViewName = "friends-list";

        ListView m_FriendListView;
        VisualElement m_RequestListViewParent;

        /// <summary>
        /// Finds and binds the UI Elements with the controller
        /// </summary>
        /// <param name="viewParent">One of the parents of the friends-list (In RelationShipBarView.uxml)</param>
        /// <param name="friendEntryTemplate">The Friends Template (FriendListEntry.uxml) </param>
        /// <param name="boundFriendProfiles">The List of users we bind the listview to.</param>
        public FriendsListViewUIToolkit(VisualElement viewParent, VisualTreeAsset friendEntryTemplate)
        {
            m_FriendListView = viewParent.Q<ListView>(k_FriendsListViewName);
            var scrollView = m_FriendListView.Q<ScrollView>();
            scrollView.style.position = Position.Relative;
            m_RequestListViewParent = m_FriendListView.parent;

            m_FriendListView.makeItem = () =>
            {
                var newListEntry = friendEntryTemplate.Instantiate();
                var newListEntryLogic = new FriendEntryViewUIToolkit(newListEntry.contentContainer);
                newListEntry.userData = newListEntryLogic;
                return newListEntry;
            };
        }

        public void BindList(List<FriendsEntryData> friendEntryDatas)
        {
            m_FriendListView.bindItem = (item, index) =>
            {
                var friendControl = item.userData as FriendEntryViewUIToolkit;
                friendControl.Show();
                var friendData = friendEntryDatas[index];
                friendControl.Refresh(friendData.Name, friendData.Activity, friendData.Availability);
                friendControl.onRemoveFriend = () =>
                {
                    onRemove?.Invoke(friendData.Id);
                    friendControl.Hide();
                };

                friendControl.onBlockFriend = () =>
                {
                    onBlock?.Invoke(friendData.Id);
                    friendControl.Hide();
                };
            };
            m_FriendListView.itemsSource = friendEntryDatas;
            Refresh();
        }

        public void Show()
        {
            m_RequestListViewParent.style.display = DisplayStyle.Flex;
            Refresh();
        }

        public void Hide()
        {
            m_RequestListViewParent.style.display = DisplayStyle.None;
        }

        /// <summary>
        /// Force a refresh now!
        /// </summary>
        public void Refresh()
        {
#if UNITY_2020
            m_FriendListView.Refresh();
#elif UNITY_2021_1_OR_NEWER
            m_FriendListView.RefreshItems();
#endif
        }
    }
}
