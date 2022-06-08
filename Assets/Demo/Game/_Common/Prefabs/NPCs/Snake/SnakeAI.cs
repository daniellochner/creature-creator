// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SnakeAI : AnimalAI
    {
        #region Fields
        [SerializeField] private TrackRegion strikeRegion;
        #endregion

        #region Methods
        public override void Start()
        {
            base.Start();

            strikeRegion.OnTrack += delegate
            {
                if (currentStateId == "WAN")
                {
                    ChangeState("STR");
                }
            };
            strikeRegion.OnLoseTrackOf += delegate
            {
                if (strikeRegion.tracked.Count == 0)
                {
                    ChangeState("WAN");
                }
            };
        }

        public override void Follow(Transform target)
        {
            base.Follow(target);
            strikeRegion.enabled = false;
        }
        public override void StopFollowing()
        {
            base.StopFollowing();
            strikeRegion.enabled = true;
        }
        #endregion

        #region Inner Classes
        [Serializable]
        public class Striking : BaseState
        {
            [SerializeField] private float rotSmoothing;
            [SerializeField] private float maxStrikeDistance;
            [SerializeField] private float minStrikeAngle;
            [SerializeField] private MinMax strikeCooldown;
            private Coroutine strikeCoroutine;

            public SnakeAI SnakeAI => StateMachine as SnakeAI;

            public override void Enter()
            {
                strikeCoroutine = SnakeAI.StartCoroutine(StrikeRoutine());
                SnakeAI.agent.updateRotation = false;
            }
            public override void Exit()
            {
                SnakeAI.StopCoroutine(strikeCoroutine);
                SnakeAI.creature.Animator.InteractTarget = null;
                SnakeAI.agent.updateRotation = true;
            }

            private IEnumerator StrikeRoutine()
            {
                while (IsActive)
                {
                    // Sit-And-Wait...
                    float angle = Mathf.Infinity, distance = Mathf.Infinity;
                    while (angle > minStrikeAngle || distance > maxStrikeDistance)
                    {
                        Transform target = SnakeAI.creature.Animator.InteractTarget = SnakeAI.strikeRegion.Nearest.transform;

                        Vector3 dir = Vector3.ProjectOnPlane(target.position - SnakeAI.transform.position, SnakeAI.transform.up);
                        SnakeAI.transform.rotation = Quaternion.Slerp(SnakeAI.transform.rotation, Quaternion.LookRotation(dir), rotSmoothing * Time.deltaTime);

                        angle = Vector3.Angle(SnakeAI.transform.forward, dir);
                        distance = Vector3.Distance(target.position, SnakeAI.transform.position);

                        yield return null;
                    }

                    // Strike!
                    SnakeAI.Animator.SetTrigger("Strike");

                    // Rest...
                    yield return new WaitForSeconds(strikeCooldown.Random);
                }
            }
        }
        #endregion
    }
}