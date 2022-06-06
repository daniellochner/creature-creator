// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
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
                ChangeState("STR");
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
            [SerializeField] private string[] strikeNoises;
            [SerializeField] private MinMax strikeCooldown;
            [SerializeField] private float maxStrikeDistance;
            private float strikeTimeLeft;

            public SnakeAI SnakeAI => StateMachine as SnakeAI;

            public Striking(SnakeAI snakeAI) : base(snakeAI) { }

            public override void Enter()
            {
                SnakeAI.creature.Animator.Animator.SetTrigger("Look");
                SnakeAI.agent.SetDestination(SnakeAI.transform.position);

                strikeTimeLeft = 0f;
            }
            public override void UpdateLogic()
            {
                HandleStrike();
                HandleLookAt();
            }
            public override void Exit()
            {
                SnakeAI.creature.Animator.LookTarget = SnakeAI.creature.Animator.InteractTarget = null;
            }

            private void HandleStrike()
            {
                if (SnakeAI.creature.Animator.InteractTarget != null)
                {
                    TimerUtility.OnTimer(ref strikeTimeLeft, strikeCooldown.Random, Time.deltaTime, Strike);
                }
            }
            private void HandleLookAt()
            {
                // Determine the nearest creature within this snake's maximum striking distance
                Transform nearestCreature = SnakeAI.strikeRegion.Nearest.transform;

                // Set the snake's look target to be the nearest creature, and start looking at that creature
                if (nearestCreature != null && SnakeAI.creature.Animator.LookTarget != nearestCreature)
                {
                    SnakeAI.creature.Animator.LookTarget = nearestCreature;
                }
                else
                if (nearestCreature == null)
                {
                    SnakeAI.creature.Animator.LookTarget = null;
                }

                // Set the snake's interact target to be the same creature if it is near enough
                float distance = Vector3.Distance(nearestCreature.position, SnakeAI.transform.position);
                if (distance < maxStrikeDistance)
                {
                    SnakeAI.creature.Animator.InteractTarget = nearestCreature;
                }
                else if (SnakeAI.creature.Animator.InteractTarget != null)
                {
                    SnakeAI.creature.Animator.InteractTarget = null;
                    strikeTimeLeft = strikeCooldown.Random; // Set back to the maximum time left if the target creature is lost
                }
            }

            private void Strike()
            {
                SnakeAI.creature.Animator.Animator.SetTrigger("Strike");
                SnakeAI.creature.Effector.PlaySound(strikeNoises);
            }
        }
        #endregion
    }
}