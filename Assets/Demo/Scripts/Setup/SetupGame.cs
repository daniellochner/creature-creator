// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SetupGame : MonoBehaviourSingleton<SetupGame>
    {
        #region Methods
        private void Start()
        {
            Setup();
        }
        private void Setup()
        {
            NetworkCreature networkCreature = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<NetworkCreature>();
            EditorManager.Instance.Player = networkCreature.Player;
            networkCreature.Player.gameObject.SetActive(true);
            EditorManager.Instance.UnlockedBodyParts = ProgressManager.Data.UnlockedBodyParts;
            EditorManager.Instance.UnlockedPatterns = ProgressManager.Data.UnlockedPatterns;
            EditorManager.Instance.Setup();

            CreatureInformationManager.Instance.Setup();
            NetworkCreaturesMenu.Instance.Setup();
            NetworkCreaturesManager.Instance.Setup();

            //if (isPrivate)
            //{
            //    InformationDialog.Inform("Lobby Code", $"The code to your private lobby is \"{lobby.LobbyCode}\".\nPress {KeybindingsManager.Data.ViewPlayers} to view it again.");
            //}
        }
        #endregion
    }
}