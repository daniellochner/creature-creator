using System;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class FactoryData : Data
    {
        public List<ulong> SubscribedItems = new();
        public List<ulong> LikedItems = new();
        public List<ulong> DislikedItems = new();

        public SerializableDictionaryBase<FactoryItemQuery, CachedItemData> CachedItems = new();
        public SerializableDictionaryBase<ulong, string> CachedUsernames = new();

        public override void Revert()
        {
            SubscribedItems.Clear();
            LikedItems.Clear();
            DislikedItems.Clear();

            CachedItems.Clear();
            CachedUsernames.Clear();
        }

        [Serializable]
        public class CachedItemData
        {
            public long Ticks;
            public uint Total;
            public List<FactoryItem> Items = new();

            public CachedItemData()
            {
                Ticks = DateTime.UtcNow.Ticks;
            }
        }
    }

}