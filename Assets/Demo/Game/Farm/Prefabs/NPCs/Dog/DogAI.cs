// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DogAI : AnimalAI
    {
        public void Bark(Collider col)
        {
            ChangeState("BAR");
        }

        #region States
        public override void Reset()
        {
            base.Reset();
            states.Add(new Barking(this));
            states.Add(new Scurrying(this));
            states.Add(new Hiding(this));
            states.Add(new Biting(this));
        }

        [Serializable]
        public class Barking : BaseState
        {
            [SerializeField] private MinMax barkCooldown;
            [SerializeField] private MinMaxInt barkCount;
            [SerializeField] private string[] barkSounds;
            [SerializeField] private MinMax barkDelay;

            private float barkTimeLeft;

            public DogAI DogAI => StateMachine as DogAI;

            public Barking(DogAI dogAI) : base(dogAI) { }

            public override void UpdateLogic()
            {
                TimerUtility.OnTimer(ref barkTimeLeft, barkCooldown.Random, Time.deltaTime, Bark);
            }

            private void Bark()
            {
                DogAI.StartCoroutine(BarkRoutine());
            }
            private IEnumerator BarkRoutine()
            {
                int barks = barkCount.Random;
                for (int i = 0; i < barks; i++)
                {
                    DogAI.creature.Effector.PlaySound(barkSounds, 0.5f);
                    yield return new WaitForSeconds(barkDelay.Random);
                }
            }
        }

        [Serializable]
        public class Scurrying : BaseState
        {
            [SerializeField] private Transform doghouse;

            public DogAI DogAI => StateMachine as DogAI;

            public Scurrying(DogAI dogAI) : base(dogAI) { }

            public override void Enter()
            {
                DogAI.agent.SetDestination(doghouse.position);
            }
            public override void UpdateLogic()
            {
                if (!DogAI.IsMovingToPosition)
                {

                }
            }
        }

        [Serializable]
        public class Hiding : BaseState
        {
            private float hideTimeLeft;

            public DogAI DogAI => StateMachine as DogAI;

            public Hiding(DogAI dogAI) : base(dogAI) { }

            public override void UpdateLogic()
            {

            }
            public override void Exit()
            {
                DogAI.agent.SetDestination(DogAI.creature.transform.position);
            }
        }

        [Serializable]
        public class Biting : BaseState
        {
            public DogAI DogAI => StateMachine as DogAI;

            public Biting(DogAI dogAI) : base(dogAI) { }

            public override void UpdateLogic()
            {
            }
        }
        #endregion
    }
}