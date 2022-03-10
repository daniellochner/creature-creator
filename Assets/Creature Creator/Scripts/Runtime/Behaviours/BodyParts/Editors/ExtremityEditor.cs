// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ExtremityEditor : BodyPartEditor
    {
        #region Properties
        public ExtremityConstructor ExtremityConstructor => BodyPartConstructor as ExtremityConstructor;
        public ExtremityEditor FlippedExtremity => Flipped as ExtremityEditor;

        public LimbEditor ConnectedLimb { get; set; }
        #endregion

        #region Methods
        private void LateUpdate()
        {
            HandleConnection();
        }

        public override void Setup(CreatureEditor creatureEditor)
        {
            base.Setup(creatureEditor);

            ExtremityConstructor.OnConnectToLimb += delegate (LimbConstructor constructor)
            {
                ConnectedLimb = constructor.GetComponent<LimbEditor>();
                ConnectedLimb.ConnectedExtremity = this;
            };
            ExtremityConstructor.OnDisconnectFromLimb += delegate
            {
                ConnectedLimb.ConnectedExtremity = null;
                ConnectedLimb = null;
            };
        }

        public override bool CanAttach(out Vector3 aPosition, out Quaternion aRotation)
        {
            if (Physics.Raycast(RectTransformUtility.ScreenPointToRay(CreatureEditor.CameraOrbit.Camera, Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Body Parts")))
            {
                LimbConstructor limb = hitInfo.collider.GetComponentInParent<LimbConstructor>();
                if (CanConnectToLimb(limb))
                {
                    ExtremityConstructor current = limb.ConnectedExtremity;
                    if (current != ExtremityConstructor)
                    {
                        if (current != null && current != ExtremityConstructor.Flipped)
                        {
                            current.DisconnectFromLimb();
                            current.Detach();
                        }
                        ExtremityConstructor.ConnectToLimb(limb);
                    }
                    aPosition = limb.Extremity.position;
                    aRotation = limb.Extremity.rotation;
                    return true;
                }
            }
            if (ExtremityConstructor.ConnectedLimb != null)
            {
                ExtremityConstructor.DisconnectFromLimb();
            }
            aPosition = Drag.TargetPosition;
            aRotation = transform.rotation;
            return false;
        }
        public virtual bool CanConnectToLimb(LimbConstructor limb)
        {
            return limb != null && limb.Limb.CanAttachExtremity;
        }

        private void HandleConnection()
        {
            if (ExtremityConstructor.ConnectedLimb != null)
            {
                transform.position = ExtremityConstructor.ConnectedLimb.Extremity.position;
            }
        }
        #endregion
    }
}