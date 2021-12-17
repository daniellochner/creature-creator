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

        public float BaseOffset
        {
            get => baseOffset;
        }
        #endregion

        #region Methods
        public override void SetScale(Vector3 scale, MinMax minMaxScale)
        {
            base.SetScale(scale, minMaxScale);

            if (ConnectedLeg != null)
            {
                float scaledBaseOffset = BaseOffset * scale.y;

                ConnectedLeg.SetFootOffset(scaledBaseOffset);
                ConnectedLeg.FlippedLeg.SetFootOffset(scaledBaseOffset);
            }
        }
        #endregion
    }
}