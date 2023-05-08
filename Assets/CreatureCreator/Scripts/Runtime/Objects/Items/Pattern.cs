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
        public override Sprite Icon => Sprite.Create(Texture as Texture2D, new Rect(0, 0, Texture.width, Texture.height), new Vector2(0.5f, 0.5f));
        #endregion
    }
}