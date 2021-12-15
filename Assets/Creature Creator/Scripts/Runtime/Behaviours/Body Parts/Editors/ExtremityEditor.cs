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
        #endregion

        #region Methods
        private void LateUpdate()
        {
            HandleConnection();
        }

        public override bool CanAttach(out Vector3 aPosition, out Quaternion aRotation)
        {
            if (Physics.Raycast(RectTransformUtility.ScreenPointToRay(CreatureEditor.CameraOrbit.Camera, Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Body Parts")))
            {
                LimbConstructor limb = hitInfo.collider.GetComponentInParent<LimbConstructor>();
                if (CanConnectToLimb(limb))
                {
                    ExtremityConstructor.ConnectToLimb(limb);
                    aPosition = limb.Extremity.position;
                    aRotation = limb.Extremity.rotation;
                    return true;
                }
            }
            ExtremityConstructor.DisconnectFromLimb();
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