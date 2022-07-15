// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Player : MonoBehaviourSingleton<Player>
    {
        #region Fields
        [SerializeField] private CreaturePlayerLocal creature;
        #endregion

        #region Properties
        public CreaturePlayerLocal Creature => creature;
        #endregion
    }
}