// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    [RequireComponent(typeof(SimpleScrollSnap))]
    public abstract class TransitionEffectBase<T> : MonoBehaviour where T : Component
    {
        #region Fields
        [SerializeField] protected MinMax minMaxDisplacement = new MinMax(-1000, 1000);
        [SerializeField] protected MinMax minMaxValue = new MinMax(0, 1);
        [SerializeField] protected AnimationCurve function = AnimationCurve.Linear(0, 0, 1, 1);

        private Dictionary<GameObject, T> cachedComponents = new Dictionary<GameObject, T>();
        #endregion

        #region Methods
        public void OnTransition(GameObject panel, float displacement)
        {
            if (!cachedComponents.ContainsKey(panel))
            {
                cachedComponents.Add(panel, panel.GetComponent<T>());
            }

            float t = Mathf.InverseLerp(minMaxDisplacement.min, minMaxDisplacement.max, displacement);
            float f = function.Evaluate(t);
            float v = Mathf.Lerp(minMaxValue.min, minMaxValue.max, f);
            OnTransition(cachedComponents[panel], v);
        }
        public abstract void OnTransition(T component, float value);
        #endregion
    }
}