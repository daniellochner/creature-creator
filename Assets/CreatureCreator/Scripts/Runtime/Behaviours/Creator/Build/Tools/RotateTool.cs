// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class RotateTool : Tool
    {
        #region Fields
        [SerializeField] private float fixedScaleZ = 2f;

        private Quaternion initialRotation;
        private Vector3 initialDirection;
        #endregion

        #region Properties
        public override bool CanShow => BodyPartEditor.BodyPartConstructor.CanMirror;
        public override float ScaleFactor => base.ScaleFactor * BodyPartEditor.BodyPartConstructor.BodyPart.RotateScaleFactor;

        protected override Change Type => Change.RotateBodyPart;
        #endregion

        #region Methods
        protected override void Setup()
        {
            base.Setup();

            model.localScale = new Vector3(model.localScale.x, model.localScale.y, fixedScaleZ);

            drag.OnBeginDrag.AddListener(delegate
            {
                initialDirection = (drag.TargetPosition - BodyPartEditor.transform.position).normalized;
                initialRotation = transform.rotation;
            });
            drag.OnDrag.AddListener(delegate
            {
                Vector3 direction = (drag.TargetPosition - BodyPartEditor.transform.position).normalized;

                float angle = Vector3.SignedAngle(direction, initialDirection, BodyPartEditor.transform.forward);
                Quaternion rotation = BodyPartEditor.transform.rotation = initialRotation * Quaternion.AngleAxis(-angle, Vector3.forward);

                BodyPartEditor.BodyPartConstructor.SetPositionAndRotation(BodyPartEditor.transform.position, rotation);
            });
        }
        #endregion
    }
}