using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(-1)]
    public class PlatformSpecificEvent : MonoBehaviour
    {
        #region Fields
        [SerializeField] private UnityEvent onStandalone;
        [SerializeField] private UnityEvent onHandheld;
        [SerializeField] private UnityEvent onConsole;
        #endregion

        #region Methods
        private void Awake()
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