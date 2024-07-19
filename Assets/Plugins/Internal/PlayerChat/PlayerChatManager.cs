using DanielLochner.Assets;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.EventSystems;
using static DanielLochner.Assets.SimpleSideMenu;

namespace DanielLochner.Assets
{
    public class PlayerChatManager : NetworkSingleton<PlayerChatManager>
    {
        public TMP_InputField messageInputField;
        public TextMeshProUGUI messagePrefab;
        public float messageCooldown = 1f;
        public int maxMessageLength = 256;
        public Transform messagesRoot;
        public SimpleSideMenu sideMenu;
        public Color friendColor;
        public Keybind keybind;

        private float lastMessageTime;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) && (sideMenu.CurrentState == State.Open) && !string.IsNullOrEmpty(messageInputField.text))
            {
                TrySendChatMessage();
            }

            if (InputUtility.GetKeyDown(keybind) && !InputDialog.Instance.IsOpen)
            {
                sideMenu.Open();
                EventSystem.current.SetSelectedGameObject(messageInputField.gameObject);
            }
        }

        private void OnEnable()
        {
            sideMenu.gameObject.SetActive(true);
        }
        private void OnDisable()
        {
            sideMenu.gameObject.SetActive(false);
        }

        public void TrySendChatMessage()
        {
            string message = messageInputField.text.NoParse();

            if (message.Length > maxMessageLength)
            {
                InformationDialog.Inform("Message Too Long", $"Your message cannot be more than {maxMessageLength} characters.");
                return;
            }

            if (Time.time > (lastMessageTime + messageCooldown))
            {
                ulong clientId = NetworkManager.Singleton.LocalClientId;

                SendChatMessageSelf(clientId, message);
                SendChatMessageServerRpc(clientId, message);

                lastMessageTime = Time.time;
                messageInputField.text = null;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendChatMessageServerRpc(ulong clientId, string message)
        {
            SendChatMessageClientRpc(clientId, message, NetworkUtils.DontSendTo(clientId));
        }

        [ClientRpc]
        public void SendChatMessageClientRpc(ulong clientId, string message, ClientRpcParams clientRpcParams)
        {
            SendChatMessageSelf(clientId, message);
        }

        public void SendChatMessageSelf(ulong clientId, string message)
        {
            PlayerData data = NetworkPlayersManager.Instance.Players[clientId];

            var text = Instantiate(messagePrefab, messagesRoot);
            string username = data.username.NoParse().ToColour(FriendsManager.Instance.IsFriended(data.playerId) ? friendColor : new(0.75f, 0.75f, 0.75f));
            text.text = $"{username}: {message}";
        }
    }
}