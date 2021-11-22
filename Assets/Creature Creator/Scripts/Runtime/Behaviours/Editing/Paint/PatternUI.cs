// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(ExpandOnHover))]
    public class PatternUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Toggle selectToggle;
        [SerializeField] private ClickUI clickUI;
        #endregion

        #region Properties
        public Toggle SelectToggle => selectToggle;
        public ClickUI ClickUI => clickUI;
        #endregion

        #region Methods
        public void Setup(Texture pattern)
        {
            name = pattern.name;

            Image graphic = selectToggle.graphic as Image;
            Image targetGraphic = selectToggle.targetGraphic as Image;

            graphic.sprite = targetGraphic.sprite = Sprite.Create(pattern as Texture2D, new Rect(0, 0, pattern.width, pattern.height), new Vector2(0.5f, 0.5f));
        }
        #endregion
    }
}