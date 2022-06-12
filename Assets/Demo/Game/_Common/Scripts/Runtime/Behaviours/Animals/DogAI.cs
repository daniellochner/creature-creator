// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DogAI : AnimalAI
    {
        #region Fields
        [SerializeField] protected TrackRegion trackRegion;
        [SerializeField] private float lookAtSmoothing;
        protected CreatureBase target;
        protected Vector3 lookDir;
        #endregion

        #region Methods
        public override void Start()
        {
            base.Start();

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

        public void Attack(Collider col)
        {
            if (currentStateId == "BAR")
            {
                ChangeState("BIT");
            }
        }

        private void UpdateTarget()
        {
            Transform nearest = Creature.Animator.InteractTarget = trackRegion.Nearest.transform;
            if (target == null || target.transform != nearest)
            {
                target = nearest.GetComponent<CreatureBase>();
            }
        }
        private void HandleLookAt()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), lookAtSmoothing * Time.deltaTime);
        }
        private void UpdateLookDir()
        {
            lookDir = Vector3.ProjectOnPlane(target.transform.position - transform.position, transform.up).normalized;
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
        public class Barking : BaseState
        {
            [SerializeField] private float growlTime;
            [SerializeField] private CreatureEffector.Sound[] growlSounds;
            [Space]
            [SerializeField] private MinMax barkDelayBetween;
            [SerializeField] private MinMax barkDelayWithin;
            [SerializeField] private MinMaxInt barkCount;
            [SerializeField] private CreatureEffector.Sound[] barkSounds;
            private Coroutine barkingCoroutine;

            public DogAI DogAI => StateMachine as DogAI;

            public override void Enter()
            {
                DogAI.UpdateTarget();
                barkingCoroutine = DogAI.StartCoroutine(BarkingRoutine());
            }
            public override void UpdateLogic()
            {
                DogAI.UpdateLookDir();
                DogAI.HandleLookAt();
            }
            public override void Exit()
            {
                DogAI.StopCoroutine(barkingCoroutine);
            }

            private IEnumerator BarkingRoutine()
            {
                DogAI.Creature.Effector.PlaySound(growlSounds);
                yield return new WaitForSeconds(growlTime);

                while (IsActive)
                {
                    yield return DogAI.StartCoroutine(BarkRoutine());
                    yield return new WaitForSeconds(barkDelayBetween.Random);

                    DogAI.UpdateTarget();
                }
            }
            private IEnumerator BarkRoutine()
            {
                int barks = barkCount.Random;
                for (int i = 0; i < barks; i++)
                {
                    DogAI.Creature.Effector.PlaySound(barkSounds);
                    DogAI.Animator.SetTrigger("Bark");
                    yield return new WaitForSeconds(barkDelayWithin.Random);
                }
            }
        }

        [Serializable]
        public class Scurrying : BaseState
        {
            [SerializeField] public GameObject doghouse;
            [SerializeField] private CreatureEffector.Sound[] whimperSounds;

            public DogAI DogAI => StateMachine as DogAI;

            public override void Enter()
            {
                DogAI.Agent.SetDestination(doghouse.transform.position);
                DogAI.Creature.Effector.PlaySound(whimperSounds);
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
        public class Biting : BaseState
        {
            [SerializeField] private float biteOffset;
            [SerializeField] private float minBiteAngle;
            [SerializeField] private MinMax biteDelay;
            private Coroutine bitingCoroutine;

            public DogAI DogAI => StateMachine as DogAI;

            private float TargetDistance => DogAI.Creature.Constructor.Dimensions.radius + DogAI.target.Constructor.Dimensions.radius;

            public override void Enter()
            {
                bitingCoroutine = DogAI.StartCoroutine(BitingRoutine());
            }
            public override void UpdateLogic()
            {
                if (!DogAI.Animator.GetCurrentAnimatorStateInfo(0).IsName("Strike"))
                {
                    DogAI.UpdateLookDir();

                    Vector3 offset = DogAI.lookDir * TargetDistance;
                    DogAI.Agent.SetDestination(DogAI.target.transform.position - offset);

                    if (!DogAI.IsMovingToPosition)
                    {
                        DogAI.HandleLookAt();
                    }
                }
            }
            public override void Exit()
            {
                DogAI.StopCoroutine(bitingCoroutine);
            }

            private IEnumerator BitingRoutine()
            {
                while (IsActive)
                {
                    // Move Closer.
                    float angle = Mathf.Infinity, distance = Mathf.Infinity;
                    while (angle > minBiteAngle || distance > (TargetDistance + biteOffset))
                    {
                        DogAI.UpdateTarget();
                        angle = Vector3.Angle(DogAI.transform.forward, DogAI.lookDir);
                        distance = Vector3.Distance(DogAI.target.transform.position, DogAI.transform.position);
                        yield return null;
                    }

                    // Strike (and Bite)!
                    DogAI.Animator.SetTrigger("Strike");

                    // Wait...
                    yield return new WaitForSeconds(biteDelay.Random);
                }
            }
        }
        #endregion
    }
}