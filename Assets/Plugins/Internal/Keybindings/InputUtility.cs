using UnityEngine;
using UnityEngine.EventSystems;

namespace DanielLochner.Assets
{
    public class InputUtility
    {
        public static bool GetKey(Keybind keybind)
        {
            return GetModifierKey(keybind) && Input.GetKey(keybind.Key);
        }
        public static bool GetKeyUp(Keybind keybind)
        {
            return GetModifierKey(keybind) && Input.GetKeyUp(keybind.Key);
        }
        public static bool GetKeyDown(Keybind keybind)
        {
            return GetModifierKey(keybind) && Input.GetKeyDown(keybind.Key);
        }

        public static bool GetModifierKey(Keybind keybind)
        {
            bool m = true;
            if (keybind.ModifierKey != KeyCode.None)
            {
                m = Input.GetKey(keybind.ModifierKey);
            }
            return m;
        }

        public static bool GetDelta(out float deltaX, out float deltaY)
        {
            deltaX = deltaY = 0;

            if (SystemUtility.IsDevice(DeviceType.Desktop))
            {
                deltaX = Input.GetAxis("Mouse X");
                deltaY = Input.GetAxis("Mouse Y");
            }
            else
            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                foreach (Touch touch in Input.touches)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        deltaX = touch.deltaPosition.x * 0.1f;
                        deltaY = touch.deltaPosition.y * 0.1f;
                        break;
                    }
                }
            }

            return deltaX != 0 || deltaY != 0;
        }
    }
}