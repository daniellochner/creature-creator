// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Walking : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        private float baseMovementSpeed = 0.8f;
        private float baseRotationSpeed = 120f;
        private float baseTimeToMove = 0.2f;

        private float bodySmoothing = 2.5f;
        private float maxRoll = 10f;
        private float maxPitch = 10f;
        private float stepHeight = 0.2f;
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
            if (!m_MonoBehaviour.IsAnimated) return;

            numPairs = m_MonoBehaviour.Legs.Count / 2;
            movePairs = new Coroutine[numPairs];
            moveFeet = new Coroutine[numPairs][];

            llegs = new LegAnimator[numPairs];
            rlegs = new LegAnimator[numPairs];
            int li = 0, ri = 0;
            foreach (LegAnimator leg in m_MonoBehaviour.Legs)
            {
                if (m_MonoBehaviour.CreatureConstructor.Body.W2LSpace(leg.transform.position).x < 0)
                {
                    llegs[li++] = leg;
                }
                else
                {
                    rlegs[ri++] = leg;
                }
            }

            // Determines the pair of legs with the least maneuverability (i.e., the most extended pair of legs).
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

            // Determines the number of times the most extended leg should move with respect to each leg.
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

            // Determines the time to move for each leg.
            maxTime = maxFrequency * baseTimeToMove;
            times = new float[numPairs];
            for (int i = 0; i < times.Length; ++i)
            {
                float timesInMax = maxFrequency / frequencies[i];
                times[i] = maxTime / timesInMax;
            }
            
            liftHeight = m_MonoBehaviour.DefaultHeight * 0.25f;
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            HandleLegs();
            HandleBody();
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StopMovingLegs();

            m_MonoBehaviour.CreatureConstructor.Root.localRotation = Quaternion.identity;
            m_MonoBehaviour.CreatureConstructor.Root.localPosition = Vector3.zero;
            timeLeftToMove = 0f;
        }

        #region Legs
        private void HandleLegs()
        {
            if (timeLeftToMove <= 0)
            {
                for (int i = 0; i < numPairs; ++i)
                {
                    movePairs[i] = m_MonoBehaviour.StartCoroutine(MovePairRoutine(i));
                }
                timeLeftToMove = 2f * maxTime;
            }
            else
            {
                timeLeftToMove -= Time.deltaTime * walkTimeScale;
            }

            float angularSpeed = Mathf.Abs(m_MonoBehaviour.Velocity.Angular.y);
            float t = Mathf.InverseLerp(0f, baseRotationSpeed, angularSpeed);

            walkTimeScale = Mathf.Lerp(1f, 2f, t);
            foreach (LegAnimator leg in m_MonoBehaviour.Legs)
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
                
                Quaternion rot = m_MonoBehaviour.CreatureConstructor.Body.rotation;

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
                    m_MonoBehaviour.StopCoroutine(movePair);
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

            foreach (LegAnimator leg in m_MonoBehaviour.Legs)
            {
                Vector3 pos = GetTargetFootPosition(leg, 0f);
                Quaternion rot = m_MonoBehaviour.CreatureConstructor.Body.rotation;
                leg.StartCoroutine(leg.MoveFootRoutine(pos, rot, 0.25f, 0f));
            }
        }
        private Vector3 GetTargetFootPosition(LegAnimator leg, float timeToMove)
        {
            Vector3 localPos = Vector3Utility.RotatePointAroundPivot(leg.DefaultFootPosition, Vector3.zero, m_MonoBehaviour.Velocity.Angular * timeToMove);
            Vector3 worldPos = Vector3.ProjectOnPlane(m_MonoBehaviour.CreatureConstructor.Body.TransformPoint(localPos), m_MonoBehaviour.transform.up);
            worldPos += m_MonoBehaviour.Velocity.Linear * timeToMove;
            
            Vector3 origin = worldPos + m_MonoBehaviour.transform.up * stepHeight;
            Vector3 dir = -m_MonoBehaviour.transform.up;

            if (Physics.Raycast(origin, dir, out RaycastHit hitInfo, 2f * stepHeight, LayerMask.GetMask("Ground")))
            {
                return hitInfo.point + (m_MonoBehaviour.transform.up * leg.LegConstructor.ConnectedFoot.Offset);
            }
            else
            {
                return leg.Anchor.position;
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
            foreach (LegAnimator leg in m_MonoBehaviour.Legs)
            {
                float height = m_MonoBehaviour.CreatureConstructor.Body.W2LSpace(leg.LegConstructor.Extremity.position).y;
                if (height < minHeight)
                {
                    minHeight = height;
                }
                avgHeight += height;
            }
            avgHeight /= m_MonoBehaviour.Legs.Count;

            float t = m_MonoBehaviour.Velocity.Linear.magnitude / baseMovementSpeed;
            float offset = (avgHeight - minHeight) / 2f;
            Vector3 targetPosition = Vector3.Lerp(Vector3.zero, -Vector3.up * offset, t);

            m_MonoBehaviour.CreatureConstructor.Root.localPosition = Vector3.Lerp(m_MonoBehaviour.CreatureConstructor.Root.localPosition, targetPosition, Time.deltaTime * bodySmoothing);
        }
        private void HandleBodyRotation()
        {
            Quaternion roll = Quaternion.identity, pitch = Quaternion.identity;

            // Roll
            Vector3 avgRFeetPos = GetAverageFeetPosition(rlegs);
            Vector3 avgLFeetPos = GetAverageFeetPosition(llegs);

            Vector3 rollVector = avgRFeetPos - avgLFeetPos;
            float rollAngle = Mathf.Clamp(Vector3.SignedAngle(m_MonoBehaviour.CreatureConstructor.Body.right, rollVector, m_MonoBehaviour.CreatureConstructor.Body.forward), -maxRoll, maxRoll);
            roll = Quaternion.AngleAxis(rollAngle, Vector3.forward);

            // Pitch
            int numPairs = m_MonoBehaviour.Legs.Count / 2;
            if (numPairs >= 2)
            {
                Vector3 avgFFeetPos = GetAverageFeetPosition(llegs[0], rlegs[0]);
                Vector3 avgBFeetPos = GetAverageFeetPosition(llegs[numPairs - 1], rlegs[numPairs - 1]);

                Vector3 pitchVector = avgFFeetPos - avgBFeetPos;
                float pitchAngle = Mathf.Clamp(Vector3.SignedAngle(m_MonoBehaviour.CreatureConstructor.Body.forward, pitchVector, m_MonoBehaviour.CreatureConstructor.Body.right), -maxPitch, maxPitch);
                pitch = Quaternion.AngleAxis(pitchAngle, Vector3.right);
            }

            float t = m_MonoBehaviour.Velocity.Linear.magnitude / baseMovementSpeed;
            Quaternion targetRotation = Quaternion.Lerp(Quaternion.identity, roll * pitch, t);

            m_MonoBehaviour.CreatureConstructor.Root.localRotation = Quaternion.Slerp(m_MonoBehaviour.CreatureConstructor.Root.localRotation, targetRotation, Time.deltaTime * bodySmoothing);
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