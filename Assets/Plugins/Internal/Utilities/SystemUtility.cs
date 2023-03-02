using UnityEngine;

namespace DanielLochner.Assets
{
    public static class SystemUtility
    {
        public static bool IsDevice(DeviceType type)
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isRemoteConnected)
            {
                return type == DeviceType.Handheld;
            }
#endif
            return type == SystemInfo.deviceType;
        }
    }
}