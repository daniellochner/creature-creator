// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(LimbConstructor))]
    public class LimbAnimator : BodyPartAnimator
    {
        #region Fields
        protected Vector3[] defaultPositions;

        protected Transform target, limb;
        protected ChainIKConstraint limbIK; //protected TwoBoneIKConstraint limbIK;
        #endregion

        #region Properties
        public LimbConstructor LimbConstructor => BodyPartConstructor as LimbConstructor;
        public LimbAnimator FlippedLimb => Flipped as LimbAnimator;

        public Transform Target => target;
        public Vector3[] DefaultPositions => defaultPositions;
        #endregion

        #region Methods
        private void Update()
        {
            if (!CreatureAnimator.IsAnimated)
            {
                target.position = LimbConstructor.Extremity.position;
            }
        }
        protected virtual void LateUpdate()
        {
            UpdateBoneRotations();
        }
        protected virtual void OnDestroy()
        {
            if (limbIK != null)
            {
                Destroy(limbIK.gameObject);
            }
        }

        public override void Setup(CreatureAnimator creatureAnimator)
        {
            base.Setup(creatureAnimator);

            limb = new GameObject("Limb").transform;
            limb.SetParent(creatureAnimator.Rig, false);

            target = new GameObject("Target").transform;
            target.SetParent(limb, false);

            //if (LimbConstructor.Bones.Length == 3)
            //{
            //    limbIK = limb.gameObject.AddComponent<TwoBoneIKConstraint>();
            //    limbIK.Reset();

            //    limbIK.data.root = LimbConstructor.Bones[0];
            //    limbIK.data.mid = LimbConstructor.Bones[1];
            //    limbIK.data.tip = LimbConstructor.Bones[2];
            //    limbIK.data.target = target;
            //    limbIK.data.targetRotationWeight = 0;
            //}
            //else
            {
                limbIK = limb.gameObject.AddComponent<ChainIKConstraint>();
                limbIK.Reset();

                limbIK.data.root = LimbConstructor.Bones[0];
                limbIK.data.tip = LimbConstructor.Bones[LimbConstructor.Bones.Length - 1];
                limbIK.data.target = target;
                limbIK.data.tipRotationWeight = 0;
            }

            LimbConstructor.OnAttach += delegate
            {
                limb.name = $"Limb ({name})";
                FlippedLimb.limb.name = $"Limb ({FlippedLimb.name})";
            };
        }

        public virtual void Reposition(bool isAnimated)
        {
            if (isAnimated)
            {
                defaultPositions = new Vector3[LimbConstructor.Bones.Length];
                for (int i = 0; i < defaultPositions.Length; i++)
                {
                    defaultPositions[i] = CreatureAnimator.CreatureConstructor.Body.InverseTransformPoint(LimbConstructor.Bones[i].position);
                }
            }
            else if (defaultPositions != null)
            {
                for (int i = 0; i < defaultPositions.Length; i++)
                {
                    LimbConstructor.Bones[i].position = CreatureAnimator.CreatureConstructor.Body.TransformPoint(defaultPositions[i]);
                }
                defaultPositions = null;
            }
        }
        public virtual void Restructure(bool isAnimated)
        {
            // Limb
            for (int i = 1; i < LimbConstructor.Bones.Length; i++)
            {
                LimbConstructor.Bones[i].SetParent(isAnimated ? LimbConstructor.Bones[i - 1] : LimbConstructor.Root);
            }

            // Connected extremity
            if (LimbConstructor.ConnectedExtremity != null)
            {
                LimbConstructor.ConnectedExtremity.transform.SetParent(isAnimated ? LimbConstructor.Extremity : CreatureAnimator.CreatureConstructor.Bones[LimbConstructor.AttachedLimb.boneIndex]);
            }
        }

        public void UpdateBoneRotations()
        {
            if (CreatureAnimator.IsAnimated)
            {
                target.rotation = LimbConstructor.Bones[LimbConstructor.Bones.Length - 1].rotation;
            }
            else
            {
                LimbConstructor.Realign();
            }
        }
        #endregion
    }
}