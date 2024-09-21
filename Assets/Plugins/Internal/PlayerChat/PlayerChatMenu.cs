using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using static DanielLochner.Assets.SimpleSideMenu;

namespace DanielLochner.Assets
{
    public class PlayerChatMenu : MonoBehaviourSingleton<PlayerChatMenu>
    {
        public SimpleSideMenu sideMenu;
        public TMP_InputField messageInputField;
        public TextMeshProUGUI messagePrefab;
        public Transform messagesRoot;
        public Keybind keybind;
        public Color friendColor;
        public TextMeshProUGUI unreadMessagesText;

        private int numUnreadMessages;


        public int NumUnreadMessages
        {
            get => numUnreadMessages;
            set
            {
                numUnreadMessages = value;
                unreadMessagesText.text = numUnreadMessages.ToString();
                unreadMessagesText.gameObject.SetActive(numUnreadMessages > 0);
            }
        }

        public bool IsOpen
        {
            get => sideMenu.CurrentState == State.Open;
        }


        private void OnEnable()
        {
            if (PlayerChatManager.Instance != null)
            {
                PlayerChatManager.Instance.OnMessageSent += OnMessageSent;
                PlayerChatManager.Instance.OnMessageReceived += OnMessageReceived;
            }
        }
        private void OnDisable()
        {
            if (PlayerChatManager.Instance != null)
            {
                PlayerChatManager.Instance.OnMessageSent -= OnMessageSent;
                PlayerChatManager.Instance.OnMessageReceived -= OnMessageReceived;
            }
        }

        private void Start()
        {
            sideMenu.onStateChanged.AddListener(delegate
            {
                NumUnreadMessages = 0;
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) && (sideMenu.CurrentState == State.Open) && !string.IsNullOrEmpty(messageInputField.text))
            {
                messageInputField.text = messageInputField.text.TrimEnd(); // Trim end new-line character
                PlayerChatManager.Instance.TrySendChatMessage(messageInputField.text);
            }

            if (InputUtility.GetKeyDown(keybind) && !InputDialog.Instance.IsOpen)
            {
                sideMenu.Open();
                EventSystem.current.SetSelectedGameObject(messageInputField.gameObject);
            }
        }

        public void TrySendMessage()
        {
            PlayerChatManager.Instance.TrySendChatMessage(messageInputField.text);
        }

        private void OnMessageSent(string message)
        {
            messageInputField.text = null;
        }
        private void OnMessageReceived(ulong clientId, string message)
        {
            PlayerData data = NetworkPlayersManager.Instance.Players[clientId];

            var text = Instantiate(messagePrefab, messagesRoot);
            string username = data.username.NoParse().ToColour(FriendsManager.Instance.IsFriended(data.playerId) ? friendColor : new(0.75f, 0.75f, 0.75f));
            text.text = $"{username}: {message}";

            if (!IsOpen && !NetworkUtils.IsPlayer(clientId))
            {
                NumUnreadMessages++;
            }
        }
    }
}