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
        [SerializeField] public bool premium = true;
        [SerializeField] public bool released = true;
        #endregion

        #region Properties
        public string Author => author;
        public virtual Sprite Icon => icon;
        public bool Premium => premium;
        public bool Released => released;
        #endregion
    }
}