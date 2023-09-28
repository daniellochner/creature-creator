using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class FriendsViewUGUI : ListViewUGUI, IFriendsListView
    {
        [SerializeField] RectTransform m_ParentTransform = null;
        [SerializeField] FriendEntryViewUGUI m_FriendEntryViewPrefab = null;

        List<FriendEntryViewUGUI> m_FriendEntries = new List<FriendEntryViewUGUI>();
        List<FriendsEntryData> m_FriendsEntryDatas = new List<FriendsEntryData>();

        public Action<string> onRemove { get; set; }
        public Action<string> onBlock { get; set; }

        public void BindList(List<FriendsEntryData> friendEntryDatas)
        {
            m_FriendsEntryDatas = friendEntryDatas;
        }

        public override void Refresh()
        {
            m_FriendEntries.ForEach(entry => Destroy(entry.gameObject));
            m_FriendEntries.Clear();

            foreach (var friendsEntryData in m_FriendsEntryDatas)
            {
                var entry = Instantiate(m_FriendEntryViewPrefab, m_ParentTransform);
                entry.Init(friendsEntryData.Name, friendsEntryData.Availability, friendsEntryData.Activity);
                entry.removeFriendButton.onClick.AddListener(() =>
                {
                    onRemove?.Invoke(friendsEntryData.Id);
                    entry.gameObject.SetActive(false);
                });
                entry.blockFriendButton.onClick.AddListener(() =>
                {
                    onBlock?.Invoke(friendsEntryData.Id);
                    entry.gameObject.SetActive(false);
                });
                m_FriendEntries.Add(entry);
            }
        }
    }
}