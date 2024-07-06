// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class UnlockableCollection : UnlockableItem
    {
        #region Fields
        [SerializeField] private string collectionId;
        [SerializeField] private List<Item> items;
        #endregion

        #region Properties
        public override bool IsUnlocked
        {
            get
            {
                foreach (var item in items)
                {
                    if ((item.itemType == UnlockableItemType.BodyPart) && !ProgressManager.Data.UnlockedBodyParts.Contains(item.itemID))
                    {
                        return false;
                    }
                    else
                    if ((item.itemType == UnlockableItemType.Pattern) && !ProgressManager.Data.UnlockedPatterns.Contains(item.itemID))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        #endregion

        #region Methods
        protected override void OnUnlock()
        {
            foreach (Item item in items)
            {
                if (item.itemType == UnlockableItemType.BodyPart)
                {
                    EditorManager.Instance.UnlockBodyPart(item.itemID);
                }
                else
                if (item.itemType == UnlockableItemType.Pattern)
                {
                    EditorManager.Instance.UnlockPattern(item.itemID);
                }
            }
        }
        protected override void OnSpawn()
        {
        }
        #endregion

        #region Enums
        public enum UnlockableItemType
        {
            BodyPart,
            Pattern
        }
        #endregion

        #region Structs
        [Serializable] public struct Item
        {
            public string itemID;
            public UnlockableItemType itemType;
        }
        #endregion
    }
}