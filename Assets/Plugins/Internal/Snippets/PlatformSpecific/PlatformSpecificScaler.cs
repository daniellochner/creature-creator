using UnityEngine;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(-1)]
    public class PlatformSpecificScaler : MonoBehaviour
    {
        #region Fields
        [SerializeField] private DeviceType targetPlatform;
        [SerializeField] private float scale;
        #endregion

        #region Methods
        private void Awake()
        {
            if (SystemInfo.deviceType == targetPlatform)
            {
                transform.localScale *= scale;
            }
        }
        #endregion
    }
}