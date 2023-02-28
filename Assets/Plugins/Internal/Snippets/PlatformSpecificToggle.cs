using UnityEngine;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(-1)]
    public class PlatformSpecificToggle : MonoBehaviour
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