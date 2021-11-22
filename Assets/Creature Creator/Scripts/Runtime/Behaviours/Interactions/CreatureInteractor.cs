// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreaturePlayer))]
    public class CreatureInteractor : Interactor
    {
        #region Properties
        public CreaturePlayer Creature { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Creature = GetComponent<CreaturePlayer>();
        }
        #endregion
    }
}