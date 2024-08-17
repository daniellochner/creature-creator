using DanielLochner.Assets;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.EventSystems;
using static DanielLochner.Assets.SimpleSideMenu;
using ProfanityDetector;
using System.Collections.Generic;

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
        public bool checkForProfanity;

        private float lastMessageTime;

        public bool IsOpen
        {
            get => sideMenu.CurrentState == State.Open;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) && (sideMenu.CurrentState == State.Open) && !string.IsNullOrEmpty(messageInputField.text))
            {
                messageInputField.text = messageInputField.text.TrimEnd(); // Trim end new-line character
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
            string message = messageInputField.text.Trim();

            if (string.IsNullOrEmpty(messageInputField.text))
            {
                return;
            }

            message = message.NoParse();

            if (message.Length > maxMessageLength)
            {
                return;
            }

            if (checkForProfanity)
            {
                ProfanityFilter filter = new ProfanityFilter();
                if (filter.ContainsProfanity(message))
                {
                    IReadOnlyCollection<string> profanities = filter.DetectAllProfanities(message);
                    if (profanities.Count > 0)
                    {
                        InformationDialog.Inform(LocalizationUtility.Localize("profanity_detected_title"), LocalizationUtility.Localize("profanity_detected_message_terms", string.Join(", ", profanities)));
                    }
                    else
                    {
                        InformationDialog.Inform(LocalizationUtility.Localize("profanity_detected_title"), LocalizationUtility.Localize("profanity_detected_message"));
                    }
                    return;
                }
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