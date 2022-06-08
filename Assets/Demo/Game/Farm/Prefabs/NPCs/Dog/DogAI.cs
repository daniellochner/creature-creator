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
        [SerializeField] protected TrackRegion attackRegion;
        #endregion

        #region Methods
        public override void Start()
        {
            base.Start();

            attackRegion.OnTrack += delegate
            {
                if (currentStateId != "BIT")
                {
                    ChangeState("BAR");
                }
            };
            attackRegion.OnLoseTrackOf += delegate
            {
                if (attackRegion.tracked.Count == 0)
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
        #endregion

        #region Inner Classes
        [Serializable]
        public class Barking : BaseState
        {
            [SerializeField] private float growlTime;
            [SerializeField] private string[] growlSounds;
            [Space]
            [SerializeField] private MinMax barkDelayBetween;
            [SerializeField] private MinMax barkDelayWithin;
            [SerializeField] private MinMaxInt barkCount;
            [SerializeField] private string[] barkSounds;

            private Coroutine barkingCoroutine;

            public DogAI DogAI => StateMachine as DogAI;

            public override void Enter()
            {
                DogAI.creature.Animator.Animator.SetTrigger("Look");
                barkingCoroutine = DogAI.StartCoroutine(BarkingRoutine());
                TargetNearest();
            }
            public override void UpdateLogic()
            {

            }
            public override void Exit()
            {
                DogAI.StopCoroutine(barkingCoroutine);
                DogAI.creature.Animator.LookTarget = DogAI.creature.Animator.InteractTarget = null;
            }

            private IEnumerator BarkingRoutine()
            {
                DogAI.creature.Effector.PlaySound(growlSounds, 0.5f);
                yield return new WaitForSeconds(growlTime);

                while (true)
                {
                    yield return DogAI.StartCoroutine(BarkRoutine());
                    yield return new WaitForSeconds(barkDelayBetween.Random);

                    TargetNearest();
                }
            }
            private IEnumerator BarkRoutine()
            {
                int barks = barkCount.Random;
                for (int i = 0; i < barks; i++)
                {
                    Bark();
                    yield return new WaitForSeconds(barkDelayWithin.Random);
                }
            }

            private void TargetNearest()
            {
                DogAI.creature.Animator.LookTarget = DogAI.attackRegion.Nearest.transform;
            }
            private void Bark()
            {
                DogAI.creature.Effector.PlaySound(barkSounds, 0.5f);
                DogAI.creature.Animator.Animator.SetTrigger("Bark");
            }
        }

        [Serializable]
        public class Guarding : BaseState
        {

        }

        [Serializable]
        public class Scurrying : BaseState
        {
            [SerializeField] private Transform doghouse;
            [SerializeField] private string[] whimperSounds;

            public DogAI DogAI => StateMachine as DogAI;

            public override void Enter()
            {
                DogAI.agent.SetDestination(doghouse.position);
                DogAI.creature.Effector.PlaySound(whimperSounds);
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
                DogAI.creature.Hider.Hide();
            }
            public override void UpdateLogic()
            {
                //TimerUtility.OnTimer(ref hideTimeLeft, hideTime.Random, )
            }
            public override void Exit()
            {
                DogAI.creature.Hider.Show();
                DogAI.agent.SetDestination(DogAI.creature.transform.position);
            }
        }

        [Serializable]
        public class Attacking : BaseState
        {

        }

        [Serializable]
        public class Biting : BaseState
        {
            [SerializeField] private MinMax biteDelay;
            private float biteTimeLeft;

            private Transform target;

            public DogAI DogAI => StateMachine as DogAI;

            public Biting(DogAI dogAI) : base(dogAI) { }

            public override void UpdateLogic()
            {
                TimerUtility.OnTimer(ref biteTimeLeft, biteDelay.Random, Time.deltaTime, Bite);

                Vector3 offset = (target.position - DogAI.transform.position).normalized * DogAI.creature.Constructor.Dimensions.radius;
                DogAI.agent.SetDestination(target.position - offset);
            }
            private void Bite()
            {
                DogAI.agent.updatePosition = false;
                DogAI.creature.Animator.Animator.SetTrigger("Bite");
            }

            private IEnumerator BitingRoutine()
            {
                while (true)
                {
                    while (DogAI.IsMovingToPosition)
                    {

                    }


                }
            }
        }
        #endregion
    }
}