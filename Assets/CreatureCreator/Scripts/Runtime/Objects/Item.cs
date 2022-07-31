// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public abstract class Item : ScriptableObject
    {
        #region Fields
        [Header("Item")]
        [SerializeField] private string author;
        [SerializeField] private Sprite icon;
        #endregion

        #region Properties
        public string Author => author;
        public Sprite Icon => icon;
        #endregion
    }
}