using UnityEngine;

namespace DanielLochner.Assets
{
    public static class SystemUtility
    {
        public static DeviceType DeviceType
        {
            get
            {
#if UNITY_EDITOR
                if (UnityEditor.EditorApplication.isRemoteConnected)
                {
                    return DeviceType.Handheld;
                }
#endif
                return SystemInfo.deviceType;
            }
        }
        public static bool IsDevice(DeviceType type)
        {
            return type == DeviceType;
        }
    }
}