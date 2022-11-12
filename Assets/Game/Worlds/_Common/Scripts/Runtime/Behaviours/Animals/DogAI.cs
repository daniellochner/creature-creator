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
                trackRegion.OnTrack += delegate (Collider col1, Collider col2)
                {
                    CreatureBase creature = col2.GetComponent<CreatureBase>();

                    List<Holdable> held = new List<Holdable>(creature.Holder.Held.Values);
                    dogBone = held.Find(x => x is DogBone) as DogBone;
                    if (dogBone != null)
                    {
                        ChangeState("PAN");
                    }
                    else
                    {
                        if (HasWeapon(creature) || trackRegion.tracked.Count >= 3)
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
                if (currentStateId == "BAR")
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

                Vector3 lookDir = Vector3.ProjectOnPlane(targetBone.position - StateMachine.transform.position, StateMachine.transform.up).normalized;
                Vector3 offset = lookDir * DogAI.Creature.Constructor.Dimensions.radius;
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
                DogAI.Params.SetBool("Eye_IsGlaring", true);
                DogAI.Creature.Effects.PlaySound(growlSounds);
                yield return new WaitForSeconds(growlTime);
                DogAI.Params.SetBool("Eye_IsGlaring", false);

                while (IsActive)
                {
                    yield return DogAI.StartCoroutine(BarkRoutine());
                    yield return new WaitForSeconds(barkDelayBetween.Random);

                    UpdateTarget();
                }
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
            [SerializeField] private PlayerEffects.Sound[] whimperSounds;

            public DogAI DogAI => StateMachine as DogAI;

            public override void Enter()
            {
                DogAI.Agent.SetDestination(doghouse.transform.position);
                DogAI.Creature.Effects.PlaySound(whimperSounds);
            }
            public override void UpdateLogic()
            {
                if (!DogAI.IsMovingToPosition)
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
                DogAI.Agent.SetDestination(DogAI.Creature.transform.position);
            }
        }

        [Serializable]
        public class Biting : Targeting
        {
            [SerializeField] private float biteOffset;
            [SerializeField] private float minBiteAngle;
            [SerializeField] private MinMax biteDelay;
            [SerializeField] private float biteRadius;
            [SerializeField] private MinMax biteDamage;
            [SerializeField] private PlayerEffects.Sound[] biteSounds;
            private Coroutine bitingCoroutine;
            private bool hasDealtDamage;

            public DogAI DogAI => StateMachine as DogAI;

            private float TargetDistance => DogAI.Creature.Constructor.Dimensions.radius + target.Constructor.Dimensions.radius;
            
            public override void Enter()
            {
                base.Enter();
                bitingCoroutine = DogAI.StartCoroutine(BitingRoutine());
                DogAI.Animator.GetBehaviour<Bite>().OnBite += OnBite;
            }
            public override void UpdateLogic()
            {
                if (!DogAI.IsAnimationState("Strike"))
                {
                    UpdateLookDir();

                    Vector3 offset = lookDir * TargetDistance;
                    DogAI.Agent.SetDestination(target.transform.position - offset);

                    HandleLookAt();
                }
            }
            public override void Exit()
            {
                base.Exit();
                DogAI.StopCoroutine(bitingCoroutine);
                DogAI.Animator.GetBehaviour<Bite>().OnBite -= OnBite;
            }

            private IEnumerator BitingRoutine()
            {
                while (IsActive)
                {
                    // Move Closer.
                    float angle = Mathf.Infinity, distance = Mathf.Infinity;
                    while (angle > minBiteAngle || distance > (TargetDistance + biteOffset))
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
                    DogAI.Params.SetTriggerWithValue("Body_Strike", "Body_Strike_Distance", d);

                    // Wait...
                    yield return new WaitForSeconds(biteDelay.Random);
                }
            }

            private void OnBite(MouthAnimator mouth)
            {
                DogAI.Creature.Effects.PlaySound(biteSounds);
                if (!hasDealtDamage)
                {
                    Collider[] colliders = Physics.OverlapSphere(mouth.transform.position, biteRadius);
                    foreach (Collider collider in colliders)
                    {
                        CreatureBase creature = collider.GetComponent<CreatureBase>();
                        if (creature != null && creature != DogAI.Creature)
                        {
                            creature.Health.TakeDamage(biteDamage.Random);
                            hasDealtDamage = true;
                        }
                    }
                }
            }
        }
        #endregion
    }
}