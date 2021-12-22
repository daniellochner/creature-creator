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
        private float moveThreshold = 0.25f;
        private float liftHeight = 0.25f;
        private float timeToMove = 0.5f;
        #endregion

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            liftHeight = m_MonoBehaviour.CreatureConstructor.Root.localPosition.y * 0.25f;
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            HandleBody();
            foreach (LegAnimator leg in m_MonoBehaviour.Legs)
            {
                HandleLeg(leg);
            }
        }

        private void HandleBody()
        {
        }
        private void HandleLeg(LegAnimator leg)
        {
        }
        

        //if (IsMovingToTarget || FlippedLeg.IsMovingToTarget)
        //{
        //    return;
        //}

        //Vector3 extremityOffset = m_MonoBehaviour.CreatureConstructor.Body.TransformVector(Vector3.ProjectOnPlane(defaultBonePositions[defaultBonePositions.Length - 1] / 4f, CreatureAnimator.transform.up));
        //Vector3 velocityOffset = (CreatureAnimator.MoveThreshold / 2f) * CreatureAnimator.CreatureConstructor.Body.forward;
        //Vector3 origin = transform.position + (extremityOffset + velocityOffset);

        //if (Physics.Raycast(origin, -CreatureAnimator.transform.up, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground")))
        //{
        //    Vector3 pos = hitInfo.point;

        //    Debug.DrawLine(origin, pos);


        //    if (Vector3.Distance(pos, anchor.position) > CreatureAnimator.MoveThreshold)
        //    {
        //        if (LegConstructor.ConnectedFoot != null)
        //        {
        //            pos += CreatureAnimator.transform.up * LegConstructor.ConnectedFoot.Offset;
        //        }
        //        StartCoroutine(MoveToTargetRoutine(pos));
        //    }
        //}
        
        //private void UpdatePositionAndRotation()
        //{
        //    if (m_MonoBehaviour.Legs.Length > 0)
        //    {
        //        // Position
        //        float offset = 0;
        //        foreach (LegAnimator leg in m_MonoBehaviour.Legs)
        //        {
        //            Vector3 vOffset = leg.LegConstructor.Root.InverseTransformPoint(leg.LimbConstructor.Extremity.position) - leg.ExtremityOffset;

        //            offset += vOffset.y;
        //        }
        //        offset /= m_MonoBehaviour.Legs.Length;

        //        Vector3 rootPosition = m_MonoBehaviour.CreatureConstructor.Root.localPosition;
        //        rootPosition.y = m_MonoBehaviour.DefaultHeight + offset;

        //        m_MonoBehaviour.CreatureConstructor.Root.localPosition = rootPosition;


        //        // Rotation
        //        //Vector3 forward = Vector3.zero;
        //        //Vector3 right = Vector3.zero;

        //        //foreach (LimbAnimator limb in Limbs)
        //        //{
        //        //    if (limb.IsFlipped) { continue; }

        //        //    Vector3 displacement = transform.InverseTransformPoint(limb.LimbConstructor.Extremity.position - limb.LimbConstructor.FlippedLimb.Extremity.position);
        //        //    forward += Vector3.Project(displacement, Vector3.forward);
        //        //    right += Vector3.Project(displacement, Vector3.right);
        //        //}

        //        //int numPairs = Limbs.Length / 2;
        //        //forward /= numPairs;
        //        //right /= numPairs;

        //        //CreatureConstructor.Body.localRotation = Quaternion.LookRotation(Vector3.Cross(forward, right));
        //    }
        //}





    }
}