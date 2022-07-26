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
        public Action<string, string, float> OnSetTriggerWithValue { get; set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }
        
        public void SetTrigger(string triggerParam, bool notify = true)
        {
            Animator.SetTrigger(triggerParam);
            if (notify)
            {
                OnSetTrigger?.Invoke(triggerParam);
            }
        }
        public void SetBool(string boolParam, bool boolValue, bool notify = true)
        {
            Animator.SetBool(boolParam, boolValue);
            if (notify)
            {
                OnSetBool?.Invoke(boolParam, boolValue);
            }
        }
        public void SetFloat(string floatParam, float floatValue, bool notify = true)
        {
            Animator.SetFloat(floatParam, floatValue);
            if (notify)
            {
                OnSetFloat?.Invoke(floatParam, floatValue);
            }
        }
        public void SetInteger(string intParam, int intValue, bool notify = true)
        {
            Animator.SetInteger(intParam, intValue);
            if (notify)
            {
                OnSetInteger?.Invoke(intParam, intValue);
            }
        }

        public bool GetBool(string boolParam)
        {
            return Animator.GetBool(boolParam);
        }
        public float GetFloat(string floatParam)
        {
            return Animator.GetFloat(floatParam);
        }
        public float GetInteger(string intParam)
        {
            return Animator.GetInteger(intParam);
        }

        public void SetTriggerWithValue(string triggerParam, string floatParam, float floatValue, bool notify = true)
        {
            Animator.SetFloat(floatParam, floatValue);
            Animator.SetTrigger(triggerParam);
            if (notify)
            {
                OnSetTriggerWithValue?.Invoke(triggerParam, floatParam, floatValue);
            }
        }
    }
}