// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using System.IO;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DemoManager : MonoBehaviourSingleton<DemoManager>
    {
        #region Methods
        private void Start()
        {
            Setup();
        }
        private void Setup()
        {
            Database keys = DatabaseManager.GetDatabase("Keys");
            if (keys.GetEntry<SecretKey>("Creature") == null || keys.GetEntry<SecretKey>("Progress") == null)
            {
                Debug.LogError("You must supply your own creature and progress encryption keys. [Click here to highlight the keys database]", keys);
            }

            NetworkShutdownManager.Instance.OnUncontrolledShutdown += OnUncontrolledShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledClientShutdown += OnUncontrolledClientShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledHostShutdown += OnUncontrolledHostShutdown;
        }

        public void OnUncontrolledShutdown()
        {
            SceneManager.LoadScene("MainMenu");
        }
        public void OnUncontrolledClientShutdown()
        {
            InformationDialog.Inform("Disconnected!", "You lost connection.");
        }
        public void OnUncontrolledHostShutdown()
        {
            InformationDialog.Inform("Disconnected!", "The host lost connection.");
        }

        public void OnInactivityKick()
        {
            InformationDialog.Instance.Close(true);
        }
        public void OnInactivityWarn(int warnTime)
        {
            InformationDialog.Inform("Inactivity Warning!", $"You will be kicked due to inactivity in {warnTime} seconds.", "Cancel");
        }
        #endregion
    }
}