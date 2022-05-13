// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BirdAI : AnimalAI<BirdAI>
    {
        #region Fields
        [Header("Bird")]
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

        public Transform CurrentPerchPoint
        {
            get;
            set;
        }
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
                        if (Vector3.Distance(creature.transform.position, point.position) < frightDistance)
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

                return points[Random.Range(0, points.Count)];
            }
        }

        protected override string StartState => "PER";
        #endregion

        #region Methods
        public override void Start()
        {
            transform.position = (CurrentPerchPoint = RandomPerchPoint).position;
            base.Start();
        }

        protected override void Initialize()
        {
            States.Add("PER", new Perching(this));
            States.Add("FLY", new Flying(this));
        }
        #endregion

        #region Inner Classes
        public class Perching : Idling
        {
            public Perching(BirdAI sm) : base("Perching", sm) { }

            public override void UpdateLogic()
            {
                base.UpdateLogic();
                if (Vector3.Distance(Player.Instance.Creature.transform.position, StateMachine.transform.position) < StateMachine.FrightDistance)
                {
                    StateMachine.ChangeState("FLY");
                }
            }
        }
        public class Flying : BaseState<BirdAI>
        {
            public Flying(BirdAI sm) : base("Flying", sm) { }

            public override void Enter()
            {
                StateMachine.StartCoroutine(FlyToPerchPointRoutine(StateMachine.RandomPerchPoint));
            }

            private IEnumerator FlyToPerchPointRoutine(Transform perchPoint)
            {
                Quaternion cur = StateMachine.transform.rotation;
                Quaternion tar = Quaternion.LookRotation(Vector3.ProjectOnPlane(perchPoint.position - StateMachine.transform.position, Vector3.up));

                foreach (LegAnimator leg in StateMachine.creature.Animator.Legs)
                {
                    leg.Anchor.SetParent(StateMachine.transform);
                }
                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float progress)
                {
                    StateMachine.transform.rotation = Quaternion.Slerp(cur, tar, progress);
                }, 0.5f);
                foreach (LegAnimator leg in StateMachine.creature.Animator.Legs)
                {
                    leg.Anchor.SetParent(Dynamic.Transform);
                }

                float flightDistance = Vector3.Distance(StateMachine.CurrentPerchPoint.position, perchPoint.position);
                float flightTime = flightDistance / StateMachine.FlightSpeed;

                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float progress)
                {
                    StateMachine.transform.position = Vector3.Lerp(StateMachine.CurrentPerchPoint.position, perchPoint.position, progress) + (Vector3.up * (StateMachine.FlightHeight * StateMachine.FlightPath.Evaluate(progress)));
                }, flightTime);

                StateMachine.CurrentPerchPoint = perchPoint;
                StateMachine.ChangeState("PER");
            }
        }
        #endregion
    }
}