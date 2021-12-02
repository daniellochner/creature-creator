// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DemoSetup : MonoBehaviourSingleton<DemoSetup>
    {
        #region Methods
        public void Setup()
        {
            NetworkInactivityManager.Instance.OnInactivityKick += DemoManager.Instance.OnInactivityKick;
            NetworkInactivityManager.Instance.OnInactivityWarn += DemoManager.Instance.OnInactivityWarn;

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
        #endregion
    }
}