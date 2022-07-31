// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using DanielLochner.Assets.CreatureCreator.Animations;
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
                strikeCoroutine = SnakeAI.StartCoroutine(StrikingRoutine());
                SnakeAI.Animator.GetBehaviour<Bite>().OnBite += OnBite;
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
                SnakeAI.Animator.GetBehaviour<Bite>().OnBite -= OnBite;
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
                    float d = Vector3.Distance(target.transform.position, SnakeAI.transform.position);
                    SnakeAI.Params.SetTriggerWithValue("Body_Strike", "Body_Strike_Distance", d);

                    // Rest...
                    yield return new WaitForSeconds(strikeCooldown.Random);
                }
            }

            private void OnBite(MouthAnimator mouth)
            {
                SnakeAI.Creature.Effects.PlaySound(strikeSounds);
                if (!hasDealtDamage)
                {
                    Collider[] colliders = Physics.OverlapSphere(mouth.transform.position, strikeRadius);
                    foreach (Collider collider in colliders)
                    {
                        CreatureBase creature = collider.GetComponent<CreatureBase>();
                        if (creature != null && creature.Animator != SnakeAI.Creature)
                        {
                            creature.Health.TakeDamage(strikeDamage.Random);
                            hasDealtDamage = true;
                        }
                    }
                }
            }
        }
        #endregion
    }
}