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
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }

        public void Setup()
        {
            mapOS.SetupUsingEnum<Map>();
            mapOS.Select(Map.Island);

            modeOS.SetupUsingEnum<Mode>();
            modeOS.Select(Mode.Adventure);
        }

        public void Play()
        {
            bool spawnNPC = npcToggle.isOn;
            bool enablePVE = pveToggle.isOn;
            bool creativeMode = ((Mode)modeOS.Selected) == Mode.Creative;

            MapManager.Instance.Map = mapOS.Options[mapOS.Selected].Name;


            NetworkManager.Singleton.NetworkConfig.NetworkTransport = NetworkTransportPicker.Instance.GetTransport<UnityTransport>("localhost");
            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(JsonUtility.ToJson(new ConnectionData(usernameInputField.text, "")));

            WorldManager.Instance.World = new WorldSP(false); // TODO: Use CreativeMode OptionSelector

            NetworkManager.Singleton.StartHost();
        }
        #endregion
    }
}