// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CowAI : AnimalAI
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
                        ChangeState("CHA");
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
        public class Charging : BaseState
        {
            [SerializeField] private float chargeSpeedMultiplier;
            [SerializeField] private float chargeUpTime;
            [SerializeField] private PlayerEffects.Sound[] chargeSounds;
            [Space]
            [SerializeField] private float restTime;
            [SerializeField] private PlayerEffects.Sound[] restSounds;
            [Space]
            [SerializeField] private float hornDistance;
            [SerializeField] private Vector3 chargeForce;
            [SerializeField] private MinMax chargeDamage;
            [SerializeField] private PlayerEffects.Sound[] impactSounds;
            private Coroutine chargingCoroutine;

            private CowAI CowAI => StateMachine as CowAI;

            public override void Enter()
            {
                CowAI.ResetPath();
                CowAI.Agent.speed *= chargeSpeedMultiplier;

                Charge(CowAI.trackRegion.Nearest.transform);
            }
            public override void Exit()
            {
                CowAI.TryStopCoroutine(chargingCoroutine);

                CowAI.Agent.speed /= chargeSpeedMultiplier;
            }

            private void Charge(Transform charged)
            {
                CowAI.StopStartCoroutine(ChargeRoutine(charged), ref chargingCoroutine);
            }
            private IEnumerator ChargeRoutine(Transform charged)
            {
                // Rotate to charged.
                Quaternion cur = CowAI.transform.rotation;
                Quaternion tar = Quaternion.LookRotation(Vector3.ProjectOnPlane(charged.position - CowAI.transform.position, CowAI.transform.up));
                CowAI.Agent.updateRotation = false;
                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float progress)
                {
                    CowAI.transform.rotation = Quaternion.Slerp(cur, tar, progress);
                }, 1f);
                CowAI.Agent.updateRotation = true;

                // Charge!
                CowAI.Params.SetBool("Head_IsTilted", true);
                CowAI.Creature.Effects.PlaySound(chargeSounds);
                yield return new WaitForSeconds(chargeUpTime);
                CowAI.Agent.SetDestination(charged.position);
                List<Collider> hit = new List<Collider>();
                while (CowAI.IsMovingToPosition)
                {
                    Vector3 checkPos = CowAI.transform.position + CowAI.transform.forward * hornDistance;

                    Collider[] colliders = Physics.OverlapSphere(checkPos, 0.5f);
                    foreach (Collider collider in colliders)
                    {
                        if (hit.Contains(collider)) continue;
                        hit.Add(collider);

                        CreatureBase creature = collider.GetComponent<CreatureBase>();
                        if (creature != null && creature != CowAI.Creature)
                        {
                            Vector3 dir = (creature.transform.position - CowAI.transform.position).normalized;
                            Vector3 force = dir * chargeForce.z + creature.transform.up * chargeForce.y;

                            creature.Health.TakeDamage(chargeDamage.Random, DamageReason.Charge);
                            CowAI.Creature.Effects.PlaySound(impactSounds);
                        }
                    }

                    yield return null;
                }

                // Rest...
                CowAI.Params.SetBool("Head_IsTilted", false);
                CowAI.Creature.Effects.PlaySound(restSounds);
                yield return new WaitForSeconds(restTime);

                // Charge again?
                if (CowAI.trackRegion.tracked.Count != 0)
                {
                    Charge(CowAI.trackRegion.Nearest.transform);
                }
                else
                {
                    CowAI.ChangeState("WAN");
                }
            }
        }
        #endregion
    }
}