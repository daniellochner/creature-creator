using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    [Serializable]
    public class Keybind
    {
        #region Fields
        [SerializeField] private KeyCode key;
        [SerializeField] private KeyCode modifierKey;
        #endregion

        #region Properties
        public KeyCode Key => key;
        public KeyCode ModifierKey => modifierKey;

        public static Keybind None = new Keybind(KeyCode.None, KeyCode.None);
        #endregion

        #region Methods
        public Keybind(KeyCode k, KeyCode mk = KeyCode.None)
        {
            key = k;
            modifierKey = mk;
        }

        public override string ToString()
        {
            return (modifierKey != KeyCode.None) ? $"{modifierKey} + {key}" : key.ToString();
        }
        public bool Equals(Keybind other)
        {
            return other.key == key && other.modifierKey == modifierKey;
        }
        #endregion
    }
}