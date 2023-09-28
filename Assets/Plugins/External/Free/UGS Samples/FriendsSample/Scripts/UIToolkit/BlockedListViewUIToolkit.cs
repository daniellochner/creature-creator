using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Unity.Services.Samples.Friends.UIToolkit
{
    public class BlockedListViewUIToolkit : IBlockedListView
    {
        const string k_BlockedListViewName = "blocked-list";
        public Action<string> onUnblock { get; set; }

        ListView m_BlockedListView;
        VisualElement m_BlockedListViewParent;

        /// <summary>
        /// Finds and binds the UI Elements with the controller
        /// </summary>
        /// <param name="viewParent">One of the parents of the friends-list (In RelationShipBarView.uxml)</param>
        /// <param name="blockFriendTemplate">The Friends Template (FriendListEntry.uxml) </param>
        /// <param name="boundBlockedProfiles">The List of users we bind the listview to.</param>
        public BlockedListViewUIToolkit(VisualElement viewParent, VisualTreeAsset blockFriendTemplate)
        {
            m_BlockedListView = viewParent.Q<ListView>(k_BlockedListViewName);
            var scrollView = m_BlockedListView.Q<ScrollView>();
            scrollView.style.position = Position.Relative;
            m_BlockedListViewParent = m_BlockedListView.parent;
            m_BlockedListView.makeItem = () =>
            {
                var newListEntry = blockFriendTemplate.Instantiate();
                var newListEntryLogic = new BlockedEntryViewUIToolkit(newListEntry.contentContainer);
                newListEntry.userData = newListEntryLogic;
                return newListEntry;
            };
        }

        public void BindList(List<PlayerProfile> playerProfiles)
        {
            m_BlockedListView.bindItem = (item, index) =>
            {
                var blockedEntryControl = item.userData as BlockedEntryViewUIToolkit;
                blockedEntryControl.Show();
                var userProfile = playerProfiles[index];
                blockedEntryControl.Refresh(userProfile.Name);
                blockedEntryControl.onUnBlock = () =>
                {
                    onUnblock?.Invoke(userProfile.Id);
                    blockedEntryControl.Hide();
                };
            };

            m_BlockedListView.itemsSource = playerProfiles;
            Refresh();
        }

        public void Show()
        {
            m_BlockedListViewParent.style.display = DisplayStyle.Flex;
            Refresh();
        }

        public void Hide()
        {
            m_BlockedListViewParent.style.display = DisplayStyle.None;
        }

        /// <summary>
        /// Force a refresh now!
        /// </summary>
        public void Refresh()
        {
#if UNITY_2020
            m_BlockedListView.Refresh();
#elif UNITY_2021_1_OR_NEWER
            m_BlockedListView.RefreshItems();
#endif
        }
    }
}
