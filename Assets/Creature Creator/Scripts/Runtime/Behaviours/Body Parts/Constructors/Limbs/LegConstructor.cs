// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LegConstructor : LimbConstructor
    {
        #region Properties
        public LegConstructor FlippedLeg => FlippedLimb as LegConstructor;
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
                float scaledBaseOffset = foot.BaseOffset * foot.transform.localScale.y;

                SetFootOffset(scaledBaseOffset);
                FlippedLeg.SetFootOffset(scaledBaseOffset);
            };
            OnDisconnectExtremity += delegate (ExtremityConstructor extremity)
            {
                SetFootOffset(0);
                FlippedLeg.SetFootOffset(0);
            };
        }

        public override void Add()
        {
            base.Add();
            CreatureConstructor.Legs.Add(this);
        }
        public override void Remove()
        {
            base.Remove();
            CreatureConstructor.Legs.Remove(this);
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