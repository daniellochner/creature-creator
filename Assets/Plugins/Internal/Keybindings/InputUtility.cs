using UnityEngine;

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
    }
}