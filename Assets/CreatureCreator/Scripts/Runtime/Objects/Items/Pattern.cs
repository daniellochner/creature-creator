using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Pattern")]
    public class Pattern : Item
    {
        #region Fields
        [Header("Pattern")]
        [SerializeField] private Texture2D texture;
        #endregion

        #region Properties
        public Texture2D Texture => texture;
        #endregion
    }
}