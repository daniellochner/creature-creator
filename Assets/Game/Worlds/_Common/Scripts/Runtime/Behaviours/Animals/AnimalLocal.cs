// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AnimalLocal : CreatureNonPlayerLocal
    {
        #region Fields
        [SerializeField] private AnimalAI ai;
        #endregion

        #region Properties
        public AnimalAI AI => ai;
        #endregion

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            ai = GetComponent<AnimalAI>();
        }
#endif

        public override void Setup()
        {
            base.Setup();
            AI.Setup();
        }
    }
}