// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class PivotTool : Tool
    {
        #region Fields
        private Vector3 upDirection;
        #endregion

        #region Properties
        public override bool CanShow => BodyPartEditor.BodyPartConstructor.CanMirror;

        protected override Change Type => Change.PivotBodyPart;
        #endregion

        #region Methods
        protected override void Setup()
        {
            base.Setup();

            transform.localPosition = Vector3.forward * BodyPartEditor.BodyPartConstructor.BodyPart.PivotOffset;

            drag.OnPress.AddListener(delegate
            {
                upDirection = BodyPartEditor.transform.up;
            });
            drag.OnDrag.AddListener(delegate
            {
                BodyPartEditor.transform.LookAt(transform, upDirection);
                BodyPartEditor.BodyPartConstructor.SetPositionAndRotation(BodyPartEditor.transform.position, BodyPartEditor.transform.rotation);
            });
            drag.OnRelease.AddListener(delegate
            {
                transform.position = BodyPartEditor.transform.position + BodyPartEditor.transform.forward * BodyPartEditor.transform.localScale.z * BodyPartEditor.BodyPartConstructor.BodyPart.PivotOffset;
            });
        }
        #endregion
    }
}