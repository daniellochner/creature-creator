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
        [SerializeField] private int downloadsToday;
        #endregion

        #region Properties
        public bool IsPremium
        {
            get => isPremium;
            set => isPremium = value;
        }
        public UsableItems UsableBodyParts => usableBodyParts;
        public UsableItems UsablePatterns => usablePatterns;
        public int DownloadsToday
        {
            get => downloadsToday;
            set => downloadsToday = value;
        }
        #endregion

        #region Methods
        public override void Revert()
        {
            IsPremium = SystemUtility.IsDevice(DeviceType.Desktop);
            UsableBodyParts.Clear();
            UsablePatterns.Clear();
            DownloadsToday = 0;
        }
        #endregion

        #region Nested
        [Serializable]
        public class UsableItems : SerializableDictionaryBase<string, bool> { }
        #endregion
    }
}