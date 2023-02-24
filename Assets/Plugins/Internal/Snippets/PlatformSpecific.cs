using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(-1)]
    public class PlatformSpecific : MonoBehaviour
    {
        #region Fields
        [SerializeField] private DeviceType targetPlatform;
        [SerializeField] private UnityEvent onStandalone;
        [SerializeField] private UnityEvent onHandheld;
        [SerializeField] private UnityEvent onConsole;
        #endregion

        #region Methods
        private void Awake()
        {
            gameObject.SetActive(SystemInfo.deviceType == targetPlatform);
        }
        private void Start()
        {
            switch (SystemInfo.deviceType)
            {
                case DeviceType.Desktop:
                    onStandalone.Invoke();
                    break;
                case DeviceType.Handheld:
                    onHandheld.Invoke();
                    break;
                case DeviceType.Console:
                    onConsole.Invoke();
                    break;
            }
        }
        #endregion
    }
}