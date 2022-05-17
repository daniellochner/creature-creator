// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Looking : SceneLinkedSMB<CreatureAnimator>
    {
        [SerializeField] private float rotSmoothing;

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Transform target = m_MonoBehaviour.LookTarget;
            if (target != null)
            {
                Vector3 direction = Vector3.ProjectOnPlane(target.position - m_MonoBehaviour.transform.position, m_MonoBehaviour.transform.up);
                Quaternion rotation = Quaternion.LookRotation(direction);

                m_MonoBehaviour.transform.rotation = Quaternion.Slerp(m_MonoBehaviour.transform.rotation, rotation, rotSmoothing * Time.deltaTime);
            }
        }
    }
}