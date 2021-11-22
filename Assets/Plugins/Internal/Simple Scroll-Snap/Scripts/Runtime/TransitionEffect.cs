// Simple Scroll-Snap
// Version: 1.1.5
// Author: Daniel Lochner

using System;
using UnityEditor;
using UnityEngine;

namespace DanielLochner.Assets
{
    [Serializable]
    public class TransitionEffect
    {
        #region Fields
        [SerializeField]
        protected float minDisplacement, maxDisplacement, minValue, maxValue, defaultMinValue, defaultMaxValue, defaultMinDisplacement, defaultMaxDisplacement;
        [SerializeField]
        protected bool showPanel, showDisplacement, showValue;
        [SerializeField]
        private string label;
        [SerializeField]
        private AnimationCurve function;
        [SerializeField]
        private AnimationCurve defaultFunction;
        [SerializeField]
        private SimpleScrollSnap simpleScrollSnap;
        #endregion

        #region Properties
        public string Label
        {
            get { return label; }
            set { label = value; }
        }
        public float MinValue
        {
            get { return MinValue; }
            set { minValue = value; }
        }
        public float MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
        public float MinDisplacement
        {
            get { return minDisplacement; }
            set { minDisplacement = value; }
        }
        public float MaxDisplacement
        {
            get { return maxDisplacement; }
            set { maxDisplacement = value; }
        }
        public AnimationCurve Function
        {
            get { return function; }
            set { function = value; }
        }
        #endregion

        #region Methods
        public TransitionEffect(string label, float minValue, float maxValue, float minDisplacement, float maxDisplacement, AnimationCurve function, SimpleScrollSnap simpleScrollSnap)
        {
            this.label = label;
            this.simpleScrollSnap = simpleScrollSnap;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.minDisplacement = minDisplacement;
            this.maxDisplacement = maxDisplacement;
            this.function = function;

            SetDefaultValues(minValue, maxValue, minDisplacement, maxDisplacement, function);
            #if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            #endif
        }

        private void SetDefaultValues(float minValue, float maxValue, float minDisplacement, float maxDisplacement, AnimationCurve function)
        {
            defaultMinValue = minValue;
            defaultMaxValue = maxValue;
            defaultMinDisplacement = minDisplacement;
            defaultMaxDisplacement = maxDisplacement;
            defaultFunction = function;
        }
        #if UNITY_EDITOR
        public void Init()
        {
            GUILayout.BeginVertical("HelpBox");
            showPanel = EditorGUILayout.Foldout(showPanel, label, true);
            if (showPanel)
            {
                EditorGUI.indentLevel++;
                float x = minDisplacement;
                float y = minValue;
                float width = maxDisplacement - minDisplacement;
                float height = maxValue - minValue;

                // Min/Max Values
                showValue = EditorGUILayout.Foldout(showValue, "Value", true);
                if (showValue)
                {
                    EditorGUI.indentLevel++;
                    minValue = EditorGUILayout.FloatField(new GUIContent("Min"), minValue);
                    maxValue = EditorGUILayout.FloatField(new GUIContent("Max"), maxValue);
                    EditorGUI.indentLevel--;
                }

                // Min/Max Displacements
                showDisplacement = EditorGUILayout.Foldout(showDisplacement, "Displacement", true);
                if (showDisplacement)
                {
                    EditorGUI.indentLevel++;
                    minDisplacement = EditorGUILayout.FloatField(new GUIContent("Min"), minDisplacement);
                    maxDisplacement = EditorGUILayout.FloatField(new GUIContent("Max"), maxDisplacement);
                    EditorGUI.indentLevel--;
                }
                
                // Function
                function = EditorGUILayout.CurveField("Function", function, Color.white, new Rect(x, y, width, height));

                // Reset
                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUI.indentLevel * 16);
                if (GUILayout.Button("Reset"))
                {
                    Reset();
                }

                // Remove
                if (GUILayout.Button("Remove"))
                {
                    simpleScrollSnap.transitionEffects.Remove(this);
                }
                GUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
            GUILayout.EndVertical();
        }
        #endif
        public void Reset()
        {
            minValue = defaultMinValue;
            maxValue = defaultMaxValue;
            minDisplacement = defaultMinDisplacement;
            maxDisplacement = defaultMaxDisplacement;
            function = defaultFunction;
        }
        public float GetValue(float displacement)
        {
            return (function != null) ? function.Evaluate(displacement) : 0f;
        }
        #endregion
    }
}