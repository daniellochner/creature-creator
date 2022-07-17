// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SetupGame : MonoBehaviourSingleton<SetupGame>
    {
        #region Fields
        [SerializeField] private NetworkObject playerPrefab;
        [SerializeField] private NetworkObject[] helpers;
        #endregion

        #region Properties
        public static bool IsMultiplayer { get; set; }
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }
        private void OnDestroy()
        {
            Shutdown();
        }

        public void Setup()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                foreach (NetworkObject helper in helpers)
                {
                    Instantiate(helper).Spawn();
                }
                Instantiate(playerPrefab).SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
            }
            
            if (IsMultiplayer)
            {
                NetworkShutdownManager.Instance.OnUncontrolledShutdown += OnUncontrolledShutdown;
                NetworkShutdownManager.Instance.OnUncontrolledClientShutdown += OnUncontrolledClientShutdown;
                NetworkShutdownManager.Instance.OnUncontrolledHostShutdown += OnUncontrolledHostShutdown;

                NetworkInactivityManager.Instance.OnInactivityKick += OnInactivityKick;
                NetworkInactivityManager.Instance.OnInactivityWarn += OnInactivityWarn;

                NetworkCreaturesManager.Instance.Setup();

                World world = new World(LobbyHelper.Instance.JoinedLobby);
                if (NetworkManager.Singleton.IsHost)
                {
                    if (world.IsPrivate)
                    {
                        InformationDialog.Inform("Private World", $"The code to your private world is:\n<u><b>{world.Lobby.Id}</b></u>\n\nPress the button below to copy it to your clipboard. Press {KeybindingsManager.Data.ViewPlayers} to view it again.", "Copy", true, delegate
                        {
                            GUIUtility.systemCopyBuffer = world.Lobby.Id;
                        });
                    }
                    if (world.SpawnNPC)
                    {
                        foreach (NPCSpawner npc in FindObjectsOfType<NPCSpawner>())
                        {
                            npc.Spawn();
                        }
                    }
                }
            }

            EditorManager.Instance.UnlockedBodyParts = ProgressManager.Data.UnlockedBodyParts;
            EditorManager.Instance.UnlockedPatterns = ProgressManager.Data.UnlockedPatterns;
            EditorManager.Instance.BaseCash = ProgressManager.Data.Cash;
            EditorManager.Instance.HiddenBodyParts = SettingsManager.Data.HiddenBodyParts;
            EditorManager.Instance.HiddenPatterns = SettingsManager.Data.HiddenPatterns;

            EditorManager.Instance.Setup();
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
        #endregion
    }
}