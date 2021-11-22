// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Body Part/Limb/Leg")]
    public class Leg : Limb
    {
        #region Fields
        [Header("Limb")]
        [SerializeField] private int speed;
        #endregion

        #region Properties
        public int Speed => speed;

        public override string PluralForm => "Legs";
        #endregion

        #region Methods
        public override void Add(CreatureStatistics stats)
        {
            base.Add(stats);

            stats.speed += Speed;
        }
        public override void Remove(CreatureStatistics stats)
        {
            base.Remove(stats);

            stats.speed -= Speed;
        }
        #endregion
    }
}