// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(LegConstructor))]
    public class LegAnimator : LimbAnimator
    {
        #region Fields
        private Transform anchor;

        //private Coroutine moveToAnchorCoroutine;
        #endregion
        
        #region Properties
        public LegConstructor LegConstructor => LimbConstructor as LegConstructor;
        public LegAnimator FlippedLeg => Flipped as LegAnimator;

        public Transform Anchor => anchor;

        public bool IsMovingToTarget { get; private set; }

        public Vector3 ExtremityOffset { get; set; }

        public float Length
        {
            get
            {
                float length = 0;

                for (int i = 0; i < LimbConstructor.Bones.Length - 1; i++)
                {
                    length += Vector3.Distance(LimbConstructor.Bones[i].position, LimbConstructor.Bones[i + 1].position);
                }

                return length;
            }
        }
        #endregion

        #region Methods
        protected override void LateUpdate()
        {
            base.LateUpdate();

            target.SetPositionAndRotation(anchor.position, anchor.rotation);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (anchor != null)
            {
                Destroy(anchor.gameObject);
            }
        }

        public override void Setup(CreatureAnimator creatureAnimator)
        {
            base.Setup(creatureAnimator);

            anchor = new GameObject("Anchor").transform;
            anchor.SetParent(Dynamic.Transform, true);
        }

        public override void Restructure(bool isAnimated)
        {
            base.Restructure(isAnimated);

            anchor.SetParent(isAnimated ? Dynamic.Transform : limb, true);

            ExtremityOffset = transform.position - LegConstructor.Extremity.position;
        }

        public void HandleMovement()
        {
            if (!CreatureAnimator.IsAnimated)
            {
                return;
            }

            if (IsMovingToTarget || FlippedLeg.IsMovingToTarget)
            {
                return;
            }

            Vector3 extremityOffset = CreatureAnimator.CreatureConstructor.Body.TransformVector(Vector3.ProjectOnPlane(defaultBonePositions[defaultBonePositions.Length - 1] / 4f, CreatureAnimator.transform.up));
            Vector3 velocityOffset = (CreatureAnimator.MoveThreshold / 2f) * CreatureAnimator.CreatureConstructor.Body.forward;
            Vector3 origin = transform.position + (extremityOffset + velocityOffset);

            if (Physics.Raycast(origin, -CreatureAnimator.transform.up, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                if (Vector3.Distance(hitInfo.point, anchor.position) > CreatureAnimator.MoveThreshold)
                {
                    StartCoroutine(MoveToTargetRoutine(hitInfo.point));
                }
            }
        }

        private IEnumerator MoveToTargetRoutine(Vector3 targetPosition)
        {
            IsMovingToTarget = true;

            Vector3 initialPosition = anchor.position;
            float timeElapsed = 0f;
            float progress = 0f;

            while (progress < 1)
            {
                timeElapsed += Time.deltaTime;
                progress = timeElapsed / CreatureAnimator.TimeToMove;

                Vector3 position = Vector3.Lerp(initialPosition, targetPosition, progress);
                position += CreatureAnimator.transform.up * Mathf.Sin(progress * Mathf.PI) * CreatureAnimator.LiftHeight;

                anchor.position = position;

                yield return null;
            }

            IsMovingToTarget = false;
        }
        #endregion
    }
}