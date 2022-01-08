// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Walking : SceneLinkedSMB<CreatureAnimator>
    {
        #region Fields
        [SerializeField] private float baseMovementSpeed = 0.8f;
        [SerializeField] private float baseRotationSpeed = 180f;
        [SerializeField] private float baseTimeToMove = 0.2f; // shortest legs' time to move
        [SerializeField] private float bodySmoothing = 4f;
        [SerializeField] private float maxRoll = 5f;
        [SerializeField] private float maxPitch = 5f;
        [SerializeField] private float stepHeight = 0.2f;
        [SerializeField] private float liftHeight = 0.2f;
        private float maxHeightOffset = Mathf.Infinity;

        private int numPairs;
        private float walkTimeScale = 1f;
        private LegAnimator mostExtendedLeg;

        private Coroutine[] movePairs;
        private Coroutine[][] moveFeet;

        private float[] times;
        private float maxTimeToMove = Mathf.NegativeInfinity;
        private float timeToMoveLegs;
        #endregion

        #region Methods
        public override void OnStart(Animator animator)
        {
            if (!m_MonoBehaviour.IsAnimated) return;

            LegAnimator[] pairs = m_MonoBehaviour.LLegs;

            // Determine the pair of legs with the least maneuverability (i.e., the most extended pair of legs).
            float minDistance = Mathf.Infinity;
            foreach (LegAnimator leg in pairs)
            {
                float distance = leg.MaxDistance;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    mostExtendedLeg = leg;
                }
            }

            // Determine the number of times the most extended leg should move with respect to each leg.
            int[] frequencies = new int[pairs.Length];
            int maxFrequency = int.MinValue;
            for (int i = 0; i < pairs.Length; ++i)
            {
                int frequency = frequencies[i] = Mathf.FloorToInt(pairs[i].MaxDistance / minDistance);
                if (frequency > maxFrequency)
                {
                    maxFrequency = frequency;
                }
            }

            // Determine the time to move for each leg.
            maxTimeToMove = maxFrequency * baseTimeToMove;
            times = new float[pairs.Length];
            for (int i = 0; i < times.Length; ++i)
            {
                float timesInMax = maxFrequency / frequencies[i];
                times[i] = maxTimeToMove / timesInMax;
            }

            // TODO: recalculate walk params
            numPairs = m_MonoBehaviour.Legs.Length / 2;

            
            maxHeightOffset = Mathf.Sqrt(Mathf.Pow(mostExtendedLeg.MaxLength, 2) - Mathf.Pow(mostExtendedLeg.MaxDistance, 2));

            movePairs = new Coroutine[numPairs];
            moveFeet = new Coroutine[numPairs][];
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
            timeToMoveLegs = 0f;
        }

        #region Legs
        private void HandleLegs()
        {
            if (timeToMoveLegs <= 0)
            {
                for (int i = 0; i < numPairs; ++i)
                {
                    movePairs[i] = m_MonoBehaviour.StartCoroutine(MovePairRoutine(i));
                }
                timeToMoveLegs = 2f * maxTimeToMove;
            }
            else
            {
                timeToMoveLegs -= Time.deltaTime * walkTimeScale;
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
            int cycles = (int)(maxTimeToMove / timeToMove);

            moveFeet[pair] = new Coroutine[2];
            for (int i = 0; i < cycles; ++i)
            {
                LegAnimator lleg = m_MonoBehaviour.LLegs[pair];
                LegAnimator rleg = m_MonoBehaviour.RLegs[pair];

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
                    m_MonoBehaviour.LLegs[i].StopCoroutine(moveLFoot);
                }
                Coroutine moveRFoot = moveFeet[i][1];
                if (moveRFoot != null)
                {
                    m_MonoBehaviour.RLegs[i].StopCoroutine(moveRFoot);
                }
            }

            foreach (LegAnimator leg in m_MonoBehaviour.Legs)
            {
                Vector3 defaultPosition = m_MonoBehaviour.CreatureConstructor.Body.TransformPoint(leg.DefaultFootPosition);
                Vector3 defaultRotation = m_MonoBehaviour.CreatureConstructor.Body.eulerAngles;
                leg.StartCoroutine(leg.MoveFootRoutine(defaultPosition, Quaternion.Euler(defaultRotation), 0.25f, 0f));
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
            avgHeight /= m_MonoBehaviour.Legs.Length;

            float t = m_MonoBehaviour.Velocity.Linear.magnitude / baseMovementSpeed;
            float offset = (avgHeight - minHeight) / 2f;
            Vector3 targetPosition = Vector3.Lerp(Vector3.zero, -Vector3.up * offset, t);

            m_MonoBehaviour.CreatureConstructor.Root.localPosition = Vector3.Lerp(m_MonoBehaviour.CreatureConstructor.Root.localPosition, targetPosition, Time.deltaTime * bodySmoothing);
        }
        private void HandleBodyRotation()
        {
            Quaternion roll = Quaternion.identity, pitch = Quaternion.identity;

            // Roll
            Vector3 avgRFeetPos = GetAverageFeetPosition(m_MonoBehaviour.RLegs);
            Vector3 avgLFeetPos = GetAverageFeetPosition(m_MonoBehaviour.LLegs);

            Vector3 rollVector = avgRFeetPos - avgLFeetPos;
            float rollAngle = Mathf.Clamp(Vector3.SignedAngle(m_MonoBehaviour.CreatureConstructor.Body.right, rollVector, m_MonoBehaviour.CreatureConstructor.Body.forward), -maxRoll, maxRoll);
            roll = Quaternion.AngleAxis(rollAngle, Vector3.forward);

            // Pitch
            int numPairs = m_MonoBehaviour.Legs.Length / 2;
            if (numPairs >= 2)
            {
                Vector3 avgFFeetPos = GetAverageFeetPosition(m_MonoBehaviour.LLegs[0], m_MonoBehaviour.RLegs[0]);
                Vector3 avgBFeetPos = GetAverageFeetPosition(m_MonoBehaviour.LLegs[numPairs - 1], m_MonoBehaviour.RLegs[numPairs - 1]);

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