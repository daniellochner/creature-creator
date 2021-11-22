using Unity.Netcode;

namespace DanielLochner.Assets
{
    public class NetworkSingleton<T> : NetworkBehaviour, ISingleton<T> where T : NetworkBehaviour
    {
        public static T Instance { get; private set; }

        public void SetSingleton(T instance)
        {
            if (Instance != null) Destroy(this);
            Instance = instance;
        }

        protected virtual void Awake()
        {
            SetSingleton(this as T);
        }
    }
}