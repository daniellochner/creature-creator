// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Body Part/Feature/Mouth")]
    public class Mouth : Feature
    {
        #region Fields
        [Header("Mouth")]
        [SerializeField] private Diet diet;
        #endregion

        #region Properties
        public Diet Diet => diet;

        public override string PluralForm => "Mouths";
        public override int BaseComplexity => 5;
        #endregion

        #region Methods
        public override void Add(CreatureStatistics stats)
        {
            base.Add(stats);

            stats.Diets.Add(Diet);
        }
        public override void Remove(CreatureStatistics stats)
        {
            base.Remove(stats);

            stats.Diets.Remove(Diet);
        }
        #endregion
    }
}