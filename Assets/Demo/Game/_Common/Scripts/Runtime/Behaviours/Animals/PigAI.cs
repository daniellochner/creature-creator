// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class PigAI : AnimalAI
    {
        #region Methods
        [ContextMenu("Debug/Roll")]
        public void Roll()
        {
            ChangeState("ROL");
        }
        #endregion

        #region Nested
        [Serializable]
        public class Rolling : BaseState
        {
            [SerializeField] private float rollTime;
            [SerializeField] private CreatureEffector.Sound[] squealSounds;
            private float rollTimeLeft;

            private PigAI PigAI => StateMachine as PigAI;

            public override void Enter()
            {
                PigAI.Creature.Effector.PlaySound(squealSounds);
                PigAI.Animator.SetTrigger("Body_Roll");
            }
            public override void UpdateLogic()
            {
                TimerUtility.OnTimer(ref rollTimeLeft, rollTime, Time.deltaTime, delegate
                {
                    PigAI.ChangeState("WAN");
                });
            }
        }
        #endregion
    }
}