// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class WorldManager : MonoBehaviourSingleton<WorldManager>
    {
        #region Properties
        public World World
        {
            get;
            set;
        }

        public bool IsMultiplayer => World is WorldMP;
        public bool IsUsingTeleport { get; set; }
        #endregion

        #region Methods
        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;

            NetworkShutdownManager.Instance.OnShutdown += OnShutdown;
        }

        private void OnLoadCompleted(string sceneName, LoadSceneMode loadSceneMode, System.Collections.Generic.List<ulong> clientsCompleted, System.Collections.Generic.List<ulong> clientsTimedOut)
        {
            if (GameSetup.Instance && !GameSetup.Instance.IsSetup)
            {
                GameSetup.Instance.Setup();
            }
        }
        private void OnServerStarted()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadCompleted;
            NetworkManager.Singleton.SceneManager.LoadScene(World.MapName, LoadSceneMode.Single);
        }
        private void OnShutdown()
        {
            IsUsingTeleport = false;
        }
        private void OnClientConnect(ulong clientID)
        {
            if (NetworkManager.Singleton.IsHost && NetworkUtils.IsPlayer(clientID))
            {
                SetupSceneManager();
            }
        }
        private void OnLoad(ulong clientId, string nextScene, LoadSceneMode loadSceneMode, AsyncOperation operation)
        {
            string prevScene = SceneManager.GetActiveScene().name;

            if (IsUsingTeleport)
            {
                TeleportManager.Instance.OnLeave(prevScene, nextScene);
            }

            LoadingManager.Instance.StartCoroutine(LoadingManager.Instance.LoadRoutine(operation, delegate
            {
                if (IsUsingTeleport)
                {
                    TeleportManager.Instance.OnEnter(prevScene, nextScene);
                    IsUsingTeleport = false;
                }
            }));
        }

        public void SetupSceneManager()
        {
            NetworkManager.Singleton.SceneManager.OnLoad += OnLoad;
        }
        #endregion
    }
}