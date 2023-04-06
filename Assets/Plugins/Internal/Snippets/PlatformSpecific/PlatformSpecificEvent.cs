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
            if (SystemUtility.IsDevice(DeviceType.Desktop))
            {
                onStandalone.Invoke();
            }
            else
            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                onHandheld.Invoke();
            }
            else
            if (SystemUtility.IsDevice(DeviceType.Console))
            {
                onConsole.Invoke();
            }
        }
        #endregion
    }
}