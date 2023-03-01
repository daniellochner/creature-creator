using UnityEngine;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(-1)]
    public class PlatformSpecificActivator : MonoBehaviour
    {
        #region Fields
        [SerializeField] private DeviceType targetPlatform;
        #endregion

        #region Methods
        private void Awake()
        {
            gameObject.SetActive(SystemInfo.deviceType == targetPlatform);
        }
        #endregion
    }
}