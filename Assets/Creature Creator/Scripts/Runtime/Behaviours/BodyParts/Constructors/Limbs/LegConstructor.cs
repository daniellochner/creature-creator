// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LegConstructor : LimbConstructor
    {
        #region Properties
        public LegConstructor FlippedLeg => FlippedLimb as LegConstructor;

        public FootConstructor ConnectedFoot => ConnectedExtremity as FootConstructor;
        #endregion

        #region Methods
        private void LateUpdate()
        {
            Extremity.rotation = CreatureConstructor.Body.rotation;
        }

        public override void Setup(CreatureConstructor creatureConstructor)
        {
            base.Setup(creatureConstructor);
            
            OnConnectExtremity += delegate (ExtremityConstructor extremity)
            {
                FootConstructor foot = extremity as FootConstructor;
                SetFootOffset(foot.Offset);
                FlippedLeg.SetFootOffset(foot.Offset);
            };
            OnDisconnectExtremity += delegate
            {
                SetFootOffset(0f);
                FlippedLeg.SetFootOffset(0f);
            };
        }

        public void SetFootOffset(float offset)
        {
            Transform foot = Bones[Bones.Length - 1];
            Vector3 localPosition = CreatureConstructor.transform.InverseTransformPoint(foot.position);

            foot.position = CreatureConstructor.transform.TransformPoint(localPosition.x, offset, localPosition.z);
        }
        #endregion
    }
}