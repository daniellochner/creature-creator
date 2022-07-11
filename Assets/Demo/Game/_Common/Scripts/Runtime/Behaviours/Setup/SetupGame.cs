// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SetupGame : MonoBehaviourSingleton<SetupGame>
    {
        #region Fields
        [SerializeField] private NetworkCreaturePlayer player;
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            EditorManager.Instance.UnlockedBodyParts = ProgressManager.Data.UnlockedBodyParts;
            EditorManager.Instance.UnlockedPatterns = ProgressManager.Data.UnlockedPatterns;
            EditorManager.Instance.BaseCash = ProgressManager.Data.Cash;
            EditorManager.Instance.HiddenBodyParts = SettingsManager.Data.HiddenBodyParts;
            EditorManager.Instance.HiddenPatterns = SettingsManager.Data.HiddenPatterns;

            if (NetworkConnectionManager.IsConnected)
            {
                SetupMP();
            }
            else
            {
                SetupSP();
            }
        }
        private void SetupMP()
        {
            player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<NetworkCreaturePlayer>();
            EditorManager.Instance.Player = player.Player;
            EditorManager.Instance.Setup();

            NetworkCreaturesManager.Instance.Setup();
            NetworkCreaturesMenu.Instance.Setup();

            Lobby lobby = LobbyHelper.Instance.JoinedLobby;
            World world = new World(lobby);
            if (NetworkManager.Singleton.IsHost)
            {
                if (world.IsPrivate)
                {
                    InformationDialog.Inform("Private World", $"The code to your private world is:\n<u><b>{lobby.Id}</b></u>\n\nPress the button below to copy it to your clipboard. Press {KeybindingsManager.Data.ViewPlayers} to view it again.", "Copy", true, delegate
                    {
                        GUIUtility.systemCopyBuffer = lobby.Id;
                    });
                }
                if (!world.SpawnNPC)
                {
                    foreach (NetworkCreatureNonPlayer creature in FindObjectsOfType<NetworkCreatureNonPlayer>())
                    {
                        creature.Despawn();
                    }
                }
            }
        }
        private void SetupSP()
        {
            EditorManager.Instance.Setup();
        }
        #endregion
    }
}