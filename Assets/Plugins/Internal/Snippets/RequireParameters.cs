using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(Animator))]
    public class RequireParameters : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Parameter[] paramaters;
        [Space]
        [SerializeField, Button("UpdateParameterList")] private bool updateParameterList;

        private Animator animator;
        #endregion

        #region Methods
#if UNITY_EDITOR
        private void Start()
        {
            UpdateParameterList();
        }

        public void UpdateParameterList()
        {
            animator = GetComponent<Animator>();

            if (animator.runtimeAnimatorController != null)
            {
                UnityEditor.Animations.AnimatorController controller = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
                foreach (Parameter parameter in paramaters)
                {
                    if (string.IsNullOrEmpty(parameter.name)) continue;

                    if (!controller.HasParameter(parameter.name))
                    {
                        controller.AddParameter(parameter.name, parameter.type);
                        Debug.LogWarning($"{parameter.name} was added to {controller.name}.", animator);
                    }
                }
            }
        }
#endif
        #endregion

        #region Inner Classes
        [Serializable]
        public class Parameter
        {
            public string name;
            public AnimatorControllerParameterType type;
        }
        #endregion
    }
}