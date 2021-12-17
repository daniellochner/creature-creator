// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FootEditor : ExtremityEditor
    {
        #region Properties
        public FootConstructor FootConstructor => ExtremityConstructor as FootConstructor;
        #endregion

        #region Methods
        public override void Setup(CreatureEditor creatureEditor)
        {
            base.Setup(creatureEditor);

            BodyPartConstructor.OnScale += delegate (Vector3 scale)
            {
                if (ExtremityConstructor.ConnectedLimb != null)
                {
                    float scaledBaseOffset = FootConstructor.BaseOffset * scale.y;

                    LegEditor leg = ExtremityConstructor.ConnectedLimb.GetComponent<LegEditor>();
                    leg.SetFootOffset(scaledBaseOffset);
                    leg.FlippedLeg.SetFootOffset(scaledBaseOffset);
                }
            };
        }

        public override bool CanConnectToLimb(LimbConstructor limb)
        {
            return base.CanConnectToLimb(limb) && limb is LegConstructor;
        }
        #endregion
    }
}