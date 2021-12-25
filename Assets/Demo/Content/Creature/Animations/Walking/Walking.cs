// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Walking : SceneLinkedSMB<CreatureAnimator>
    {
        [SerializeField] private float timeToMove = 0.25f;
        [SerializeField] private float distanceThreshold = 0.3f;
        [SerializeField] private float bodySmoothing = 4f;

        [SerializeField] private MinMax minMaxRoll = new MinMax(-20f, 20f);
        [SerializeField] private MinMax minMaxPitch = new MinMax(-10f, 10f);

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //LegAnimator leg = m_MonoBehaviour.Legs[0];
            //if (!leg.IsMovingToTarget)
            //{
            //    float length = Vector3.Distance(leg.transform.position, leg.LegConstructor.Extremity.position);
            //    if (length > 0.99f * leg.Length)
            //    {

            //    }
            //    else
            //    {
            //        Vector3 position = GetTargetFootPosition(leg);
            //        if (Vector3.Distance(position, leg.Anchor.position) > 0.2f)
            //        {

            //        }
            //    }
            //}
            
            foreach (LegAnimator leg in m_MonoBehaviour.Legs)
            {
                if (leg.IsMovingToTarget || leg.FlippedLeg.IsMovingToTarget) continue;
                Vector3 pos = GetTargetFootPosition(leg);
                if (Vector3.Distance(pos, leg.Anchor.position) > 0.2f)
                {
                    leg.StartCoroutine(leg.MoveToTargetRoutine(pos, m_MonoBehaviour.CreatureConstructor.Body.rotation, timeToMove, 0.25f));
                }
                Debug.DrawLine(pos + Vector3.up, pos);
            }
            HandleBody();
        }
        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.CreatureConstructor.Root.localRotation = Quaternion.identity;
            m_MonoBehaviour.CreatureConstructor.Root.localPosition = Vector3.zero;
        }

        //private IEnumerator MoveLegsRoutine()
        //{
        //    int numPairs = m_MonoBehaviour.Legs.Length / 2;
        //    for (int i = 0; i < numPairs; ++i)
        //    {
        //        LegAnimator lleg = m_MonoBehaviour.LLegs[i];
        //        LegAnimator rleg = m_MonoBehaviour.RLegs[i];
        //        m_MonoBehaviour.StartCoroutine(MovePairRoutine(rleg, lleg));

        //        yield return new WaitForSeconds(timeToMove * (3f / 4f));
        //    }
        //}
        //private IEnumerator MovePairRoutine(LegAnimator leg1, LegAnimator leg2)
        //{
        //    yield return m_MonoBehaviour.StartCoroutine(MoveToTargetRoutine(leg1));
        //    yield return m_MonoBehaviour.StartCoroutine(MoveToTargetRoutine(leg2));
        //}
        //private IEnumerator MoveToTargetRoutine(LegAnimator leg)
        //{
        //    float liftHeight = 0.25f;
        //    Vector3 position = GetTargetFootPosition(leg);
        //    Quaternion rotation = m_MonoBehaviour.CreatureConstructor.Body.rotation;

        //    yield return leg.StartCoroutine(leg.MoveToTargetRoutine(position, rotation, timeToMove, liftHeight));
        //}

        private Vector3 GetTargetFootPosition(LegAnimator leg)
        {
            Vector3 extremityPosition = Vector3.ProjectOnPlane(m_MonoBehaviour.CreatureConstructor.Body.TransformPoint(leg.DefaultFootPosition), m_MonoBehaviour.transform.up);
            Vector3 velocityOffset = m_MonoBehaviour.Velocity * timeToMove;

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

            float offset = avgHeight - minHeight;
            m_MonoBehaviour.CreatureConstructor.Root.localPosition = Vector3.Lerp(m_MonoBehaviour.CreatureConstructor.Root.localPosition, Vector3.up * offset, Time.deltaTime * bodySmoothing);
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

            // Yaw



            m_MonoBehaviour.CreatureConstructor.Root.localRotation = Quaternion.Slerp(m_MonoBehaviour.CreatureConstructor.Root.localRotation, roll * pitch, Time.deltaTime * bodySmoothing);
        }

        #region Helpers
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
    }
}