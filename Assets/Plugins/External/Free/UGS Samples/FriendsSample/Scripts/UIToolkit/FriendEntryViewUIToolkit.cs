using System;
using Unity.Services.Friends.Models;
using UnityEngine.UIElements;

namespace Unity.Services.Samples.Friends.UIToolkit
{
    public class FriendEntryViewUIToolkit
    {
        const string k_FriendEntryViewName = "friend-entry-view";

        public Action onRemoveFriend;
        public Action onBlockFriend;
        Label m_PlayerName;
        TextField m_PlayerActivity;
        VisualElement m_PlayerStatusCircle;
        VisualElement m_FriendEntryView;

        public FriendEntryViewUIToolkit(VisualElement documentParent)
        {
            m_FriendEntryView = documentParent.Q(k_FriendEntryViewName);
            m_PlayerName = m_FriendEntryView.Q<Label>("player-name-label");
            m_PlayerStatusCircle = m_FriendEntryView.Q<VisualElement>("player-status-circle");
            m_PlayerActivity = m_FriendEntryView.Q<TextField>("player-activity-field");
            m_PlayerActivity.focusable = false;
            m_PlayerActivity.isReadOnly = true;
            var removeFriendButton = m_FriendEntryView.Q<Button>("remove-button");
            removeFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                onRemoveFriend?.Invoke();
            });
            var blockFriendButton = m_FriendEntryView.Q<Button>("block-button");
            blockFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                onBlockFriend?.Invoke();
            });
        }

        public void Refresh(string name, string activity, Availability presenceStatus)
        {
            m_PlayerName.text = name;
            m_PlayerActivity.SetValueWithoutNotify(activity);

            var index = (int)presenceStatus - 1;
            var presenceColor = ColorUtils.GetPresenceColor(index);
            m_PlayerStatusCircle.style.backgroundColor = presenceColor;
        }

        public void Show()
        {
            m_FriendEntryView.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            m_FriendEntryView.style.display = DisplayStyle.None;
        }
    }
}