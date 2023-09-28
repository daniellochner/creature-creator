using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Unity.Services.Samples.Friends.UIToolkit
{
    public class AddFriendViewUIToolkit : IAddFriendView
    {
        const string k_AddFriendViewName = "add-friend-bg";
        public Action<string> onFriendRequestSent { get; set; }

        VisualElement m_AddFriendView;
        Label m_FeedbackLabel;

        public AddFriendViewUIToolkit(VisualElement viewParent)
        {
            m_AddFriendView = viewParent.Q(k_AddFriendViewName);

            var exitButton = m_AddFriendView.Q<Button>("exit-button");
            exitButton.RegisterCallback<ClickEvent>((e) =>
            {
                Hide();
            });

            var clickOffButton = m_AddFriendView.Q<Button>("add-friend-clickoff-button");
            clickOffButton.RegisterCallback<ClickEvent>((e) =>
            {
                Hide();
            });
            m_FeedbackLabel = m_AddFriendView.Q<Label>("feedback-label");
            var addFriendField = m_AddFriendView.Q<TextField>("search-field");

            //Support for Enter and Numpad Enter
            addFriendField.Q(TextField.textInputUssName).RegisterCallback<KeyDownEvent>(
                evt =>
                {
                    if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                    {
                        onFriendRequestSent?.Invoke(addFriendField.text);
                    }
                });
            var requestFriendButton = m_AddFriendView.Q<Button>("add-button");
            requestFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                onFriendRequestSent?.Invoke(addFriendField.text);
            });
        }

        public async void FriendRequestSuccess()
        {
            m_FeedbackLabel.text = "Friend request sent!";
            m_FeedbackLabel.style.color = Color.white;

            m_FeedbackLabel.style.opacity = 1;
            await Task.Delay(2000);
            m_FeedbackLabel.style.opacity = 0;
            Hide();
        }

        public async void FriendRequestFailed()
        {
            m_FeedbackLabel.text = "Could not send request.";
            m_FeedbackLabel.style.color = Color.red;
            m_FeedbackLabel.style.opacity = 1;
            await Task.Delay(2000);
            m_FeedbackLabel.style.opacity = 0;
        }

        public void Show()
        {
            m_AddFriendView.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            m_AddFriendView.style.display = DisplayStyle.None;
        }
    }
}