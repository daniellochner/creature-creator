using System;
using UnityEngine.UIElements;

namespace Unity.Services.Samples.Friends.UIToolkit
{
    public class NavBarViewUIToolkit : IRelationshipBarView
    {
        const string k_RelationshipsBarViewName = "nav-bar-view";
        public Action onShowAddFriend { get; set; }

        NavBarTab m_CurrentSelectedTab;
        NavBarTab[] m_NavBarTabs;

        public NavBarViewUIToolkit(VisualElement viewParent, IListView[] listViews)
        {
            var navBarView = viewParent.Q(k_RelationshipsBarViewName);
            Init(navBarView, listViews);

            var addFriendButton = navBarView.Q<Button>("add-friend-button");
            addFriendButton.RegisterCallback<ClickEvent>((_) => { onShowAddFriend?.Invoke(); });
        }

        private void Init(VisualElement navBarView, IListView[] listViews)
        {
            var friendsButton = new NavBarButtonUIToolkit(navBarView, "friends-button");
            var requestsButton = new NavBarButtonUIToolkit(navBarView, "requests-button");
            var blocksButton = new NavBarButtonUIToolkit(navBarView, "blocks-button");
            var navBarButtons = new[] { friendsButton, requestsButton, blocksButton };

            var count = listViews.Length;
            m_NavBarTabs = new NavBarTab[count];
            for (int i = 0; i < count; i++)
            {
                m_NavBarTabs[i] = new NavBarTab()
                {
                    ListView = listViews[i],
                    NavBarButton = navBarButtons[i]
                };
            }

            foreach (var navBarTab in m_NavBarTabs)
            {
                navBarTab.NavBarButton.onSelected += () => { ShowTab(navBarTab); };
                navBarTab.ListView.Hide();
            }
            //Select the friends tab by default
            m_NavBarTabs[0].NavBarButton.Select();
        }

        public void Refresh()
        {
            m_CurrentSelectedTab?.ListView.Refresh();
        }

        void ShowTab(NavBarTab navBarTab)
        {
            if (m_CurrentSelectedTab != null)
            {
                m_CurrentSelectedTab.NavBarButton.Deselect();
                m_CurrentSelectedTab.ListView.Hide();
            }

            if (navBarTab == m_CurrentSelectedTab)
            {
                m_CurrentSelectedTab = null;
                return;
            }

            m_CurrentSelectedTab = navBarTab;
            m_CurrentSelectedTab.ListView.Show();
        }

        private class NavBarTab
        {
            public IListView ListView;
            public NavBarButtonUIToolkit NavBarButton;
        }
    }
}