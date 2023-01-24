namespace DanielLochner.Assets
{
    public class MenuSingleton<T> : Menu, ISingleton<T> where T : Menu
    {
        public static T Instance { get; private set; }

        public void SetSingleton(T instance)
        {
            if (Instance != null) Destroy(this);
            Instance = instance;
        }

        protected override void Awake()
        {
            base.Awake();
            SetSingleton(this as T);
        }
        protected virtual void OnDestroy()
        {
            SetSingleton(null);
        }
    }
}