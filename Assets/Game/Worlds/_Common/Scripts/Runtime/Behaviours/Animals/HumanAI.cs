// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using DanielLochner.Assets.CreatureCreator.Animations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class HumanAI : AnimalAI
    {
        #region Fields
        [SerializeField] protected TrackRegion nearTrackRegion;
        [SerializeField] protected TrackRegion farTrackRegion;
        #endregion

        #region Methods
        public override void Start()
        {
            base.Start();

            if (PVE)
            {
                SetupTrackRegionBuffer(nearTrackRegion);
                SetupTrackRegionBuffer(farTrackRegion);

                nearTrackRegion.OnTrack += delegate
                {
                    ChangeState("SPI");
                };
                nearTrackRegion.OnLoseTrackOf += delegate
                {
                    if (Creature.Health.Health <= 0) return;
                    ChangeState("SHO");
                };

                farTrackRegion.OnTrack += delegate
                {
                    ChangeState("SHO");
                };
                farTrackRegion.OnLoseTrackOf += delegate
                {
                    ChangeState("WAN");
                };
            }
        }
        #endregion

        #region Nested
        [Serializable]
        public class Shooting : Targeting
        {
            [SerializeField] private float baseShootDistance;
            [SerializeField] private float baseShootSpeed;
            [SerializeField] private MinMax shootDelay;
            private Coroutine shootingCoroutine;

            public HumanAI HumanAI => StateMachine as HumanAI;

            public List<BodyPartLauncher> Launchers
            {
                get
                {
                    List<BodyPartLauncher> launchers = new List<BodyPartLauncher>();
                    foreach (BodyPartConstructor constructor in HumanAI.Creature.Constructor.BodyParts)
                    {
                        BodyPartLauncher launcher = constructor.GetComponent<BodyPartLauncher>();
                        if (launcher != null)
                        {
                            launchers.Add(launcher);
                            launchers.Add(launcher.Flipped);
                        }
                    }
                    return launchers;
                }
            }

            public override void Enter()
            {
                base.Enter();
                UpdateTarget();

                HumanAI.ResetPath();
                shootingCoroutine = HumanAI.StartCoroutine(ShootingRoutine());
            }
            public override void UpdateLogic()
            {
                UpdateLookDir();
                HandleLookAt();
            }
            public override void Exit()
            {
                base.Exit();
                HumanAI.StopCoroutine(shootingCoroutine);
            }

            private IEnumerator ShootingRoutine()
            {
                while (IsActive)
                {
                    // Move Closer.
                    float distance = Vector3.Distance(target.transform.position, HumanAI.transform.position);

                    // Shoot!
                    foreach (BodyPartLauncher launcher in Launchers)
                    {
                        launcher.Speed = baseShootSpeed * (distance / baseShootDistance);
                        HumanAI.Creature.Launcher.Launch(launcher);
                    }

                    // Wait...
                    yield return new WaitForSeconds(shootDelay.Random);
                }
            }
        }

        [Serializable]
        public class Spinning : Targeting
        {
            [SerializeField] private float maxSpinDistance;
            [SerializeField] private MinMax spinDelay;
            [SerializeField] private float spinRadius;
            [SerializeField] private MinMax spinDamage;
            [SerializeField] private PlayerEffects.Sound[] spinSounds;
            private Coroutine spinningCoroutine;
            private List<CreatureBase> damagedCreatures = new List<CreatureBase>();

            public HumanAI HumanAI => StateMachine as HumanAI;

            public override void Enter()
            {
                base.Enter();
                UpdateTarget();

                HumanAI.ResetPath();
                spinningCoroutine = HumanAI.StartCoroutine(SpinningRoutine());
            }
            public override void UpdateLogic()
            {
                if (!HumanAI.IsAnimationState("Spin"))
                {
                    UpdateLookDir();

                    Vector3 offset = lookDir * GetTargetDistance(HumanAI.Creature, target);
                    HumanAI.Agent.SetDestination(target.transform.position - offset);

                    HandleLookAt();

                    NavMeshPath path = new NavMeshPath();
                    HumanAI.Agent.CalculatePath(target.transform.position, path);
                    if (path.status != NavMeshPathStatus.PathComplete)
                    {
                        HumanAI.ChangeState("SWI");
                    }
                }
            }
            public override void Exit()
            {
                base.Exit();
                HumanAI.StopCoroutine(spinningCoroutine);
            }

            private IEnumerator SpinningRoutine()
            {
                while (IsActive)
                {
                    // Move Closer.
                    float distance = Mathf.Infinity;
                    while (distance > GetTargetDistance(HumanAI.Creature, target, maxSpinDistance))
                    {
                        UpdateTarget();
                        distance = Vector3.Distance(target.transform.position, HumanAI.transform.position);
                        yield return null;
                    }

                    // Spin!
                    damagedCreatures.Clear();

                    HumanAI.Animator.GetBehaviour<Spin>().OnSpinArm = OnSpinArm;
                    HumanAI.Animator.GetBehaviour<Spin>().OnSpin = OnSpin;

                    HumanAI.Params.SetTrigger("Body_Spin");

                    // Wait...
                    yield return new WaitForSeconds(spinDelay.Random);
                }
            }

            private void OnSpinArm(ArmAnimator arm)
            {
                Collider[] colliders = Physics.OverlapSphere(arm.LimbConstructor.Extremity.position, spinRadius);
                foreach (Collider collider in colliders)
                {
                    CreatureBase creature = collider.GetComponent<CreatureBase>();
                    if (creature != null && creature != HumanAI.Creature && !damagedCreatures.Contains(creature))
                    {
                        creature.Health.TakeDamage(spinDamage.Random, DamageReason.Spin);
                        damagedCreatures.Add(creature);
                    }
                }
            }
            private void OnSpin()
            {
                HumanAI.Creature.Effects.PlaySound(spinSounds);
            }
        }
        #endregion
    }
}