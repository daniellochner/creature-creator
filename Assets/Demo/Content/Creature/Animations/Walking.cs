// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Walking : SceneLinkedSMB<CreatureAnimator>
    {
        //public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // slowly move up to default root pos.
        //}

        //public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    UpdatePositionAndRotation();

        //    foreach (LegAnimator leg in m_MonoBehaviour.Legs)
        //    {
        //        leg.HandleMovement();
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