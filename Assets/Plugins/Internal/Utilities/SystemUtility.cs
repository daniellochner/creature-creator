using UnityEngine;

namespace DanielLochner.Assets
{
    public static class SystemUtility
    {
        private static readonly int LOW_END_MEMORY_THRESHOLD = 512; // MB

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

        public static bool IsLowEndDevice
        {
            get
            {
                return SystemInfo.systemMemorySize < LOW_END_MEMORY_THRESHOLD;
            }
        }
    }
}