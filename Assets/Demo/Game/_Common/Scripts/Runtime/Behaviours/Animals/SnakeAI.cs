// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SnakeAI : AnimalAI
    {
        #region Fields
        [SerializeField] private TrackRegion trackRegion;
        #endregion

        #region Methods
        public override void Start()
        {
            base.Start();

            trackRegion.OnTrack += delegate
            {
                if (currentStateId == "WAN")
                {
                    ChangeState("STR");
                }
            };
            trackRegion.OnLoseTrackOf += delegate
            {
                if (trackRegion.tracked.Count == 0)
                {
                    ChangeState("WAN");
                }
            };

            foreach (DampedTransform dt in GetComponentsInChildren<DampedTransform>())
            {
                dt.weight = 0.1f;
            }
        }

        public override void Follow(Transform target)
        {
            base.Follow(target);
            trackRegion.enabled = false;
        }
        public override void StopFollowing()
        {
            base.StopFollowing();
            trackRegion.enabled = true;
        }
        #endregion

        #region Nested
        [Serializable]
        public class Striking : BaseState
        {
            [SerializeField] private float lookAtSmoothing;
            [SerializeField] private float maxStrikeDistance;
            [SerializeField] private float minStrikeAngle;
            [SerializeField] private MinMax strikeCooldown;
            private Coroutine strikeCoroutine;
            private CreatureBase target;
            private Vector3 lookDir;

            public SnakeAI SnakeAI => StateMachine as SnakeAI;

            public override void Enter()
            {
                strikeCoroutine = SnakeAI.StartCoroutine(StrikingRoutine());
            }
            public override void UpdateLogic()
            {
                if (!SnakeAI.Animator.GetCurrentAnimatorStateInfo(0).IsName("Strike"))
                {
                    UpdateLookDir();
                    HandleLookAt();
                }
            }
            public override void Exit()
            {
                SnakeAI.StopCoroutine(strikeCoroutine);
            }

            private void UpdateTarget()
            {
                Transform nearest = SnakeAI.Creature.Animator.InteractTarget = SnakeAI.trackRegion.Nearest.transform;
                if (target == null || target.transform != nearest)
                {
                    target = nearest.GetComponent<CreatureBase>();
                }
            }
            private void HandleLookAt()
            {
                SnakeAI.transform.rotation = Quaternion.Slerp(SnakeAI.transform.rotation, Quaternion.LookRotation(lookDir), lookAtSmoothing * Time.deltaTime);
            }
            private void UpdateLookDir()
            {
                lookDir = Vector3.ProjectOnPlane(target.transform.position - SnakeAI.transform.position, SnakeAI.transform.up).normalized;
            }

            private IEnumerator StrikingRoutine()
            {
                while (IsActive)
                {
                    // Sit-And-Wait...
                    float angle = Mathf.Infinity, distance = Mathf.Infinity;
                    while (angle > minStrikeAngle || distance > maxStrikeDistance)
                    {
                        UpdateTarget();
                        angle = Vector3.Angle(SnakeAI.transform.forward, lookDir);
                        distance = Vector3.Distance(target.transform.position, SnakeAI.transform.position);
                        yield return null;
                    }

                    // Strike!
                    SnakeAI.Params.SetTrigger("Body_Strike");

                    // Rest...
                    yield return new WaitForSeconds(strikeCooldown.Random);
                }
            }
        }
        #endregion
    }
}