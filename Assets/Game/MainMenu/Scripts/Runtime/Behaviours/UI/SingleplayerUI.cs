// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Text;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SingleplayerUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private OptionSelector mapOS;
        [SerializeField] private OptionSelector modeOS;
        [SerializeField] private Toggle npcToggle;
        [SerializeField] private Toggle pveToggle;
        [SerializeField] private Toggle unlimitedToggle;
        #endregion

        #region Properties
        private ushort Port
        {
            get => (ushort)PlayerPrefs.GetInt("LOCALHOST_PORT", 7771);
            set
            {
                PlayerPrefs.SetInt("LOCALHOST_PORT", NetworkTransport.ConnectionData.Port = value);
            }
        }

        private UnityTransport NetworkTransport => NetworkTransportPicker.Instance.GetTransport<UnityTransport>("localhost");
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }
        private void OnDestroy()
        {
            if (NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnFailed;
            }
        }

        public void Setup()
        {
            mapOS.SetupUsingEnum<Map>();
            mapOS.Select(Map.Island, false);

            modeOS.SetupUsingEnum<Mode>();
            modeOS.OnSelected.AddListener(delegate (int option)
            {
                unlimitedToggle.transform.parent.parent.gameObject.SetActive(option == 1); // only show unlimited toggle for creative mode
            });
            modeOS.Select(Mode.Adventure);

            NetworkManager.Singleton.OnClientDisconnectCallback += OnFailed;
        }

        public void Play()
        {
            string mapName = ((Map)mapOS.Selected).ToString();
            bool creativeMode = ((Mode)modeOS.Selected) == Mode.Creative;
            bool spawnNPC = npcToggle.isOn;
            bool enablePVE = pveToggle.isOn;
            bool unlimited = unlimitedToggle.isOn && creativeMode;

            NetworkManager.Singleton.NetworkConfig.NetworkTransport = NetworkTransport;
            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(JsonUtility.ToJson(new ConnectionData("", usernameInputField.text, "")));
            NetworkTransport.ConnectionData.Port = Port;

            WorldManager.Instance.World = new WorldSP(mapName, creativeMode, spawnNPC, enablePVE, unlimited);

            NetworkManager.Singleton.StartHost();
        }

        private void OnFailed(ulong clientId)
        {
            if (WorldManager.Instance.World is WorldSP)
            {
                Port++;
                Play();
            }
        }
        #endregion
    }
}