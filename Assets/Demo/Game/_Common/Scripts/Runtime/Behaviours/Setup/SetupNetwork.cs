// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SetupNetwork : MonoBehaviour
    {
        private void OnDestroy()
        {
            Shutdown();
        }

        public void Setup()
        {
            NetworkShutdownManager.Instance.OnUncontrolledShutdown += OnUncontrolledShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledClientShutdown += OnUncontrolledClientShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledHostShutdown += OnUncontrolledHostShutdown;

            NetworkInactivityManager.Instance.OnInactivityKick += OnInactivityKick;
            NetworkInactivityManager.Instance.OnInactivityWarn += OnInactivityWarn;

            NetworkCreaturesManager.Instance.Setup();
            
            Lobby lobby = LobbyHelper.Instance.JoinedLobby;
            World world = new World(lobby);
            if (NetworkManager.Singleton.IsHost)
            {
                if (world.IsPrivate == true)
                {
                    InformationDialog.Inform("Private World", $"The code to your private world is:\n<u><b>{lobby.Id}</b></u>\n\nPress the button below to copy it to your clipboard. Press {KeybindingsManager.Data.ViewPlayers} to view it again.", "Copy", true, delegate
                    {
                        GUIUtility.systemCopyBuffer = lobby.Id;
                    });
                }
                if (world.SpawnNPC == false)
                {
                    foreach (NetworkCreatureNonPlayer creature in FindObjectsOfType<NetworkCreatureNonPlayer>())
                    {
                        creature.Despawn();
                    }
                }
            }
        }

        public void Shutdown()
        {
            NetworkShutdownManager.Instance.OnUncontrolledShutdown -= OnUncontrolledShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledClientShutdown -= OnUncontrolledClientShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledHostShutdown -= OnUncontrolledHostShutdown;

            NetworkInactivityManager.Instance.OnInactivityKick -= OnInactivityKick;
            NetworkInactivityManager.Instance.OnInactivityWarn -= OnInactivityWarn;
        }

        private void OnUncontrolledShutdown()
        {
            SceneManager.LoadScene("MainMenu");
        }
        private void OnUncontrolledClientShutdown()
        {
            InformationDialog.Inform("Disconnected!", "You lost connection.");
        }
        private void OnUncontrolledHostShutdown()
        {
            InformationDialog.Inform("Disconnected!", "The host lost connection.");
        }
        private void OnInactivityKick()
        {
            InformationDialog.Instance.Close(true);
        }
        private void OnInactivityWarn(int warnTime)
        {
            InformationDialog.Inform("Inactivity Warning!", $"You will be kicked due to inactivity in {warnTime} seconds.", "Cancel");
        }
    }
}