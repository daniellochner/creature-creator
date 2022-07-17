// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Text;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SingleplayerUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private string defaultMap;
        #endregion

        #region Methods
        public void PlayDefault()
        {
            MapManager.Instance.Map = defaultMap;
            Play();
        }
        public void PlaySandbox()
        {
            if (!ProgressManager.Instance.IsComplete)
            {
                InformationDialog.Inform("Sandbox Locked", "You must collect all parts and patterns before you may access the sandbox!");
            }
            else
            {
                MapManager.Instance.Map = "Sandbox";
                Play();
            }
        }

        public void Play()
        {
            NetworkManager.Singleton.NetworkConfig.NetworkTransport = NetworkTransportPicker.Instance.GetTransport<UnityTransport>("localhost");
            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(JsonUtility.ToJson(new ConnectionData(usernameInputField.text, "")));

            SetupGame.IsMultiplayer = false;

            NetworkManager.Singleton.StartHost();
        }
        #endregion
    }
}