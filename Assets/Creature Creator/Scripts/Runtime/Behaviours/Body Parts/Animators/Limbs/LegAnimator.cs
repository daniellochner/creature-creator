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
        private Coroutine moveToTargetCoroutine;
        #endregion

        #region Properties
        public Transform Anchor => anchor;

        public LegConstructor LegConstructor => LimbConstructor as LegConstructor;
        public LegAnimator FlippedLeg => Flipped as LegAnimator;

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
        public bool IsMovingToTarget
        {
            get; private set;
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
            anchor.SetParent(LimbConstructor.Extremity, false);
        }

        public override void Restructure(bool isAnimated)
        {
            base.Restructure(isAnimated);

            if (isAnimated)
            {
                anchor.SetParent(Dynamic.Transform);
                anchor.SetPositionAndRotation(LimbConstructor.Extremity.position, LimbConstructor.Extremity.rotation);
            }
            else
            {
                anchor.SetParent(LimbConstructor.Extremity);
                anchor.localPosition = Vector3.zero;
                anchor.localRotation = Quaternion.identity;
                anchor.localScale = Vector3.one;
            }
        }

        public void MoveToPosition(Vector3 position, float timeToMove, float liftHeight)
        {
            if (moveToTargetCoroutine != null)
            {
                StopCoroutine(moveToTargetCoroutine);
            }
            moveToTargetCoroutine = StartCoroutine(MoveToPositionRoutine(position, timeToMove, liftHeight));
        }
        private IEnumerator MoveToPositionRoutine(Vector3 position, float timeToMove, float liftHeight)
        {
            IsMovingToTarget = true;

            Vector3 initialPosition = anchor.position;
            float timeElapsed = 0f;
            float progress = 0f;

            while (progress < 1)
            {
                timeElapsed += Time.deltaTime;
                progress = timeElapsed / timeToMove;

                Vector3 targetPosition = Vector3.Lerp(initialPosition, position, progress);
                targetPosition += CreatureAnimator.transform.up * Mathf.Sin(progress * Mathf.PI) * liftHeight;

                anchor.position = position;

                yield return null;
            }

            IsMovingToTarget = false;
        }
        #endregion
    }
}