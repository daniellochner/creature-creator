// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SnakeAI : AnimalAI
    {
        [Header("Snake")]
        [SerializeField] private GameObject mouthCol;

        public MouthAnimator Mouth { get; set; }

        public override void Start()
        {
            base.Start();

            //creature.Tracker.OnTrack += delegate (CreatureBase other)
            //{
            //    ChangeState("STR");
            //};
            //creature.Tracker.OnLoseTrackOf += delegate (CreatureBase other)
            //{
            //    Debug.Log("TEST");
            //    if (creature.Tracker.Tracked.Count == 0)
            //    {
            //        ChangeState("WAN");
            //    }
            //};


            Mouth = GetComponentInChildren<MouthAnimator>();
            Instantiate(mouthCol, Mouth.transform, false);
        }
        

        #region States
        public override void Reset()
        {
            base.Reset();
            states.Add(new Striking(this));
        }


        [Serializable]
        public class Striking : BaseState
        {
            [SerializeField] private string[] strikeNoises;
            [SerializeField] private MinMax strikeCooldown;
            [SerializeField] private float maxStrikeDistance;
            private float strikeTimeLeft;
            [SerializeField] private float rotSmoothing;

            private CreatureBase targetedCreature;

            public SnakeAI SnakeAI => StateMachine as SnakeAI;

            public Striking(SnakeAI snakeAI) : base(snakeAI) { }


            public override void Enter()
            {
                SnakeAI.agent.SetDestination(SnakeAI.transform.position);
            }
            public override void UpdateLogic()
            {
                LookAt();
                TimerUtility.OnTimer(ref strikeTimeLeft, strikeCooldown.Random, Time.deltaTime, Strike);
            }
            
            private void LookAt()
            {
                targetedCreature = null;
                float minDistance = Mathf.Infinity;
                //foreach (CreatureBase creature in SnakeAI.creature.Tracker.Tracked)
                //{
                //    float distance = Vector3.Distance(creature.transform.position, SnakeAI.transform.position);
                //    if (distance < minDistance)
                //    {
                //        targetedCreature = creature;
                //        minDistance = distance;
                //    }
                //}

                if (targetedCreature != null)
                {
                    Quaternion look = Quaternion.LookRotation(targetedCreature.transform.position - SnakeAI.transform.position);
                    SnakeAI.transform.rotation = Quaternion.Slerp(SnakeAI.transform.rotation, look, rotSmoothing * Time.deltaTime);
                }
            }
            


            private void Strike()
            {
                float distance = Vector3.Distance(targetedCreature.transform.position, SnakeAI.transform.position);
                //if (distance < maxStrikeDistance)
                {
                    SnakeAI.creature.Animator.InteractTarget = targetedCreature.transform;

                    SnakeAI.creature.Animator.Animator.SetTrigger("Strike");
                    SnakeAI.creature.Effector.PlaySound(strikeNoises);



                }
            }
        }
        #endregion
    }
}