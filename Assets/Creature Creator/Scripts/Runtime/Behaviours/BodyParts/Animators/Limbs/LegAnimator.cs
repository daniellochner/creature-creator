// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    /// <summary>
    /// Provides useful interface for creature animations involving legs.
    /// </summary>
    [RequireComponent(typeof(LegConstructor))]
    public class LegAnimator : LimbAnimator
    {
        #region Properties
        public LegConstructor LegConstructor => LimbConstructor as LegConstructor;
        public LegAnimator FlippedLeg => Flipped as LegAnimator;

        public Transform Anchor
        {
            get; private set;
        }

        public float MaxLength
        {
            get; private set;
        }
        public float MaxDistance
        {
            get; private set;
        }
        public float MovementTimeScale
        {
            get; set;
        } = 1f;

        public Vector3 DefaultFootPosition
        {
            get => defaultBonePositions[defaultBonePositions.Length - 1];
        }
        public float Length
        {
            get => Vector3.Distance(transform.position, LegConstructor.Extremity.position);
        }

        public bool IsMovingFoot
        {
            get; private set;
        }
        #endregion

        #region Methods
        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (Anchor != null)
            {
                Destroy(Anchor.gameObject);
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            if (LegConstructor.ConnectedFoot != null)
            {
                LegConstructor.ConnectedFoot.transform.forward = CreatureAnimator.transform.forward;
            }
        }

        public override void Setup(CreatureAnimator creatureAnimator)
        {
            base.Setup(creatureAnimator);

            Anchor = new GameObject("Anchor").transform;
            Anchor.SetParent(LimbConstructor.Extremity, false);
        }

        public override void Restructure(bool isAnimated)
        {
            base.Restructure(isAnimated);

            if (isAnimated)
            {
                Anchor.SetParent(Dynamic.Transform);
                Anchor.SetPositionAndRotation(LimbConstructor.Extremity.position, LimbConstructor.Extremity.rotation);
            }
            else
            {
                Anchor.SetParent(LimbConstructor.Extremity);
                Anchor.localPosition = Vector3.zero;
                Anchor.localRotation = Quaternion.identity;
                Anchor.localScale = Vector3.one;
            }
        }
        public override void Reinitialize()
        {
            base.Reinitialize();

            // Max Length
            float length = 0;
            for (int i = 0; i < LimbConstructor.Bones.Length - 1; i++)
            {
                length += Vector3.Distance(LimbConstructor.Bones[i].position, LimbConstructor.Bones[i + 1].position);
            }
            MaxLength = length;

            // Max Distance
            float a = Vector3.ProjectOnPlane(transform.position - LegConstructor.Extremity.position, CreatureAnimator.transform.up).magnitude;
            float c = MaxLength;
            float b = Mathf.Sqrt(Mathf.Pow(c, 2) - Mathf.Pow(a, 2));
            MaxDistance = b;
        }

        protected override void HandleTarget()
        {
            target.SetPositionAndRotation(Anchor.position, Anchor.rotation);
        }

        public IEnumerator MoveFootRoutine(Vector3 targetPosition, Quaternion targetRotation, float timeToMove, float liftHeight)
        {
            IsMovingFoot = true;

            Vector3 initialPosition = LegConstructor.Extremity.position;
            Quaternion initialRotation = LegConstructor.Extremity.rotation;

            yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float progress)
            {
                // Position
                Vector3 position = Vector3.Lerp(initialPosition, targetPosition, progress);
                position += (liftHeight * Mathf.Sin(progress * Mathf.PI)) * CreatureAnimator.transform.up;
                Anchor.position = position;

                // Rotation
                Quaternion rotation = Quaternion.Slerp(initialRotation, targetRotation, progress);
                Anchor.rotation = rotation;
            }, 
            timeToMove, MovementTimeScale);

            IsMovingFoot = false;
        }
        #endregion
    }
}