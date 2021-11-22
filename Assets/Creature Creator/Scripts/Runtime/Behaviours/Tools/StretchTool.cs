// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class StretchTool : Tool
    {
        #region Fields
        [SerializeField] private Vector3Int stretchDir;
        #endregion

        #region Properties
        public override bool CanShow => true;
        #endregion

        #region Methods
        protected override void Setup()
        {
            base.Setup();

            // Drag
            drag.boxSize = BodyPartEditor.BodyPartConstructor.BodyPart.StretchDistance;
            drag.boundsOffset = BodyPartEditor.BodyPartConstructor.BodyPart.StretchDistance / 2f * (Vector3)stretchDir;
            drag.OnDrag.AddListener(delegate
            {
                float distance = BodyPartEditor.transform.InverseTransformVector(transform.position - BodyPartEditor.transform.position).magnitude;
                float stretch = (2 * distance - BodyPartEditor.BodyPartConstructor.BodyPart.StretchDistance) / BodyPartEditor.BodyPartConstructor.BodyPart.StretchDistance;

                BodyPartEditor.BodyPartConstructor.SetStretch(stretch * Vector3.one, stretchDir);
            });

            // Reposition
            transform.localPosition = (Vector3)stretchDir * (BodyPartEditor.BodyPartConstructor.BodyPart.StretchDistance / 2f * (1 + Vector3.Dot(BodyPartEditor.BodyPartConstructor.AttachedBodyPart.stretch, stretchDir)));
        }
        #endregion
    }
}