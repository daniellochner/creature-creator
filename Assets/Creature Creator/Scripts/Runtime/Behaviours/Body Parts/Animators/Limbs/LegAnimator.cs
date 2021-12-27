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
        [Header("Leg")]
        [SerializeField] private AnimationCurve moveCurve;
        [SerializeField] private float timeToMove = 0.25f;
        [SerializeField] private float liftHeight = 0.2f;

        private Transform anchor;
        private float maxLength;
        private Vector3? prevPosition = null, velocity;
        #endregion

        #region Properties
        public Transform Anchor => anchor;

        public LegConstructor LegConstructor => LimbConstructor as LegConstructor;
        public LegAnimator FlippedLeg => Flipped as LegAnimator;

        public Vector3 Velocity => (Vector3)velocity;
        public float TimeToMove => timeToMove;

        public float Length
        {
            get => Vector3.Distance(transform.position, LegConstructor.Extremity.position);
        }
        public float MaxLength
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
        public Vector3 DefaultFootPosition
        {
            get => defaultBonePositions[defaultBonePositions.Length - 1];
        }

        public bool IsGrounded
        {
            get
            {
                return Physics.Raycast(LegConstructor.Extremity.position, -CreatureAnimator.transform.up, LegConstructor.ConnectedFoot.Offset + 0.001f, LayerMask.GetMask("Ground"));
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

            if (prevPosition != null)
            {
                velocity = (transform.position - prevPosition) / Time.deltaTime;
            }
            prevPosition = transform.position;
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

        public IEnumerator MoveToTargetRoutine(Vector3 targetPosition, Quaternion targetRotation)
        {
            IsMovingToTarget = true;

            Vector3 initialPosition = LegConstructor.Extremity.position;
            Quaternion initialRotation = LegConstructor.Extremity.rotation;


            //float t = Mathf.Clamp01(0f, Vector3.Distance(initialPosition, targetPosition) / 0.2f);
            

            float timeElapsed = 0f, progress = 0f;
            while (progress < 1f)
            {
                timeElapsed += Time.deltaTime;
                progress = timeElapsed / timeToMove;

                // Position
                Vector3 position = Vector3.Lerp(initialPosition, targetPosition, progress);
                position += (liftHeight * moveCurve.Evaluate(progress)) * CreatureAnimator.transform.up; // Mathf.Sin(progress * Mathf.PI)
                anchor.position = position;

                // Rotation
                Quaternion rotation = Quaternion.Slerp(initialRotation, targetRotation, progress);
                anchor.rotation = rotation;

                yield return null;
            }

            IsMovingToTarget = false;
        }
        #endregion
    }
}