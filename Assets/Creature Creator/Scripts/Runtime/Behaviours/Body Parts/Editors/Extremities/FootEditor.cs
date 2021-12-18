// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FootEditor : ExtremityEditor
    {
        #region Properties
        public FootConstructor FootConstructor => ExtremityConstructor as FootConstructor;

        public LegEditor ConnectedLeg => ConnectedLimb as LegEditor;
        #endregion

        #region Methods
        public override void Setup(CreatureEditor creatureEditor)
        {
            base.Setup(creatureEditor);

            ExtremityConstructor.OnScale += delegate
            {
                ConnectedLeg.SetFootOffset(FootConstructor.Offset, true);
                ConnectedLeg.FlippedLeg.SetFootOffset(FootConstructor.Offset, true);
            };
        }

        public override bool CanConnectToLimb(LimbConstructor limb)
        {
            return base.CanConnectToLimb(limb) && limb is LegConstructor;
        }
        #endregion
    }
}