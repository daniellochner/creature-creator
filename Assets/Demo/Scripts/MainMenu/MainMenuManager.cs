// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using Unity.Netcode;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MainMenuManager : MonoBehaviourSingleton<MainMenuManager>
    {
        #region Methods
        public void Quit()
        {
            ConfirmationDialog.Confirm("Quit", "Are you sure you want to exit this application?", yesEvent: Application.Quit);
        }
        private void QuickLoad()
        {
            PlayerData playerData = new PlayerData()
            {
                username = "123"
            };
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(playerData));
            UnityTransport ipTransport = NetworkTransportPicker.Instance.GetTransport<UnityTransport>("IP");
            ipTransport.SetConnectionData("127.0.0.1", 1337);
            NetworkManager.Singleton.NetworkConfig.NetworkTransport = ipTransport;
            NetworkManager.Singleton.StartHost();
        }
        #endregion
    }
}