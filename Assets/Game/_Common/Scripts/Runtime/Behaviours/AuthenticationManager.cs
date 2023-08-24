using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AuthenticationManager : MonoBehaviourSingleton<AuthenticationManager>
    {
        #region Properties
        public AuthStatus Status { get; set; } = AuthStatus.Busy;
        #endregion

        #region Methods
        public void Authenticate()
        {
            Status = AuthStatus.Busy;

#if UNITY_STANDALONE
            if (EducationManager.Instance.IsEducational)
            {
                EducationManager.Instance.StartCoroutine(EducationManager.Instance.VerifyRoutine(delegate (bool isVerified)
                {
                    Status = isVerified ? AuthStatus.Success : AuthStatus.Fail;
                }));
            }
            else
            {
                if (SteamManager.Initialized)
                {
                    Steamworks.SteamUserStats.StoreStats();
                    Status = AuthStatus.Success;
                }
                else
                {
                    Status = AuthStatus.Fail;
                }
            }
#elif UNITY_IOS || UNITY_ANDROID
            GameServices.Instance.LogIn(delegate (bool isLoggedIn)
            {
                Status = AuthStatus.Success; // isLoggedIn ? AuthStatus.Success : AuthStatus.Fail;
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