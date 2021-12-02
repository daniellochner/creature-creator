// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public abstract class Limb : BodyPart
    {
        #region Fields
        [Header("Limb")]
        [SerializeField] private bool canAttachExtremity;
        #endregion

        #region Properties
        public bool CanAttachExtremity => canAttachExtremity;
        public override int BaseComplexity => 7;
        #endregion
    }
}