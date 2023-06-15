using DanielLochner.Assets.CreatureCreator.Abilities;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SharkTale : TopKills
    {
        #region Fields
        [Header("Shark Tale")]
        [SerializeField] private Transform landSpawnPoint;
        #endregion

        #region Methods
        public override Transform GetSpawnPoint()
        {
            if (!Player.Instance.Abilities.HasAbility<Swim>())
            {
                return landSpawnPoint;
            }
            else
            {
                return base.GetSpawnPoint();
            }
        }
        #endregion
    }
}