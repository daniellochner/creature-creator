// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SeagullAI : AnimalAI
    {
        #region Fields
        [Header("Seagull")]
        [SerializeField] private Transform perchPoints;
        [SerializeField] private float flightSpeed;
        [SerializeField] private float flightHeight;
        [SerializeField] private AnimationCurve flightPath;
        [SerializeField] private float frightDistance;
        #endregion

        #region Properties
        public Transform PerchPoints => perchPoints;

        public float FlightSpeed => flightSpeed;
        public float FlightHeight => flightHeight;
        public AnimationCurve FlightPath => flightPath;
        public float FrightDistance => frightDistance;

        public Transform CurrentPerchPoint { get; set; }
        #endregion

        #region Methods
        public override void Start()
        {
            base.Start();
            transform.position = (CurrentPerchPoint = perchPoints.GetChild(Random.Range(0, perchPoints.childCount))).position;
        }

        protected override void InitializeStates()
        {
            States = new Dictionary<string, BaseState>()
            {
                { "IDL", new Idling(this) },
                { "FLY", new Flying(this) }
            };
            ChangeState("IDL");
        }
        #endregion

        #region States
        public class Idling : BaseState
        {
            private SeagullAI seagullAI;

            public Idling(SeagullAI s) : base("Idling", s) { seagullAI = s; }

            public override void UpdateLogic()
            {
                if (Vector3.Distance(Player.Instance.Creature.transform.position, StateMachine.transform.position) < seagullAI.FrightDistance)
                {
                    seagullAI.ChangeState("FLY");
                }
            }
        }
        public class Flying : BaseState
        {
            private SeagullAI seagullAI;

            public Flying(SeagullAI s) : base("Flying", s) { seagullAI = s; }

            public override void Enter()
            {
                FlyToRandomPerchPoint();
            }

            private void FlyToRandomPerchPoint()
            {
                List<Transform> points = new List<Transform>();
                foreach (Transform point in seagullAI.PerchPoints)
                {
                    bool isFarEnough = true;
                    foreach (CreatureBase creature in FindObjectsOfType<CreatureBase>())
                    {
                        if (Vector3.Distance(creature.transform.position, point.position) < seagullAI.FrightDistance)
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

                Transform perchPoint = points[Random.Range(0, points.Count)];
                seagullAI.StartCoroutine(FlyToPerchPointRoutine(perchPoint));
            }
            private IEnumerator FlyToPerchPointRoutine(Transform perchPoint)
            {
                float flightDistance = Vector3.Distance(seagullAI.CurrentPerchPoint.position, perchPoint.position);
                float flightTime = flightDistance / seagullAI.FlightSpeed;

                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float progress)
                {
                    seagullAI.transform.position = Vector3.Lerp(seagullAI.CurrentPerchPoint.position, perchPoint.position, progress) + (Vector3.up * (seagullAI.FlightHeight * seagullAI.FlightPath.Evaluate(progress)));
                },
                flightTime);

                seagullAI.CurrentPerchPoint = perchPoint;
                seagullAI.ChangeState("IDL");
            }
        }
        #endregion
    }
}