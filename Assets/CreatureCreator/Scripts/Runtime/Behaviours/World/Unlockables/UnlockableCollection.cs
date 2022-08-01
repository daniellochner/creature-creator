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
        [SerializeField] private int cash;
        [SerializeField] private List<Item> items;
        #endregion

        #region Properties
        public override bool IsUnlocked
        {
            get
            {
                return PlayerPrefs.GetInt(collectionId) == 1;
                //List<Item> tmp = new List<Item>(items);
                //foreach (Item item in items)
                //{
                //    if ((item.itemType == UnlockableItemType.BodyPart && ProgressManager.Data.UnlockedBodyParts.Contains(item.itemID)) || (item.itemType == UnlockableItemType.Pattern  && ProgressManager.Data.UnlockedPatterns.Contains(item.itemID)))
                //    {
                //        tmp.RemoveAll(x => x.itemID == item.itemID);
                //        cash = 0;
                //    }
                //}
                //items = tmp;
                //return items.Count == 0;
            }
        }
        #endregion

        #region Methods
        protected override void OnUnlock()
        {
            if (cash > 0)
            {
                NotificationsManager.Notify($"You received ${cash}!");
                ProgressManager.Data.Cash += cash;
            }

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

            PlayerPrefs.SetInt(collectionId, 1);
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