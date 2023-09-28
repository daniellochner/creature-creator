using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.Samples.Friends.UIToolkit
{
    public class BlockedEntryViewUIToolkit
    {
        const string k_BlockEntryViewName = "block-entry-view";
        public Action onUnBlock;
        Label m_PlayerName;

        VisualElement m_BlockedEntryRoot;

        public BlockedEntryViewUIToolkit(VisualElement viewParent)
        {
            m_BlockedEntryRoot = viewParent.Q(k_BlockEntryViewName);
            m_PlayerName = m_BlockedEntryRoot.Q<Label>("player-name-label");
            var blockButton = m_BlockedEntryRoot.Q<Button>("unblock-button");
            blockButton.RegisterCallback<ClickEvent>(_ =>
                {
                    onUnBlock?.Invoke();
                }
            );
        }

        public void Refresh(string name)
        {
            m_PlayerName.text = name;
        }

        public void Show()
        {
            m_BlockedEntryRoot.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            m_BlockedEntryRoot.style.display = DisplayStyle.None;
        }
    }
}
