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

        public void Play()
        {
            NetworkManager.Singleton.NetworkConfig.NetworkTransport = NetworkTransportPicker.Instance.GetTransport<UnityTransport>("localhost");
            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(JsonUtility.ToJson(new ConnectionData(usernameInputField.text, "")));

            WorldManager.Instance.World = new WorldSP(false); // TODO: Use CreativeMode OptionSelector

            NetworkManager.Singleton.StartHost();
        }
        #endregion
    }
}