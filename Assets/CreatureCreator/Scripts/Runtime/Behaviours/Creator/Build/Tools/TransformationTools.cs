// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TransformationTools : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TransformationToolsDictionary transformationTools;
        #endregion

        #region Properties
        public TransformationToolsDictionary TransformationToolsDict => transformationTools;
        #endregion

        #region Methods
        private void Start()
        {
            foreach (Tool tool in transformationTools.Values)
            {
                tool.Initialize();
            }
        }

        public void Show(BodyPartEditor bpe, Transformation transformations)
        {
            transform.SetParent(bpe.transform, false);

            foreach (Transformation transformation in transformationTools.Keys)
            {
                if (transformations.HasFlag(transformation))
                {
                    Tool tool = transformationTools[transformation];

                    tool.Setup(bpe);

                    if (tool.CanShow)
                    {
                        tool.gameObject.SetActive(true);
                    }
                }
            }
        }
        public void Hide()
        {
            transform.SetParent(Dynamic.Transform, false);

            foreach (Tool tool in transformationTools.Values)
            {
                tool.gameObject.SetActive(false);
            }
        }
        #endregion

        #region Inner Classes
        [Serializable] public class TransformationToolsDictionary : SerializableDictionaryBase<Transformation, Tool> { }
        #endregion
    }
}