using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ProjectileGroup : MonoBehaviour
    {
        #region Fields
        private int count;
        #endregion

        #region Properties
        public bool HasDamaged { get; set; }

        public int Count
        {
            get => count;
            set
            {
                count = value;

                if (count <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        #endregion
    }
}