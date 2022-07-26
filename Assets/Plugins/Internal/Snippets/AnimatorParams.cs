using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorParams : MonoBehaviour
    {
        public Animator Animator { get; private set; }

        public Action<string> OnSetTrigger { get; set; }
        public Action<string, bool> OnSetBool { get; set; }
        public Action<string, float> OnSetFloat { get; set; }
        public Action<string, int> OnSetInteger { get; set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }
        
        public void SetTrigger(string param, bool notify = true)
        {
            Animator.SetTrigger(param);
            if (notify)
            {
                OnSetTrigger?.Invoke(param);
            }
        }
        public void SetBool(string param, bool value, bool notify = true)
        {
            Animator.SetBool(param, value);
            if (notify)
            {
                OnSetBool?.Invoke(param, value);
            }
        }
        public void SetFloat(string param, float value, bool notify = true)
        {
            Animator.SetFloat(param, value);
            if (notify)
            {
                OnSetFloat?.Invoke(param, value);
            }
        }
        public void SetInteger(string param, int value, bool notify = true)
        {
            Animator.SetInteger(param, value);
            if (notify)
            {
                OnSetInteger?.Invoke(param, value);
            }
        }

        public bool GetBool(string param)
        {
            return Animator.GetBool(param);
        }
        public float GetFloat(string param)
        {
            return Animator.GetFloat(param);
        }
        public float GetInteger(string param)
        {
            return Animator.GetInteger(param);
        }
    }
}