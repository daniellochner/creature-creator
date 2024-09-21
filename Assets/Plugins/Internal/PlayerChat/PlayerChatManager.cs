using UnityEngine;
using Unity.Netcode;
using ProfanityDetector;
using System.Collections.Generic;
using System;

namespace DanielLochner.Assets
{
    public class PlayerChatManager : NetworkSingleton<PlayerChatManager>
    {
        public PlayerChatMenu chatMenu;
        public float messageCooldown = 1f;
        public int maxMessageLength = 256;
        public Color friendColor;
        public Keybind keybind;
        public bool checkForProfanity;

        private float lastMessageTime;

        public Action<string> OnMessageSent { get; set; }
        public Action<ulong, string> OnMessageReceived { get; set; }

        public void Start()
        {
            Instantiate(chatMenu, Dynamic.OverlayCanvas);
        }

        public void TrySendChatMessage(string text)
        {
            string message = text.Trim();

            if (string.IsNullOrEmpty(text))
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

                OnMessageReceived(clientId, message);
                SendChatMessageServerRpc(clientId, message);

                lastMessageTime = Time.time;

                OnMessageSent?.Invoke(message);
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
            OnMessageReceived?.Invoke(clientId, message);
        }
    }
}