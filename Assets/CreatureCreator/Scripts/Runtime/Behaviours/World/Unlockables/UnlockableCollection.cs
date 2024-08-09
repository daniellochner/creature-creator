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
                    if ((item.itemType == UnlockableItemType.BodyPart) && !WorldManager.Instance.IsBodyPartUnlocked(item.itemID))
                    {
                        return false;
                    }
                    else
                    if ((item.itemType == UnlockableItemType.Pattern) && !WorldManager.Instance.IsPatternUnlocked(item.itemID))
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
                    string bodyPartID = item.itemID;
                    switch (WorldManager.Instance.World.Mode)
                    {
                        case Mode.Adventure:
                            ProgressManager.Instance.UnlockBodyPart(bodyPartID);
                            break;

                        case Mode.Timed:
                            TimedManager.Instance.UnlockBodyPart(bodyPartID);
                            break;
                    }

                    EditorManager.Instance.UnlockBodyPart(bodyPartID);
                }
                else
                if (item.itemType == UnlockableItemType.Pattern)
                {
                    string patternID = item.itemID;
                    switch (WorldManager.Instance.World.Mode)
                    {
                        case Mode.Adventure:
                            ProgressManager.Instance.UnlockPattern(patternID);
                            break;

                        case Mode.Timed:
                            TimedManager.Instance.UnlockPattern(patternID);
                            break;
                    }

                    EditorManager.Instance.UnlockPattern(patternID);
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