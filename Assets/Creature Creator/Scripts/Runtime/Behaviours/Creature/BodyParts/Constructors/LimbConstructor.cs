// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LimbConstructor : BodyPartConstructor
    {
        #region Fields
        [Header("Limb")]
        [SerializeField] private Transform[] bones;
        [SerializeField] private Transform root;
        [SerializeField] private Transform extremity;

        private bool isHandlingAlignment;
        private LookAtConstraint[] boneConstraints;
        private RotationConstraint extremityConstraint;
        #endregion

        #region Properties
        public Limb Limb => BodyPart as Limb;

        public Transform[] Bones
        {
            get => bones;
            set
            {
                bones = value;
            }
        }
        public Transform Root
        {
            get => root;
            set
            {
                root = value;
            }
        }
        public Transform Extremity
        {
            get => extremity;
            set
            {
                extremity = value;
            }
        }

        public bool IsHandlingAlignment
        {
            get => isHandlingAlignment;
            set
            {
                isHandlingAlignment = value;

                foreach (LookAtConstraint boneConstraint in boneConstraints)
                {
                    boneConstraint.constraintActive = isHandlingAlignment;
                }
                extremityConstraint.constraintActive = isHandlingAlignment;
            }
        }

        public AttachedLimb AttachedLimb => AttachedBodyPart as AttachedLimb;
        public LimbConstructor FlippedLimb => Flipped as LimbConstructor;

        public ExtremityConstructor ConnectedExtremity { get; set; }

        public Action<int, float> OnSetWeight { get; set; }
        public Action<ExtremityConstructor> OnConnectExtremity { get; set; }
        public Action OnDisconnectExtremity { get; set; }

        public override bool CanMirror => true; // Always mirror limbs.
        #endregion

        #region Methods
        private void OnEnable()
        {
            if (ConnectedExtremity != null)
            {
                ConnectedExtremity.gameObject.SetActive(true);
            }
            IsHandlingAlignment = true;
        }
        private void OnDisable()
        {
            if (ConnectedExtremity != null)
            {
                ConnectedExtremity.gameObject.SetActive(false);
            }
            IsHandlingAlignment = false;
        }

        public override void Initialize()
        {
            base.Initialize();

            boneConstraints = new LookAtConstraint[bones.Length - 1];
            for (int i = 0; i < bones.Length - 1; i++)
            {
                boneConstraints[i] = bones[i].gameObject.GetOrAddComponent<LookAtConstraint>();
            }
            extremityConstraint = bones[bones.Length - 1].gameObject.GetComponent<RotationConstraint>();
        }

        public override void Attach(AttachedBodyPart abp)
        {
            base.Attach(abp);

            // Bones
            for (int i = 0; i < AttachedLimb.bones.Count; i++)
            {
                Bones[i].position = CreatureConstructor.transform.TransformPoint(AttachedLimb.bones[i].position);
                Bones[i].rotation = CreatureConstructor.transform.rotation * AttachedLimb.bones[i].rotation;
                SetWeight(i, AttachedLimb.bones[i].weight);
            }
        }
        public override void Detach()
        {
            if (ConnectedExtremity != null)
            {
                ConnectedExtremity.Detach(); // Detach connected extremity before detaching limb.
            }
            base.Detach();
        }

        public override void Add()
        {
            base.Add();
            CreatureConstructor.Limbs.Add(this);
        }
        public override void Remove()
        {
            base.Remove();
            CreatureConstructor.Limbs.Remove(this);
        }
        public override void Flip()
        {
            base.Flip();

            // Bones
            for (int i = 0; i < Bones.Length; i++)
            {
                // Position
                Vector3 localBonePosition = CreatureConstructor.transform.InverseTransformPoint(Bones[i].position);
                localBonePosition.x *= -1;
                FlippedLimb.Bones[i].position = CreatureConstructor.transform.TransformPoint(localBonePosition);

                // Rotation
                Quaternion worldRotation = CreatureConstructor.transform.rotation * Quaternion.Euler(Bones[i].eulerAngles.x, -Bones[i].eulerAngles.y, -Bones[i].eulerAngles.z);
                FlippedLimb.Bones[i].rotation = worldRotation;

                // Weight
                FlippedLimb.SetWeight(i, GetWeight(i));
            }
        }

        public float GetWeight(int index)
        {
            return SkinnedMeshRenderer.GetBlendShapeWeight(index);
        }
        public void SetWeight(int index, float weight)
        {
            if (SkinnedMeshRenderer.sharedMesh.blendShapeCount > 0)
            {
                weight = Mathf.Clamp(weight, 0f, 100f);

                AttachedLimb.bones[index].weight = weight;
                SkinnedMeshRenderer.SetBlendShapeWeight(index, weight);

                OnSetWeight?.Invoke(index, weight);
            }
        }
        public void AddWeight(int index, float amount)
        {
            SetWeight(index, GetWeight(index) + amount);
        }
        public void RemoveWeight(int index, float amount)
        {
            SetWeight(index, GetWeight(index) - amount);
        }

        public override void SetFlipped(BodyPartConstructor bpc)
        {
            IsFlipped = true;

            Flipped = bpc;
            bpc.Flipped = this;

            Root.localScale = new Vector3(-1f, 1f, 1f);
        }
        public override void SetAttached(AttachedBodyPart abp)
        {
            if (abp.boneIndex == -1)
            {
                abp = new AttachedLimb(abp.bodyPartID)
                {
                    bones = new List<Bone>()
                };

                // Initialize
                for (int i = 0; i < bones.Length; i++)
                {
                    (abp as AttachedLimb).bones.Add(new Bone());
                }
            }
            base.SetAttached(abp);
        }

        public override void UpdateAttachmentConfiguration()
        {
            base.UpdateAttachmentConfiguration();

            // Bones
            for (int boneIndex = 0; boneIndex < bones.Length; boneIndex++)
            {
                AttachedLimb.bones[boneIndex].position = CreatureConstructor.transform.InverseTransformPoint(Bones[boneIndex].position);
                AttachedLimb.bones[boneIndex].rotation = Quaternion.Inverse(CreatureConstructor.transform.rotation) * bones[boneIndex].rotation;
                AttachedLimb.bones[boneIndex].weight = GetWeight(boneIndex);
            }
        }
        #endregion
    }
}