using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.Samples.Friends.UIToolkit
{
    public class RequestListViewUIToolkit : IRequestListView
    {
        public Action<string> onAccept { get; set; }
        public Action<string> onDecline { get; set; }
        public Action<string> onBlock { get; set; }

        const string k_RequestListViewName = "request-list";

        ListView m_RequestListView;
        VisualElement m_RequestListViewParent;

        /// <summary>
        /// Finds and binds the UI Elements with the controller
        /// </summary>
        /// <param name="viewParent">One of the parents of the friends-list (In RelationShipBarView.uxml)</param>
        /// <param name="requestEntryTemplate">The Friends Template (FriendListEntry.uxml) </param>
        /// <param name="boundRequestProfiles">The List of users we bind the listview to.</param>
        public RequestListViewUIToolkit(VisualElement viewParent, VisualTreeAsset requestEntryTemplate)
        {
            m_RequestListView = viewParent.Q<ListView>(k_RequestListViewName);
            var scrollView = m_RequestListView.Q<ScrollView>();
            scrollView.style.position = Position.Relative;

            m_RequestListViewParent = m_RequestListView.parent;

            m_RequestListView.makeItem = () =>
            {
                var newListEntry = requestEntryTemplate.Instantiate();
                var newListEntryLogic = new RequestEntryViewUIToolkit(newListEntry.contentContainer);
                newListEntry.userData = newListEntryLogic;
                return newListEntry;
            };
        }

        public void BindList(List<PlayerProfile> playerProfiles)
        {
            m_RequestListView.bindItem = (item, index) =>
            {
                var requestControl = item.userData as RequestEntryViewUIToolkit;
                requestControl.Show();
                var userProfile = playerProfiles[index];
                requestControl.Refresh(userProfile.Name);
                requestControl.onAccept = () =>
                {
                    onAccept?.Invoke(userProfile.Id);
                    requestControl.Hide();
                };

                requestControl.onDecline = () =>
                {
                    onDecline?.Invoke(userProfile.Id);
                    requestControl.Hide();
                };

                requestControl.onBlockFriend = () =>
                {
                    onBlock?.Invoke(userProfile.Id);
                    requestControl.Hide();
                };
            };

            m_RequestListView.itemsSource = playerProfiles;
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
            m_RequestListView.Refresh();
#elif UNITY_2021_1_OR_NEWER
            m_RequestListView.RefreshItems();
#endif
        }
    }
}