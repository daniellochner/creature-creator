using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TeleportManager : MonoBehaviourSingleton<TeleportManager>
    {
        public static bool IsUsingTeleport { get; set; }

        public virtual void TeleportTo(string targetScene)
        {
            IsUsingTeleport = true;
            LoadScene(targetScene);
        }
        public virtual void OnLeave(string prevScene, string nextScene, CreatureData data)
        {
        }
        public virtual void OnEnter(string prevScene, string nextScene, CreatureData data)
        {
            IsUsingTeleport = false;
            EditorManager.Instance.Load(data);
        }

        protected void LoadScene(string targetScene)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
            }
        }
    }
}