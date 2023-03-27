// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class HandConstructor : ExtremityConstructor
    {
        #region Fields
        [Header("Hand")]
        [SerializeField] private Transform palm;
        #endregion

        #region Properties
        public ArmConstructor ConnectedArm => ConnectedLimb as ArmConstructor;

        public HandConstructor FlippedHand => Flipped as HandConstructor;

        public Transform Palm => palm;
        #endregion
    }
}