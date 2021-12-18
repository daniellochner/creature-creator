// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FootConstructor : ExtremityConstructor
    {
        #region Fields
        [Header("Foot")]
        [SerializeField] private float baseOffset;
        #endregion

        #region Properties
        public LegConstructor ConnectedLeg => ConnectedLimb as LegConstructor;

        public float Offset
        {
            get => baseOffset * transform.localScale.y;
        }
        #endregion

        #region Methods
        public override void Setup(CreatureConstructor creatureConstructor)
        {
            base.Setup(creatureConstructor);

            OnScale += delegate
            {
                ConnectedLeg.SetFootOffset(Offset);
                ConnectedLeg.FlippedLeg.SetFootOffset(Offset);
            };
        }
        #endregion
    }
}