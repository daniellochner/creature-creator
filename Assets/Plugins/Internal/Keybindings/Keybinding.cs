using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    [Serializable]
    public class Keybinding
    {
        [SerializeField] private KeyCode key;
        [SerializeField] private KeyCode modifierKey;

        public KeyCode Key => key;
        public KeyCode ModifierKey => modifierKey;

        public Keybinding(KeyCode k, KeyCode mk = KeyCode.None)
        {
            key = k;
            modifierKey = mk;
        }

        public override string ToString()
        {
            return (modifierKey != KeyCode.None) ? $"{modifierKey} + {key}" : key.ToString();
        }
        public bool Equals(Keybinding other)
        {
            return other.key == key && other.modifierKey == modifierKey;
        }
    }
}