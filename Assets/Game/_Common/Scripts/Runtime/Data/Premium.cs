using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class Premium : Data
    {
        #region Fields
        [SerializeField] private bool isPremium = false;
        [SerializeField] private UsableItems usableBodyParts = new UsableItems();
        [SerializeField] private UsableItems usablePatterns = new UsableItems();
        #endregion

        #region Properties
        public bool IsPremium
        {
            get => isPremium;
            set => isPremium = value;
        }
        public UsableItems UsableBodyParts => usableBodyParts;
        public UsableItems UsablePatterns => usablePatterns;
        #endregion

        #region Methods
        public override void Revert()
        {
            isPremium = SystemUtility.IsDevice(DeviceType.Desktop);
            usableBodyParts.Clear();
            usablePatterns.Clear();
        }
        #endregion

        #region Nested
        [Serializable]
        public class UsableItems : SerializableDictionaryBase<string, bool> { }
        #endregion
    }
}