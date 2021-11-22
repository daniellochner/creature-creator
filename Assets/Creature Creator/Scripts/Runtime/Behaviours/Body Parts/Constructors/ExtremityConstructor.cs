// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ExtremityConstructor : BodyPartConstructor
    {
        #region Properties
        public AttachedExtremity AttachedExtremity => AttachedBodyPart as AttachedExtremity;
        public ExtremityConstructor FlippedExtremity => Flipped as ExtremityConstructor;

        public LimbConstructor ConnectedLimb { get; set; }

        public override bool CanMirror => true; // Always mirror extremities.
        #endregion

        #region Methods
        public override void Attach(AttachedBodyPart attachedBodyPart)
        {
            base.Attach(attachedBodyPart);

            // Connected Limb
            foreach (LimbConstructor limb in CreatureConstructor.Root.GetComponentsInChildren<LimbConstructor>())
            {
                if (limb.AttachedLimb.GUID == AttachedExtremity.connectedLimbGUID)
                {
                    ConnectToLimb(limb);
                    break;
                }
            }
        }

        public override void Flip()
        {
            base.Flip();

            // Connected Limb
            FlippedExtremity.ConnectToLimb(ConnectedLimb.FlippedLimb);
        }

        public void ConnectToLimb(LimbConstructor limb)
        {
            ConnectedLimb = limb;
            ConnectedLimb.ConnectedExtremity = this;
        }
        public void DisconnectFromLimb()
        {
            if (ConnectedLimb != null)
            {
                ConnectedLimb.ConnectedExtremity = null;
                ConnectedLimb = null;
            }
        }

        public override void SetAttached(AttachedBodyPart abp)
        {
            if (abp.boneIndex == -1)
            {
                abp = new AttachedExtremity(abp.bodyPartID);
            }
            base.SetAttached(abp);
        }

        /// <summary>
        /// Set the connected limb's GUID and swap this extremity with its connected limb (if attached prior to its connected limb).
        /// This is necessary, as a connected limb must be constructed before its connected extremity, to ensure that it is found when an extremity is attached.
        /// </summary>
        public override void UpdateAttachmentConfiguration()
        {
            base.UpdateAttachmentConfiguration();

            AttachedExtremity.connectedLimbGUID = ConnectedLimb.AttachedBodyPart.GUID;

            List<AttachedBodyPart> attachedBodyParts = CreatureConstructor.Data.AttachedBodyParts;
            int extremityIndex = -1, connectedLimbIndex = -1;

            // Determine extremity and its connected limb's indices.
            for (int i = 0; i < attachedBodyParts.Count; i++)
            {
                AttachedBodyPart current = attachedBodyParts[i];

                if (current.GUID == ConnectedLimb.AttachedBodyPart.GUID)
                {
                    connectedLimbIndex = i;
                }
                else if (current.GUID == AttachedBodyPart.GUID)
                {
                    extremityIndex = i;
                }
            }

            // Swap if extremity was attached prior to its connected limb.
            if (connectedLimbIndex > extremityIndex)
            {
                AttachedBodyPart temp = attachedBodyParts[connectedLimbIndex];
                attachedBodyParts[connectedLimbIndex] = attachedBodyParts[extremityIndex];
                attachedBodyParts[extremityIndex] = temp;
            }
        }
        #endregion
    }
}