// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AnimalAI<T> : StateMachine<T> where T : StateMachine<T>
    {
        #region Fields
        [Header("Animal")]
        [SerializeField] private TextAsset data;
        [SerializeField] private AmbientNoisesInfo ambientNoises;

        protected CreatureSource creature;
        #endregion

        #region Methods
        public override void Awake()
        {
            base.Awake();
            creature = GetComponentInParent<CreatureSource>();
        }
        public virtual void Start()
        {
            creature.Setup();
            creature.Constructor.Construct(JsonUtility.FromJson<CreatureData>(data.text));
            creature.Animator.IsAnimated = true;
        }
        #endregion

        #region Inner Classes
        public class Idling<S> : BaseState<S> where S : AnimalAI<S>
        {
            private float timeLeft;

            public Idling(S sm) : base("Idling", sm)
            {
                timeLeft = StateMachine.ambientNoises.cooldown.Random;
            }

            public override void UpdateLogic()
            {
                if (StateMachine.ambientNoises.noises.Length > 0)
                {
                    TimerUtility.OnTimer(ref timeLeft, StateMachine.ambientNoises.cooldown.Random, Time.deltaTime, MakeRandomAmbientNoise);
                }
            }

            private void MakeRandomAmbientNoise()
            {
                StateMachine.creature.Effector.PlaySound(StateMachine.ambientNoises.noises[UnityEngine.Random.Range(0, StateMachine.ambientNoises.noises.Length)], StateMachine.ambientNoises.pitch.Random);
            }
        }

        [Serializable]
        public class AmbientNoisesInfo
        {
            public string[] noises;
            public MinMax pitch;
            public MinMax cooldown;
        }
        #endregion
    }
}