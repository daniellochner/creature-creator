// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class RotateTool : Tool
    {
        #region Properties
        public override bool CanShow => BodyPartEditor.BodyPartConstructor.CanMirror;
        public override float ScaleFactor => base.ScaleFactor * BodyPartEditor.BodyPartConstructor.BodyPart.RotateScaleFactor;
        #endregion

        #region Methods
        protected override void Setup()
        {
            base.Setup();

            Quaternion initialRotation = Quaternion.identity;
            Vector3 initialDirection = Vector3.zero;

            drag.OnBeginDrag.AddListener(delegate
            {
                initialDirection = (drag.TargetPosition - BodyPartEditor.transform.position).normalized;
                initialRotation = transform.rotation;
            });
            drag.OnDrag.AddListener(delegate
            {
                Vector3 direction = (drag.TargetPosition - BodyPartEditor.transform.position).normalized;

                float angle = Vector3.SignedAngle(direction, initialDirection, BodyPartEditor.transform.forward);
                transform.rotation = BodyPartEditor.transform.rotation = initialRotation * Quaternion.AngleAxis(-angle, Vector3.forward);
            });
        }
        #endregion
    }
}