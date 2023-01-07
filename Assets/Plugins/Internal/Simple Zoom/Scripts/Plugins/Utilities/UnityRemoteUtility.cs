namespace DanielLochner.Assets
{
    public static class UnityRemoteUtility
    {
        public static bool UsingUnityRemote
        {
            get
            {
#if UNITY_EDITOR
                return UnityEditor.EditorApplication.isRemoteConnected;
#else
                return false;
#endif
            }
        }
    }
}