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

        #region Properties
        public LegEditor FlippedLeg => FlippedLimb as LegEditor;

        public FootEditor ConnectedFoot => ConnectedExtremity as FootEditor;
        #endregion

        #region Methods
        private void LateUpdate()
        {
            HandleFloor();
        }

        protected override void Initialize()
        {
            base.Initialize();

            footBoneDrag = LimbConstructor.Bones[LimbConstructor.Bones.Length - 1].GetComponent<Drag>();
        }

        public override void Setup(CreatureEditor creatureEditor)
        {
            base.Setup(creatureEditor);

            LimbConstructor.OnConnectExtremity += delegate (ExtremityConstructor extremity)
            {
                FootConstructor foot = extremity as FootConstructor;
                SetFootOffset(foot.Offset);
                FlippedLeg.SetFootOffset(foot.Offset);
            };
            LimbConstructor.OnDisconnectExtremity += delegate
            {
                SetFootOffset(0f, true);
                FlippedLeg.SetFootOffset(0f, true);
            };

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
        
        public void SetFootOffset(float offset, bool updateCollider = false)
        {
            footBoneDrag.boundsOffset = Vector3.up * offset;
            if (updateCollider)
            {
                UpdateMeshCollider();
            }
        }

        private void HandleFloor()
        {
            footBoneDrag.transform.position = footBoneDrag.ClampToBounds(footBoneDrag.transform.position);
        }
        #endregion
    }
}