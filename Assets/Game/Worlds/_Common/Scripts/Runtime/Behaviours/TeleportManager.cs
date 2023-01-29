using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TeleportManager : MonoBehaviourSingleton<TeleportManager>
    {
        #region Fields
        private static CreatureData dataBuffer;
        #endregion

        #region Methods
        public virtual void TeleportTo(string targetScene, CreatureData data)
        {
            WorldManager.Instance.IsUsingTeleport = true;

            dataBuffer = data;

            LoadScene(targetScene);
        }

        public virtual void OnLeave(string prevScene, string nextScene)
        {
        }
        public virtual void OnEnter(string prevScene, string nextScene)
        {
            WorldManager.Instance.IsUsingTeleport = false;
            StartCoroutine(LoadRoutine());
        }

        protected void LoadScene(string targetScene)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
            }
        }

        private IEnumerator LoadRoutine()
        {
            yield return new WaitUntil(() => Player.Instance != null && Player.Instance.IsSetup);
            EditorManager.Instance.Load(dataBuffer);
            dataBuffer = null;
        }
        #endregion
    }
}