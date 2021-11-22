// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LegEditor : LimbEditor
    {
        #region Fields
        private Drag footBoneDrag;
        private float dragRadius;
        #endregion

        #region Methods
        private void LateUpdate()
        {
            HandleFloor();
        }

        public override void Setup(CreatureEditor creatureEditor)
        {
            base.Setup(creatureEditor);

            footBoneDrag = LimbConstructor.Bones[LimbConstructor.Bones.Length - 1].GetComponent<Drag>();

            Drag.OnPress.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    dragRadius = footBoneDrag.cylinderRadius;
                    footBoneDrag.cylinderRadius = Mathf.Infinity;
                }
            });
            Drag.OnRelease.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    footBoneDrag.cylinderRadius = dragRadius;
                }
            });
        }
        
        private void HandleFloor()
        {
            footBoneDrag.transform.position = footBoneDrag.ClampToBounds(footBoneDrag.transform.position);
        }
        #endregion
    }
}