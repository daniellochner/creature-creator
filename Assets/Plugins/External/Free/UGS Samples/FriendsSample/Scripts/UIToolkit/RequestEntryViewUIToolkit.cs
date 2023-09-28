using System;
using UnityEngine.UIElements;

namespace Unity.Services.Samples.Friends.UIToolkit
{
    public class RequestEntryViewUIToolkit
    {
        const string k_RequestEntryViewName = "request-friend-view";

        public Action onAccept;
        public Action onDecline;
        public Action onBlockFriend;

        Label m_PlayerName;

        VisualElement m_requestEntryView;

        public RequestEntryViewUIToolkit(VisualElement viewParent)
        {
            m_requestEntryView = viewParent.Q(k_RequestEntryViewName);
            m_PlayerName = m_requestEntryView.Q<Label>("player-name-label");
            var acceptButton = m_requestEntryView.Q<Button>("accept-button");

            acceptButton.RegisterCallback<ClickEvent>(_ =>
            {
                onAccept?.Invoke();
            });

            var denyButton = m_requestEntryView.Q<Button>("remove-button");
            denyButton.RegisterCallback<ClickEvent>(_ =>
            {
                onDecline?.Invoke();
            });

            var blockButton = m_requestEntryView.Q<Button>("block-button");
            blockButton.RegisterCallback<ClickEvent>(_ =>
            {
                onBlockFriend?.Invoke();
            });
        }

        public void Refresh(string name)
        {
            m_PlayerName.text = name;
        }

        public void Show()
        {
            m_requestEntryView.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            m_requestEntryView.style.display = DisplayStyle.None;
        }
    }
}
