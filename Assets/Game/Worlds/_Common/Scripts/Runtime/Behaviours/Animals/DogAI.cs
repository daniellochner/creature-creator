// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using DanielLochner.Assets.CreatureCreator.Animations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DogAI : AnimalAI
    {
        #region Fields
        [SerializeField] protected TrackRegion trackRegion;

        private DogBone dogBone;
        #endregion

        #region Methods
        public override void Start()
        {
            base.Start();

            if (PVE)
            {
                SetupTrackRegionBuffer(trackRegion);

                trackRegion.OnTrack += delegate (Collider col)
                {
                    CreatureBase creature = col.GetComponent<CreatureBase>();

                    List<Holdable> held = new List<Holdable>(creature.Holder.Held.Values);
                    dogBone = held.Find(x => x is DogBone) as DogBone;
                    if (dogBone != null)
                    {
                        ChangeState("PAN");
                    }
                    else
                    {
                        if ((HasWeapon(creature) || trackRegion.tracked.Count >= 3) && GetState<Scurrying>("SCU").doghouse != null)
                        {
                            ChangeState("SCU");
                        }
                        else if (currentStateId == "WAN")
                        {
                            ChangeState("BAR");
                        }
                    }
                };
                trackRegion.OnLoseTrackOf += delegate
                {
                    if (Creature.Health.Health <= 0) return;

                    if (currentStateId != "HID" && currentStateId != "SCU" && trackRegion.tracked.Count == 0)
                    {
                        ChangeState("WAN");
                    }
                };
            }
        }

        public void Attack(Collider col)
        {
            if (PVE)
            {
                if (currentStateId == "BAR" || currentStateId == "WAN")
                {
                    ChangeState("BIT");
                }
            }
        }

        private bool HasWeapon(CreatureBase creature)
        {
            foreach (BodyPartConstructor bpc in creature.Constructor.BodyParts)
            {
                if (bpc.BodyPart is Weapon)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Nested
        [Serializable]
        public class Panting : BaseState
        {
            [SerializeField] private PlayerEffects.Sound[] pantSounds;
            [SerializeField] private MinMax pantTime;
            private float pantTimeLeft;

            public DogAI DogAI => StateMachine as DogAI;

            public override void UpdateLogic()
            {
                Transform targetBone = null;
                if (DogAI.dogBone.Dummy == null)
                {
                    targetBone = DogAI.dogBone.transform;
                }
                else
                {
                    targetBone = DogAI.dogBone.Dummy.transform;
                }

                Vector3 lookDir = Vector3.ProjectOnPlane(targetBone.position - DogAI.transform.position, DogAI.transform.up).normalized;
                Vector3 offset = lookDir * GetTargetDistance(DogAI.Creature);
                DogAI.Agent.SetDestination(targetBone.position - offset);

                TimerUtility.OnTimer(ref pantTimeLeft, pantTime.Random, Time.deltaTime, delegate
                {
                    DogAI.Creature.Effects.PlaySound(pantSounds);
                });
            }
            public override void Exit()
            {
                pantTimeLeft = 0;
            }
        }

        [Serializable]
        public class Barking : Targeting
        {
            [SerializeField] private float growlTime;
            [SerializeField] private PlayerEffects.Sound[] growlSounds;
            [Space]
            [SerializeField] private MinMax barkDelayBetween;
            [SerializeField] private MinMax barkDelayWithin;
            [SerializeField] private MinMaxInt barkCount;
            [SerializeField] private PlayerEffects.Sound[] barkSounds;
            private Coroutine barkingCoroutine;

            public DogAI DogAI => StateMachine as DogAI;
            
            public override void Enter()
            {
                DogAI.Agent.ResetPath();
                UpdateTarget();
                barkingCoroutine = DogAI.StartCoroutine(BarkingRoutine());
            }
            public override void UpdateLogic()
            {
                UpdateLookDir();
                HandleLookAt();
            }
            public override void Exit()
            {
                DogAI.StopCoroutine(barkingCoroutine);
            }

            private IEnumerator BarkingRoutine()
            {
                yield return GrowlRoutine();
                while (IsActive)
                {
                    yield return BarkRoutine();
                    yield return new WaitForSeconds(barkDelayBetween.Random);
                    UpdateTarget();
                }
            }

            private IEnumerator GrowlRoutine()
            {
                DogAI.Creature.Effects.PlaySound(growlSounds);

                DogAI.Params.SetBool("Eye_IsGlaring", true);
                yield return new WaitForSeconds(growlTime);
                DogAI.Params.SetBool("Eye_IsGlaring", false);
            }
            private IEnumerator BarkRoutine()
            {
                int barks = barkCount.Random;
                for (int i = 0; i < barks; i++)
                {
                    DogAI.Creature.Effects.PlaySound(barkSounds);
                    DogAI.Params.SetTrigger("Mouth_Bark");
                    yield return new WaitForSeconds(barkDelayWithin.Random);
                }
            }
        }

        [Serializable]
        public class Scurrying : BaseState
        {
            [SerializeField] public GameObject doghouse;
            [SerializeField] private float distanceToDoghouse;
            [SerializeField] private PlayerEffects.Sound[] whimperSounds;

            public DogAI DogAI => StateMachine as DogAI;

            public override void Enter()
            {
                DogAI.Agent.SetDestination(doghouse.transform.position);
                DogAI.Creature.Effects.PlaySound(whimperSounds);
            }
            public override void UpdateLogic()
            {
                if (!Vector3Utility.SqrDistanceComp(DogAI.transform.position, doghouse.transform.position, distanceToDoghouse))
                {
                    DogAI.ChangeState("HID");
                }
            }
        }

        [Serializable]
        public class Hiding : BaseState
        {
            [SerializeField] private MinMax hideTime;
            private float hideTimeLeft;

            public DogAI DogAI => StateMachine as DogAI;

            public override void Enter()
            {
                DogAI.Creature.Loader.HideFromOthers();
                DogAI.Creature.Constructor.Body.gameObject.SetActive(false);
                hideTimeLeft = hideTime.Random;
            }
            public override void UpdateLogic()
            {
                if (DogAI.trackRegion.tracked.Count == 0)
                {
                    TimerUtility.OnTimer(ref hideTimeLeft, hideTime.Random, Time.deltaTime, delegate
                    {
                        DogAI.ChangeState("WAN");
                    });
                }
            }
            public override void Exit()
            {
                DogAI.Creature.Loader.ShowMeToOthers();
                DogAI.Creature.Constructor.Body.gameObject.SetActive(true);
                DogAI.Agent.SetDestination(DogAI.Creature.transform.position);
            }
        }

        [Serializable]
        public class Biting : Targeting
        {
            [SerializeField] private float biteMaxDistance;
            [SerializeField] private float minBiteAngle;
            [SerializeField] private MinMax biteDelay;
            [SerializeField] private float biteRadius;
            [SerializeField] private MinMax biteDamage;
            [SerializeField] private PlayerEffects.Sound[] biteSounds;
            private Coroutine bitingCoroutine;
            private bool hasDealtDamage;

            public DogAI DogAI => StateMachine as DogAI;
            
            public override void Enter()
            {
                base.Enter();
                DogAI.Agent.ResetPath();

                bitingCoroutine = DogAI.StartCoroutine(BitingRoutine());
            }
            public override void UpdateLogic()
            {
                if (!DogAI.IsAnimationState("Strike"))
                {
                    UpdateLookDir();

                    Vector3 offset = lookDir * GetTargetDistance(DogAI.Creature, target);
                    DogAI.Agent.SetDestination(target.transform.position - offset);

                    HandleLookAt();
                }
            }
            public override void Exit()
            {
                base.Exit();
                DogAI.StopCoroutine(bitingCoroutine);
            }

            private IEnumerator BitingRoutine()
            {
                while (IsActive)
                {
                    // Move Closer.
                    float angle = Mathf.Infinity, distance = Mathf.Infinity;
                    while (angle > minBiteAngle || distance > GetTargetDistance(DogAI.Creature, target, biteMaxDistance))
                    {
                        UpdateTarget();
                        angle = Vector3.Angle(DogAI.transform.forward, lookDir);
                        distance = Vector3.Distance(target.transform.position, DogAI.transform.position);
                        yield return null;
                    }

                    // Strike!
                    hasDealtDamage = false;
                    Vector3 head = DogAI.Creature.Animator.Mouths[0].transform.position;
                    Vector3 displacement = Vector3.ProjectOnPlane(target.transform.position - head, DogAI.Creature.transform.up);
                    float d = displacement.magnitude;

                    DogAI.Animator.GetBehaviour<Bite>().OnBiteMouth = OnBiteMouth;
                    DogAI.Animator.GetBehaviour<Bite>().OnBite = OnBite;

                    DogAI.Params.SetTriggerWithValue("Body_Strike", "Body_Strike_Distance", d);

                    // Wait...
                    yield return new WaitForSeconds(biteDelay.Random);
                }
            }

            private void OnBiteMouth(MouthAnimator mouth)
            {
                if (!hasDealtDamage)
                {
                    Collider[] colliders = Physics.OverlapSphere(mouth.transform.position, biteRadius);
                    foreach (Collider collider in colliders)
                    {
                        CreatureBase creature = collider.GetComponent<CreatureBase>();
                        if (creature != null && creature != DogAI.Creature)
                        {
                            creature.Health.TakeDamage(biteDamage.Random, DamageReason.BiteAttack, DogAI.Creature.Constructor.Data.Name);
                            hasDealtDamage = true;
                        }
                    }
                }
            }
            private void OnBite()
            {
                DogAI.Creature.Effects.PlaySound(biteSounds);
            }
        }
        #endregion
    }
}