using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Strike : SceneLinkedSMB<CreatureAnimator>
    {
        [SerializeField] private float strikeTime;

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Transform headPos = m_MonoBehaviour.GetComponentInChildren<MouthAnimator>().transform;
            Transform targetCreature = m_MonoBehaviour.InteractTarget;

            Vector3 strikeDisplacement = targetCreature.position - headPos.position;
            
            Vector3 startPosition = m_MonoBehaviour.transform.position;
            Vector3 targetPosition = startPosition + strikeDisplacement;

            m_MonoBehaviour.InvokeOverTime(delegate (float p)
            {
                m_MonoBehaviour.Constructor.Body.position = Vector3.Lerp(startPosition, targetPosition, Mathf.Sin(p * Mathf.PI));
            }, 
            strikeTime);
        }
    }
}