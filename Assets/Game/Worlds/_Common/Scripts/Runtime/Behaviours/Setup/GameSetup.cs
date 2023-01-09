// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    [DefaultExecutionOrder(-1)]
    public class GameSetup : MonoBehaviourSingleton<GameSetup>
    {
        #region Fields
        [SerializeField] private NetworkObject playerPrefab;
        [SerializeField] private Platform startingPlatform;

        [Header("Multiplayer")]
        [SerializeField] private NetworkObject[] helpers;
        #endregion

        #region Properties
        public bool IsMultiplayer => WorldManager.Instance.World is WorldMP;
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
                NetworkObject obj = Instantiate(playerPrefab, startingPlatform.Position, startingPlatform.Rotation);
                obj.SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
            }

            if (IsMultiplayer)
            {
                SetupMP();
            }
            else
            {
                SetupSP();
            }

            Player.Instance.Editor.Platform = startingPlatform;
            EditorManager.Instance.Setup();
            MapManager.Instance.Setup();

            if ((ProgressManager.Data.UnlockedBodyParts.Count == 0) && (ProgressManager.Data.UnlockedPatterns.Count == 0) && !EditorManager.Instance.CreativeMode && SettingsManager.Data.Tutorial)
            {
                EditorManager.Instance.SetMode(EditorManager.EditorMode.Play, true);
                TutorialManager.Instance.Begin();
            }
            else
            {
                EditorManager.Instance.SetMode(EditorManager.EditorMode.Build, true);
            }
        }
        public void SetupMP()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                NetworkHostManager.Instance.SpawnPosition = startingPlatform.Position;
                NetworkHostManager.Instance.SpawnRotation = startingPlatform.Rotation;

                foreach (NetworkObject helper in helpers)
                {
                    Instantiate(helper).Spawn();
                }
            }
            
            NetworkShutdownManager.Instance.OnUncontrolledShutdown += OnUncontrolledShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledClientShutdown += OnUncontrolledClientShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledHostShutdown += OnUncontrolledHostShutdown;

            NetworkInactivityManager.Instance.OnInactivityKick += OnInactivityKick;
            NetworkInactivityManager.Instance.OnInactivityWarn += OnInactivityWarn;


            WorldMP world = WorldManager.Instance.World as WorldMP;

            if (NetworkManager.Singleton.IsHost)
            {
                if (world.IsPrivate)
                {
                    InformationDialog.Inform("Private World", $"The code to your private world is:\n<u><b>{world.Id}</b></u>\n\nPress the button below to copy it to your clipboard. Press {KeybindingsManager.Data.ViewPlayers} to view it again.", "Copy", true, delegate
                    {
                        GUIUtility.systemCopyBuffer = world.Id;
                    });
                }
                if (world.SpawnNPC)
                {
                    foreach (NPCSpawner npc in NPCSpawner.Spawners)
                    {
                        if (npc.spawnOnStart)
                        {
                            npc.Spawn();
                            npc.SpawnedNPC.GetComponent<AnimalAI>().PVE = world.EnablePVE;
                        }
                    }
                }
            }

            NetworkPlayersManager.Instance.Setup(world.Id);

            EditorManager.Instance.CreativeMode = world.CreativeMode;
            EditorManager.Instance.CheckForProfanity = !world.AllowProfanity;
        }
        public void SetupSP()
        {
            WorldSP world = WorldManager.Instance.World as WorldSP;
            
            if (world.SpawnNPC)
            {
                foreach (NPCSpawner npc in NPCSpawner.Spawners)
                {
                    if (npc.spawnOnStart)
                    {
                        npc.Spawn();
                        npc.SpawnedNPC.GetComponent<AnimalAI>().PVE = world.EnablePVE;
                    }
                }
            }

            EditorManager.Instance.CreativeMode = world.CreativeMode;
        }

        public void Shutdown()
        {
            if (IsMultiplayer)
            {
                ShutdownMP();
            }
            else
            {
                ShutdownSP();
            }

            WorldManager.Instance.World = null;
            MusicManager.Instance.FadeTo(null, 0f, 1f);
        }
        public void ShutdownMP()
        {
            NetworkShutdownManager.Instance.OnUncontrolledShutdown -= OnUncontrolledShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledClientShutdown -= OnUncontrolledClientShutdown;
            NetworkShutdownManager.Instance.OnUncontrolledHostShutdown -= OnUncontrolledHostShutdown;

            NetworkInactivityManager.Instance.OnInactivityKick -= OnInactivityKick;
            NetworkInactivityManager.Instance.OnInactivityWarn -= OnInactivityWarn;
        }
        public void ShutdownSP()
        {

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