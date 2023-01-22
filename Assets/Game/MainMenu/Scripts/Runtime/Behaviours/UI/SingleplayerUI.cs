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

        #region Methods
        private void Start()
        {
            Setup();
        }

        public void Setup()
        {
            mapOS.SetupUsingEnum<Map>(false);
            mapOS.Select(Map.Island, false);

            modeOS.SetupUsingEnum<Mode>(true);
            modeOS.OnSelected.AddListener(delegate (int option)
            {
                unlimitedToggle.transform.parent.parent.gameObject.SetActive(option == 1); // only show unlimited toggle for creative mode
            });
            modeOS.Select(Mode.Adventure);
        }

        public void Play()
        {
            string mapName = ((Map)mapOS.Selected).ToString();
            bool creativeMode = ((Mode)modeOS.Selected) == Mode.Creative;
            bool spawnNPC = npcToggle.isOn;
            bool enablePVE = pveToggle.isOn;
            bool unlimited = unlimitedToggle.isOn && creativeMode;

            NetworkManager.Singleton.NetworkConfig.NetworkTransport = NetworkTransportPicker.Instance.GetTransport<UnityTransport>("localhost");
            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(JsonUtility.ToJson(new ConnectionData("", usernameInputField.text, "")));

            WorldManager.Instance.World = new WorldSP(mapName, creativeMode, spawnNPC, enablePVE, unlimited);

            NetworkManager.Singleton.StartHost();
        }
        #endregion
    }
}