// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BirdAI : AnimalAI
    {
        #region Methods
        public void Frighten(Collider col)
        {
            CreatureBase other = col.GetComponent<CreatureBase>();
            if (other != null && other != creature)
            {
                ChangeState("FLY");
            }
        }
        #endregion

        #region States
        public override void Reset()
        {
            base.Reset();
            states.Add(new Flying(this));
        }

        [Serializable]
        public class Flying : BaseState
        {
            [SerializeField] private Transform perchPoints;
            [SerializeField] private float flightSpeed;
            [SerializeField] private float flightHeight;
            [SerializeField] private AnimationCurve flightPath;
            [SerializeField] private float minDistanceFromPlayer;

            public BirdAI BirdAI => StateMachine as BirdAI;

            public Flying(BirdAI birdAI) : base(birdAI) { }

            public Transform RandomPerchPoint
            {
                get
                {
                    List<Transform> points = new List<Transform>();
                    foreach (Transform point in perchPoints)
                    {
                        bool isFarEnough = true;
                        foreach (CreatureBase creature in FindObjectsOfType<CreatureBase>())
                        {
                            if (Vector3.Distance(creature.transform.position, point.position) < minDistanceFromPlayer)
                            {
                                isFarEnough = false;
                                break;
                            }
                        }
                        if (isFarEnough)
                        {
                            points.Add(point);
                        }
                    }

                    return points[UnityEngine.Random.Range(0, points.Count)];
                }
            }

            public override void Enter()
            {
                BirdAI.StartCoroutine(FlyToPositionRoutine(RandomPerchPoint.position));
            }

            private IEnumerator FlyToPositionRoutine(Vector3 to)
            {
                Quaternion cur = BirdAI.transform.rotation;
                Quaternion tar = Quaternion.LookRotation(Vector3.ProjectOnPlane(to - BirdAI.transform.position, Vector3.up));

                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float progress)
                {
                    BirdAI.transform.rotation = Quaternion.Slerp(cur, tar, progress);
                }, 1f);

                Vector3 from = BirdAI.transform.position;
                float flightDistance = Vector3.Distance(from, to);
                float flightTime = flightDistance / flightSpeed;

                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float progress)
                {
                    BirdAI.transform.position = Vector3.Lerp(from, to, progress) + (Vector3.up * (flightHeight * flightPath.Evaluate(progress)));
                }, flightTime);

                BirdAI.ChangeState("IDL");
            }
        }
        #endregion
    }
}