// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public abstract class BodyPart : Item
    {
        #region Fields
        [Header("Body Part")]
        [SerializeField] private BodyPartPrefab prefab;
        [SerializeField] private BodyPartDefaultColours defaultColours;
        [SerializeField] private int price;
        [SerializeField] private int complexity;
        [SerializeField] private int health;
        [SerializeField] private float weight;
        [SerializeField, Range(1, 5)] private int appeal = 1;

        [SerializeField] private Transformation transformations;
        [SerializeField, HasFlag("transformations", Transformation.Pivot | Transformation.PivotXY)] public float pivotOffset = 0.5f;
        [SerializeField, HasFlag("transformations", Transformation.Rotate)] private float rotateScaleFactor = 1f;
        [SerializeField, HasFlag("transformations", Transformation.Scale)] private MinMax minMaxScale = new MinMax(0.25f, 2.5f);
        [SerializeField, HasFlag("transformations", Transformation.Scale)] private float scaleIncrement = 0.1f;
        [SerializeField, HasFlag("transformations", Transformation.StretchX | Transformation.StretchY | Transformation.StretchZ)] public float stretchDistance = 0.25f;
        [SerializeField] private float toolsScaleFactor = 1f;

        [SerializeField] private List<Ability> abilities;
        #endregion

        #region Properties
        public BodyPartPrefab Prefab => prefab;
        public BodyPartDefaultColours DefaultColours
        {
            get => defaultColours;
            set => defaultColours = value;
        }

        public Transformation Transformations
        {
            get => transformations;
            set => transformations = value;
        }
        public int Price
        {
            get => price;
            set => price = value;
        }
        public int Complexity
        {
            get => complexity;
            set => complexity = value;
        }
        public int Health
        {
            get => health;
            set => health = value;
        }
        public float Weight
        {
            get => weight;
            set => weight = value;
        }

        public int Appeal => appeal;
        public float PivotOffset => pivotOffset;
        public float RotateScaleFactor => rotateScaleFactor;
        public MinMax MinMaxScale => minMaxScale;
        public float ScaleIncrement => scaleIncrement;
        public float StretchDistance => stretchDistance;
        public float ToolsScaleFactor => toolsScaleFactor;
        public List<Ability> Abilities => abilities;

        public abstract string PluralForm { get; }
        public virtual int BaseComplexity { get; }
        #endregion

        #region Methods
        public GameObject GetPrefab(PrefabType prefabType)
        {
            GameObject prefab = null;

            switch (prefabType)
            {
                case PrefabType.Constructible:
                    prefab = this.prefab.constructible;
                    break;
                case PrefabType.Animatable:
                    prefab = this.prefab.animatable;
                    break;
                case PrefabType.Editable:
                    prefab = this.prefab.editable;
                    break;
            }

            return prefab;
        }

        public virtual void Add(CreatureStatistics stats)
        {
            stats.complexity += Complexity;
            stats.health += Health;
        }
        public virtual void Remove(CreatureStatistics stats)
        {
            stats.complexity -= Complexity;
            stats.health -= Health;
        }

        public override string ToString()
        {
            return name;
        }
        #endregion

        #region Enums
        public enum PrefabType
        {
            Constructible,
            Animatable,
            Editable
        }
        #endregion

        #region Inner Classes
        [Serializable]
        public class BodyPartDefaultColours
        {
            public Color primary;
            public Color secondary;
        }

        [Serializable]
        public class BodyPartPrefab
        {
            public GameObject constructible;
            public GameObject animatable;
            public GameObject editable;
        }
        #endregion
    }
}