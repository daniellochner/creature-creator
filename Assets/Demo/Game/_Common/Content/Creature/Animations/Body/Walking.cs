// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator.Animations
{
    public class Walking : CreatureAnimation
    {
        #region Fields
        private float baseMovementSpeed = 1f;
        private float baseRotationSpeed = 120f;
        private float baseTimeToMove = 0.2f;

        private float bodySmoothing = 2.5f;
        private float maxRoll = 10f;
        private float maxPitch = 30f;
        private float stepHeight = 0.5f;
        private float liftHeight = 0.2f;

        private LegAnimator[] llegs, rlegs;
        private LegAnimator mostExtendedLeg;

        private int numPairs;
        private float[] times;
        private float maxTime;
        private float walkTimeScale = 1f;
        private float timeLeftToMove;

        private Coroutine[] movePairs;
        private Coroutine[][] moveFeet;
        #endregion

        #region Methods
        public override void OnStart(Animator animator)
        {
            numPairs = Creature.Legs.Count / 2;
            movePairs = new Coroutine[numPairs];
            moveFeet = new Coroutine[numPairs][];
            for (int i = 0; i < numPairs; ++i)
            {
                moveFeet[i] = new Coroutine[2];
            }

            llegs = new LegAnimator[numPairs];
            rlegs = new LegAnimator[numPairs];
            int li = 0, ri = 0;
            foreach (LegAnimator leg in Creature.Legs)
            {
                if (Creature.Constructor.Body.W2LSpace(leg.transform.position).x < 0)
                {
                    llegs[li++] = leg;
                }
                else
                {
                    rlegs[ri++] = leg;
                }
            }

            // Determine the pair of legs with the least maneuverability (i.e., the most extended pair of legs)
            float minDistance = Mathf.Infinity;
            foreach (LegAnimator leg in llegs)
            {
                float distance = leg.MaxDistance;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    mostExtendedLeg = leg;
                }
            }

            // Determine the number of times the most extended leg should move with respect to each leg
            int[] frequencies = new int[numPairs];
            int maxFrequency = int.MinValue;
            for (int i = 0; i < numPairs; ++i)
            {
                int frequency = frequencies[i] = Mathf.FloorToInt(llegs[i].MaxDistance / minDistance);
                if (frequency > maxFrequency)
                {
                    maxFrequency = frequency;
                }
            }

            // Determine the time to move for each leg
            maxTime = maxFrequency * baseTimeToMove;
            times = new float[numPairs];
            for (int i = 0; i < times.Length; ++i)
            {
                float timesInMax = maxFrequency / frequencies[i];
                times[i] = maxTime / timesInMax;
            }
            
            liftHeight = Creature.DefaultHeight * 0.25f;
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            HandleLegs();
            HandleBody();
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StopMovingLegs();

            Creature.Constructor.Root.localRotation = Quaternion.identity;
            Creature.Constructor.Root.localPosition = Vector3.zero;
            timeLeftToMove = 0f;
        }

        #region Legs
        private void HandleLegs()
        {
            TimerUtility.OnTimer(ref timeLeftToMove, 2f * maxTime, Time.deltaTime * walkTimeScale, delegate
            {
                for (int i = 0; i < numPairs; ++i)
                {
                    movePairs[i] = Creature.StartCoroutine(MovePairRoutine(i));
                }
            });

            float angularSpeed = Mathf.Abs(Creature.Velocity.Angular.y);
            float t = Mathf.InverseLerp(0f, baseRotationSpeed, angularSpeed);

            walkTimeScale = Mathf.Lerp(1f, 2f, t);
            foreach (LegAnimator leg in Creature.Legs)
            {
                leg.MovementTimeScale = walkTimeScale;
            }
        }
        private IEnumerator MovePairRoutine(int pair)
        {
            float timeToMove = times[pair];
            int cycles = (int)(maxTime / timeToMove);

            moveFeet[pair] = new Coroutine[2];
            for (int i = 0; i < cycles; ++i)
            {
                LegAnimator lleg = llegs[pair];
                LegAnimator rleg = rlegs[pair];

                LegAnimator leg1 = (pair % 2 == 0) ? lleg : rleg;
                LegAnimator leg2 = (pair % 2 == 0) ? rleg : lleg;
                
                Quaternion rot = Creature.Constructor.Body.rotation;
                float liftHeight = Creature.Constructor.transform.W2LSpace(leg1.transform.position).y * 0.4f;

                Vector3 pos1 = GetTargetFootPosition(leg1, timeToMove);
                float t1 = Mathf.Clamp01(Vector3.Distance(leg1.Anchor.position, pos1) / leg1.MaxDistance);                
                Coroutine moveFoot1 = leg1.StartCoroutine(leg1.MoveFootRoutine(pos1, rot, timeToMove, liftHeight * t1));
                moveFeet[pair][0] = moveFoot1;
                yield return moveFoot1;

                Vector3 pos2 = GetTargetFootPosition(leg2, timeToMove);
                float t2 = Mathf.Clamp01(Vector3.Distance(leg2.Anchor.position, pos2) / leg2.MaxDistance);
                Coroutine moveFoot2 = leg2.StartCoroutine(leg2.MoveFootRoutine(pos2, rot, timeToMove, liftHeight * t2));
                moveFeet[pair][1] = moveFoot2;
                yield return moveFoot2;
            }
        }

        private void StopMovingLegs()
        {
            for (int i = 0; i < numPairs; ++i)
            {
                Coroutine movePair = movePairs[i];
                if (movePair != null)
                {
                    Creature.StopCoroutine(movePair);
                }

                Coroutine moveLFoot = moveFeet[i][0];
                if (moveLFoot != null)
                {
                    llegs[i].StopCoroutine(moveLFoot);
                }
                Coroutine moveRFoot = moveFeet[i][1];
                if (moveRFoot != null)
                {
                    rlegs[i].StopCoroutine(moveRFoot);
                }
            }

            foreach (LegAnimator leg in Creature.Legs)
            {
                Vector3 pos = GetTargetFootPosition(leg, 0f);
                Quaternion rot = Creature.Constructor.Body.rotation;
                leg.StartCoroutine(leg.MoveFootRoutine(pos, rot, 0.25f, 0f));
            }
        }
        private Vector3 GetTargetFootPosition(LegAnimator leg, float timeToMove)
        {
            Vector3 localPos = Vector3Utility.RotatePointAroundPivot(leg.DefaultFootPosition, Vector3.zero, Creature.Velocity.Angular * timeToMove);
            Vector3 worldPos = Creature.Constructor.Body.L2WSpace(localPos);
            worldPos += Vector3.ProjectOnPlane(Creature.Velocity.Linear, Creature.transform.up) * timeToMove;
            
            if (Physics.Raycast(worldPos, -Creature.transform.up, out RaycastHit hitInfo/*, Creature.Constructor.MaxHeight*/))
            {
                return hitInfo.point + (Creature.transform.up * (leg.LegConstructor.ConnectedFoot?.Offset ?? 0f));
            }
            else
            {
                return worldPos;
            }
        }
        #endregion

        #region Body
        private void HandleBody()
        {
            HandleBodyPosition();
            HandleBodyRotation();
        }
        private void HandleBodyPosition()
        {
            float avgHeight = 0f, minHeight = Mathf.Infinity;
            foreach (LegAnimator leg in Creature.Legs)
            {
                float height = Creature.Constructor.Body.W2LSpace(leg.LegConstructor.Extremity.position).y;
                if (height < minHeight)
                {
                    minHeight = height;
                }
                avgHeight += height;
            }
            avgHeight /= Creature.Legs.Count;

            float t = Creature.Velocity.Linear.magnitude / baseMovementSpeed;
            float offset = avgHeight - minHeight;
            Vector3 targetPosition = Vector3.Lerp(Vector3.zero, -Vector3.up * offset, t);

            Creature.Constructor.Root.localPosition = Vector3.Lerp(Creature.Constructor.Root.localPosition, targetPosition, Time.deltaTime * bodySmoothing);
        }
        private void HandleBodyRotation()
        {
            Quaternion roll = Quaternion.identity, pitch = Quaternion.identity;

            // Roll
            Vector3 avgRFeetPos = GetAverageFeetPosition(rlegs);
            Vector3 avgLFeetPos = GetAverageFeetPosition(llegs);

            Vector3 rollVector = avgRFeetPos - avgLFeetPos;
            float rollAngle = Mathf.Clamp(Vector3.SignedAngle(Creature.Constructor.Body.right, rollVector, Creature.Constructor.Body.forward), -maxRoll, maxRoll);
            roll = Quaternion.AngleAxis(rollAngle, Vector3.forward);

            // Pitch
            int numPairs = Creature.Legs.Count / 2;
            if (numPairs >= 2)
            {
                Vector3 avgFFeetPos = GetAverageFeetPosition(llegs[0], rlegs[0]);
                Vector3 avgBFeetPos = GetAverageFeetPosition(llegs[numPairs - 1], rlegs[numPairs - 1]);

                Vector3 pitchVector = avgFFeetPos - avgBFeetPos;
                float pitchAngle = Mathf.Clamp(Vector3.SignedAngle(Creature.Constructor.Body.forward, pitchVector, Creature.Constructor.Body.right), -maxPitch, maxPitch);
                pitch = Quaternion.AngleAxis(pitchAngle, Vector3.right);
            }

            float t = Creature.Velocity.Linear.magnitude / baseMovementSpeed;
            Quaternion targetRotation = Quaternion.Lerp(Quaternion.identity, roll * pitch, t);

            Creature.Constructor.Root.localRotation = Quaternion.Slerp(Creature.Constructor.Root.localRotation, targetRotation, Time.deltaTime * bodySmoothing);
        }

        private Vector3 GetAverageFeetPosition(params LegAnimator[] legs)
        {
            Vector3 avgFeetPos = Vector3.zero;
            foreach (LegAnimator leg in legs)
            {
                avgFeetPos += leg.LegConstructor.Extremity.position;
            }
            avgFeetPos /= legs.Length;
            return avgFeetPos;
        }
        #endregion
        #endregion
    }
}