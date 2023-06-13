// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using DanielLochner.Assets.CreatureCreator.Animations;
using System;
using System.Collections;
using UnityEngine;

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

            if (PVE)
            {
                SetupTrackRegionBuffer(trackRegion);

                trackRegion.OnTrack += delegate
                {
                    if (currentStateId == "WAN")
                    {
                        ChangeState("STR");
                    }
                };
                trackRegion.OnLoseTrackOf += delegate
                {
                    if (Creature.Health.Health <= 0) return;

                    if (trackRegion.tracked.Count == 0)
                    {
                        ChangeState("WAN");
                    }
                };
            }
        }
        #endregion

        #region Nested
        [Serializable]
        public class Striking : Targeting
        {
            [SerializeField] private float maxStrikeDistance;
            [SerializeField] private float minStrikeAngle;
            [SerializeField] private MinMax strikeCooldown;
            [SerializeField] private float strikeRadius;
            [SerializeField] private MinMax strikeDamage;
            [SerializeField] private PlayerEffects.Sound[] strikeSounds;
            private Coroutine strikeCoroutine;
            private bool hasDealtDamage;

            public SnakeAI SnakeAI => StateMachine as SnakeAI;

            public override void Enter()
            {
                base.Enter();
                SnakeAI.Agent.ResetPath();
                strikeCoroutine = SnakeAI.StartCoroutine(StrikingRoutine());
            }
            public override void UpdateLogic()
            {
                if (!SnakeAI.IsAnimationState("Strike"))
                {
                    UpdateLookDir();
                    HandleLookAt();
                }
            }
            public override void Exit()
            {
                base.Exit();
                SnakeAI.StopCoroutine(strikeCoroutine);
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
                    hasDealtDamage = false;
                    Vector3 head = SnakeAI.Creature.Animator.Mouths[0].transform.position;
                    Vector3 displacement = Vector3.ProjectOnPlane(target.transform.position - head, SnakeAI.Creature.transform.up);
                    float d = displacement.magnitude;

                    SnakeAI.Animator.GetBehaviour<Bite>().OnBiteMouth = OnBiteMouth;
                    SnakeAI.Animator.GetBehaviour<Bite>().OnBite = OnBite;

                    SnakeAI.Params.SetTriggerWithValue("Body_Strike", "Body_Strike_Distance", d);

                    // Rest...
                    yield return new WaitForSeconds(strikeCooldown.Random);
                }
            }

            private void OnBiteMouth(MouthAnimator mouth)
            {
                if (!hasDealtDamage)
                {
                    Collider[] colliders = Physics.OverlapSphere(mouth.transform.position, strikeRadius);
                    foreach (Collider collider in colliders)
                    {
                        CreatureBase creature = collider.GetComponent<CreatureBase>();
                        if (creature != null && creature != SnakeAI.Creature)
                        {
                            creature.Health.TakeDamage(strikeDamage.Random, DamageReason.BiteAttack, SnakeAI.Creature.Constructor.Data.Name);
                            hasDealtDamage = true;
                        }
                    }
                }
            }
            private void OnBite()
            {
                SnakeAI.Creature.Effects.PlaySound(strikeSounds);
            }
        }
        #endregion
    }
}