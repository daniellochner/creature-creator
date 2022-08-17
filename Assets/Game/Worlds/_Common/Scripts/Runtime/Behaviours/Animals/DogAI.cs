// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using DanielLochner.Assets.CreatureCreator.Animations;
using System;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DogAI : AnimalAI
    {
        #region Fields
        [SerializeField] protected TrackRegion trackRegion;
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
                    if (HasWeapon(creature) || trackRegion.tracked.Count >= 3)
                    {
                        ChangeState("SCU");
                    }
                    else if (currentStateId == "WAN")
                    {
                        ChangeState("BAR");
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
                DogAI.Creature.Hider.Hide();
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
                DogAI.Creature.Hider.Show();
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
                bitingCoroutine = DogAI.StartCoroutine(BitingRoutine());
                DogAI.Animator.GetBehaviour<Bite>().OnBite += OnBite;
            }
            public override void UpdateLogic()
            {
                if (!DogAI.Animator.GetCurrentAnimatorStateInfo(0).IsName("Strike"))
                {
                    UpdateLookDir();

                    Vector3 offset = lookDir * TargetDistance;
                    DogAI.Agent.SetDestination(target.transform.position - offset);

                    if (!DogAI.IsMovingToPosition)
                    {
                        HandleLookAt();
                    }
                }
            }
            public override void Exit()
            {
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
                    float d = Vector3.Distance(target.transform.position, DogAI.transform.position);
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