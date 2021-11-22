using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BPStatsWindow : EditorWindow
    {
        #region Fields
        [SerializeField] private Database bodyParts;
        [SerializeField] private MinMax minMaxHealth;
        [SerializeField] private int maxComplexity;

        private SerializedProperty _bodyParts, _minMaxHealth, _maxComplexity;
        private SerializedObject target;
        #endregion

        #region Methods
        private void OnEnable()
        {
            target = new SerializedObject(this);

            _bodyParts = target.FindProperty("bodyParts");
            _minMaxHealth = target.FindProperty("minMaxHealth");
            _maxComplexity = target.FindProperty("maxComplexity");
        }
        private void OnGUI()
        {
            target.Update();

            EditorGUILayout.PropertyField(_bodyParts);
            EditorGUILayout.PropertyField(_minMaxHealth);
            EditorGUILayout.PropertyField(_maxComplexity);

            EditorGUILayout.Space();

            if (!bodyParts || maxComplexity == 0f || minMaxHealth.min == 0f || minMaxHealth.max == 0f)
            {
                GUI.enabled = false;
            }
            if (GUILayout.Button("Update Stats"))
            {
                UpdateStats();
            }
            GUI.enabled = true;

            target.ApplyModifiedProperties();
        }

        public void UpdateStats()
        {
            MinMax minMaxVolume = new MinMax(Mathf.Infinity, Mathf.NegativeInfinity);

            foreach (var obj in bodyParts.Objects.Values)
            {
                BodyPart bodyPart = obj as BodyPart;

                float volume = bodyPart.Volume;
                if (volume > minMaxVolume.max)
                {
                    minMaxVolume.max = volume;
                }
                else
                if (volume < minMaxVolume.min)
                {
                    minMaxVolume.min = volume;
                }
            }

            foreach (var obj in bodyParts.Objects.Values)
            {
                BodyPart bodyPart = obj as BodyPart;

                // Transformations
                if (!(bodyPart is Limb))
                {
                    bodyPart.Transformations |= Transformation.Scale;
                }
                if (!(bodyPart is Limb || bodyPart is Extremity))
                {
                    bodyPart.Transformations |= Transformation.Pivot;
                    bodyPart.Transformations |= Transformation.PivotXY;
                }
                bodyPart.Transformations |= Transformation.Rotate;

                // Complexity
                bodyPart.Complexity = Math.Min(bodyPart.BaseComplexity + bodyPart.Abilities.Count, maxComplexity);

                // Health
                float t = Mathf.InverseLerp(minMaxVolume.min, minMaxVolume.max, bodyPart.Volume);
                bodyPart.Health = (int)Mathf.Lerp(minMaxHealth.min, minMaxHealth.max, t);

                // Price
                int tValue = 0;
                if (bodyPart.Transformations.HasFlag(Transformation.Rotate) || bodyPart.Transformations.HasFlag(Transformation.Scale))   tValue++;
                if (bodyPart.Transformations.HasFlag(Transformation.Pivot)  || bodyPart.Transformations.HasFlag(Transformation.PivotXY)) tValue++;
                if (bodyPart.Transformations.HasFlag(Transformation.StretchX)) tValue++;
                if (bodyPart.Transformations.HasFlag(Transformation.StretchY)) tValue++;
                if (bodyPart.Transformations.HasFlag(Transformation.StretchZ)) tValue++;

                int nonFunctionalValue = bodyPart.Appeal + tValue;
                int functionalValue = bodyPart.Health + bodyPart.Complexity;

                bodyPart.Price = (int)(0.6f * functionalValue + 0.4f * nonFunctionalValue) * 5;
            }
        }

        [MenuItem("Creature Creator/Body Part Stats")]
        public static void ShowWindow()
        {
            GetWindow<BPStatsWindow>("Body Part Stats");
        }
        #endregion
    }
}