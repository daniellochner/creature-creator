using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorSMB<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Properties
        public Animator Animator { get; private set; }
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
        }
        protected virtual void OnDisable()
        {
            foreach (SceneLinkedSMB<T> smb in Animator.GetBehaviours<SceneLinkedSMB<T>>())
            {
                for (int i = 0; i < Animator.layerCount; i++)
                {
                    AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(i);
                    if (stateInfo.shortNameHash == smb.StateInfo.shortNameHash)
                    {
                        smb.OnSLStatePreExit(Animator, stateInfo, i);
                        smb.OnSLStateExit(Animator, stateInfo, i);
                    }
                }
            }
        }
        #endregion
    }
}