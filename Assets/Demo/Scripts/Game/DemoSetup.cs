// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DemoSetup : MonoBehaviourSingleton<DemoSetup>
    {
        #region Methods
        public void Setup()
        {
            if (!NetworkManager.Singleton.IsHost) return;

            NetworkInactivityManager.Instance.OnInactivityKick += OnInactivityKick;
            NetworkInactivityManager.Instance.OnInactivityWarn += OnInactivityWarn;

            NetworkCreature networkCreature = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<NetworkCreature>();
            EditorManager.Instance.Player = networkCreature.Player;
            networkCreature.Player.gameObject.SetActive(true);
            EditorManager.Instance.UnlockedBodyParts = ProgressManager.Data.UnlockedBodyParts;
            EditorManager.Instance.UnlockedPatterns = ProgressManager.Data.UnlockedPatterns;
            EditorManager.Instance.Setup();

            CreatureInformationManager.Instance.Setup();
            NetworkCreaturesMenu.Instance.Setup();
            NetworkCreaturesManager.Instance.Setup();
        }

        private void OnInactivityKick()
        {
            InformationDialog.Instance.Close(true);
        }
        private void OnInactivityWarn(int warnTime)
        {
            InformationDialog.Inform("Inactivity Warning!", $"You will be kicked due to inactivity in {warnTime} seconds.", "Cancel");
        }
        #endregion
    }
}