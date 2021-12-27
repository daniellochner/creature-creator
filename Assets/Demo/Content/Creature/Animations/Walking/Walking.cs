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
        [SerializeField] private float distanceThreshold = 0.3f;
        [SerializeField] private float bodySmoothing = 4f;

        [SerializeField] private float movementSpeed = 0.8f;

        [SerializeField] private MinMax minMaxRoll = new MinMax(-20f, 20f);
        [SerializeField] private MinMax minMaxPitch = new MinMax(-10f, 10f);
        #endregion

        #region Methods
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            HandleLegs();
            HandleBody();
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.CreatureConstructor.Root.localRotation = Quaternion.identity;
            m_MonoBehaviour.CreatureConstructor.Root.localPosition = Vector3.zero;
        }

        #region Legs
        private void HandleLegs()
        {
            LegAnimator flLeg = m_MonoBehaviour.LLegs[0];
            LegAnimator frLeg = m_MonoBehaviour.RLegs[0];

            LegAnimator[] leadingLegs = null;
            if (!flLeg.IsMovingToTarget && !frLeg.IsMovingToTarget)
            {
                if (IsTooFar(flLeg))
                {
                    leadingLegs = m_MonoBehaviour.LLegs;
                }
                else
                if (IsTooFar(frLeg))
                {
                    leadingLegs = m_MonoBehaviour.RLegs;
                }
            }

            if (leadingLegs != null)
            {
                int numLegs = m_MonoBehaviour.Legs.Length / 2;
                for (int i = 0; i < numLegs; ++i)
                {
                    LegAnimator leg1 = leadingLegs[i];
                    LegAnimator leg2 = leg1.FlippedLeg;
                    float delay = (i % 2 == 0) ? 0f : leg1.TimeToMove;

                    m_MonoBehaviour.StartCoroutine(MovePairRoutine(leg1, leg2, delay));
                }
            }
        }

        private IEnumerator MovePairRoutine(LegAnimator leg1, LegAnimator leg2, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            Quaternion rot = m_MonoBehaviour.CreatureConstructor.Body.rotation;

            Vector3 pos1 = GetTargetFootPosition(leg1);
            yield return leg1.StartCoroutine(leg1.MoveToTargetRoutine(pos1, rot));

            Vector3 pos2 = GetTargetFootPosition(leg2);
            yield return leg2.StartCoroutine(leg2.MoveToTargetRoutine(pos2, rot));
        }
        private bool IsTooFar(LegAnimator leg)
        {
            return Vector3.Distance(leg.Anchor.position, GetTargetFootPosition(leg)) > 0.2f;
        }
        private Vector3 GetTargetFootPosition(LegAnimator leg)
        {
            Vector3 extremityPosition = Vector3.ProjectOnPlane(m_MonoBehaviour.CreatureConstructor.Body.TransformPoint(leg.DefaultFootPosition), m_MonoBehaviour.transform.up);
            Vector3 velocityOffset = Vector3.ProjectOnPlane(m_MonoBehaviour.Velocity, m_MonoBehaviour.transform.up) * leg.TimeToMove;

            Vector3 origin = extremityPosition + velocityOffset + m_MonoBehaviour.transform.up;
            Vector3 dir = -m_MonoBehaviour.transform.up;

            if (Physics.Raycast(origin, dir, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground")))
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
                float height = m_MonoBehaviour.CreatureConstructor.ToBodySpace(leg.LegConstructor.Extremity.position).y;
                if (height < minHeight)
                {
                    minHeight = height;
                }
                avgHeight += height;
            }
            avgHeight /= m_MonoBehaviour.Legs.Length;

            float t = m_MonoBehaviour.Velocity.magnitude / movementSpeed;
            float offset = avgHeight - minHeight;
            Vector3 targetPosition = Vector3.Lerp(Vector3.zero, Vector3.up * offset, t);

            m_MonoBehaviour.CreatureConstructor.Root.localPosition = Vector3.Lerp(m_MonoBehaviour.CreatureConstructor.Root.localPosition, targetPosition, Time.deltaTime * bodySmoothing);
        }
        private void HandleBodyRotation()
        {
            Quaternion roll = Quaternion.identity, pitch = Quaternion.identity;

            // Roll
            Vector3 avgRFeetPos = GetAverageFeetPosition(m_MonoBehaviour.RLegs);
            Vector3 avgLFeetPos = GetAverageFeetPosition(m_MonoBehaviour.LLegs);
            
            Vector3 rollVector = avgRFeetPos - avgLFeetPos;
            float rollAngle = MathfUtility.Clamp(Vector3.SignedAngle(m_MonoBehaviour.CreatureConstructor.Body.right, rollVector, m_MonoBehaviour.CreatureConstructor.Body.forward), minMaxRoll);
            roll = Quaternion.AngleAxis(rollAngle, Vector3.forward);

            // Pitch
            int numPairs = m_MonoBehaviour.Legs.Length / 2;
            if (numPairs >= 2)
            {
                Vector3 avgFFeetPos = GetAverageFeetPosition(m_MonoBehaviour.LLegs[0], m_MonoBehaviour.RLegs[0]);
                Vector3 avgBFeetPos = GetAverageFeetPosition(m_MonoBehaviour.LLegs[numPairs - 1], m_MonoBehaviour.RLegs[numPairs - 1]);

                Vector3 pitchVector = avgFFeetPos - avgBFeetPos;
                float pitchAngle = MathfUtility.Clamp(Vector3.SignedAngle(m_MonoBehaviour.CreatureConstructor.Body.forward, pitchVector, m_MonoBehaviour.CreatureConstructor.Body.right), minMaxPitch);
                pitch = Quaternion.AngleAxis(pitchAngle, Vector3.right);
            }

            float t = m_MonoBehaviour.Velocity.magnitude / movementSpeed;
            Quaternion targetRotation = Quaternion.Lerp(Quaternion.identity, roll * pitch, t);

            Debug.Log($"{m_MonoBehaviour.Velocity.magnitude} / {movementSpeed} = {t}");

            // 0.8 * 0.8 = 0.64?!?!?!
            // why is the velocity 0.64... should be 0.8?

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