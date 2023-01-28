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
        #endregion

        #region Methods
        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
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
        private void OnClientConnect(ulong clientID)
        {
            NetworkManager.Singleton.SceneManager.OnLoad += OnLoad;
            TeleportManager.IsUsingTeleport = false;
        }

        private void OnLoad(ulong clientId, string nextScene, LoadSceneMode loadSceneMode, AsyncOperation operation)
        {
            string prevScene = SceneManager.GetActiveScene().name;

            CreatureData creature = null;
            if (Player.Instance != null)
            {
                creature = JsonUtility.FromJson<CreatureData>(JsonUtility.ToJson(Player.Instance.Constructor.Data));
            }

            if (TeleportManager.IsUsingTeleport)
            {
                TeleportManager.Instance.OnLeave(prevScene, nextScene, creature);
            }

            LoadingManager.Instance.StartCoroutine(LoadingManager.Instance.LoadRoutine(operation, delegate
            {
                if (TeleportManager.IsUsingTeleport)
                {
                    TeleportManager.Instance.OnEnter(prevScene, nextScene, creature);
                }
            }));
        }
        #endregion
    }
}