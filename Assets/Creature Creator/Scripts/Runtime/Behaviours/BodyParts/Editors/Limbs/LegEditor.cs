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

        private MeshCollider shadowCollider;
        #endregion

        #region Properties
        public LegEditor FlippedLeg => FlippedLimb as LegEditor;
        public FootEditor ConnectedFoot => ConnectedExtremity as FootEditor;

        public bool UseShadow
        {
            get => shadowCollider.gameObject.activeSelf;
            set => shadowCollider.gameObject.SetActive(value);
        }
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

            GameObject shadow = new GameObject("Shadow");
            shadow.transform.SetParent(LimbConstructor.Model, false);
            shadow.layer = LayerMask.NameToLayer("Body Parts");
            shadow.tag = "Body Part";
            shadowCollider = shadow.AddComponent<MeshCollider>();
            UseShadow = false;
        }

        public override void Setup(CreatureEditor creatureEditor)
        {
            base.Setup(creatureEditor);

            SetupInteraction();
            SetupConstructor();
        }
        private void SetupInteraction()
        {
            LDrag.OnPress.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    dragRadius = footBoneDrag.cylinderRadius;
                    footBoneDrag.cylinderRadius = Mathf.Infinity;
                }
            });
            LDrag.OnRelease.AddListener(delegate
            {
                if (EditorManager.Instance.IsBuilding)
                {
                    footBoneDrag.cylinderRadius = dragRadius;
                }
            });
        }
        private void SetupConstructor()
        {
            LimbConstructor.OnConnectExtremity += delegate (ExtremityConstructor extremity)
            {
                FootConstructor constructor = extremity as FootConstructor;
                FootEditor editor = constructor.GetComponent<FootEditor>();

                if (editor.LDrag.IsPressing)
                {
                    shadowCollider.sharedMesh = Instantiate(colliderMesh);
                    UseShadow = FlippedLeg.UseShadow = true;
                }

                SetFootOffset(constructor.Offset);
                FlippedLeg.SetFootOffset(constructor.Offset);
            };
            LimbConstructor.OnDisconnectExtremity += delegate
            {
                SetFootOffset(0f);
                FlippedLeg.SetFootOffset(0f);

                UseShadow = FlippedLeg.UseShadow = false;
            };
        }

        public void SetFootOffset(float offset)
        {
            footBoneDrag.boundsOffset = Vector3.up * offset;
            this.InvokeAtEndOfFrame(UpdateMeshCollider);
        }

        private void HandleFloor()
        {
            footBoneDrag.transform.position = footBoneDrag.ClampToBounds(footBoneDrag.transform.position);
        }
        #endregion
    }
}