using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class AddFriendViewUGUI : MonoBehaviour, IAddFriendView
    {
        [SerializeField] Button m_AddFriendButton = null;
        [SerializeField] Button m_CloseButton = null;
        [SerializeField] Button m_BackgroundButton = null;
        [SerializeField] TMP_InputField m_NameInputField = null;
        [SerializeField] TextMeshProUGUI m_RequestResultText = null;
        public Action<string> onFriendRequestSent { get; set; }

        public void Init()
        {
            var playerName = string.Empty;
            m_NameInputField.onValueChanged.AddListener((value) => { playerName = value; });
            m_AddFriendButton.onClick.AddListener(() =>
            {
                m_RequestResultText.text = string.Empty;
                onFriendRequestSent?.Invoke(playerName);
            });
            m_BackgroundButton.onClick.AddListener(Hide);
            m_CloseButton.onClick.AddListener(Hide);
            Hide();
        }

        public void FriendRequestSuccess()
        {
            m_RequestResultText.text = "Friend request sent!";
            Hide();
        }

        public void FriendRequestFailed()
        {
            m_RequestResultText.text = "Friend request failed!";
        }

        public void Show()
        {
            m_RequestResultText.text = string.Empty;
            m_NameInputField.SetTextWithoutNotify("");
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
