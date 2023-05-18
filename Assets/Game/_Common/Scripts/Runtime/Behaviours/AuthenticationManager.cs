using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [DefaultExecutionOrder(1)]
    public class AuthenticationManager : MonoBehaviourSingleton<AuthenticationManager>
    {
        #region Properties
        public AuthStatus Status { get; set; } = AuthStatus.Busy;
        #endregion

        #region Methods
        private void Start()
        {
            Authenticate();
        }

        public void Authenticate()
        {
            Status = AuthStatus.Busy;
            
#if UNITY_EDITOR
            Status = AuthStatus.Success;
#elif UNITY_STANDALONE
            if (SteamManager.Initialized)
            {
                Steamworks.SteamUserStats.StoreStats();
                Status = AuthStatus.Success;
            }
            else
            {
                Status = AuthStatus.Fail;
            }
#elif UNITY_IOS || UNITY_ANDROID
            GameServices.Instance.LogIn(delegate (bool success)
            {
                Status = AuthStatus.Success;//success ? AuthStatus.Success : AuthStatus.Fail;
            });
#endif
        }
        #endregion

        #region Nested
        public enum AuthStatus
        {
            Fail,
            Success,
            Busy
        }
        #endregion
    }
}