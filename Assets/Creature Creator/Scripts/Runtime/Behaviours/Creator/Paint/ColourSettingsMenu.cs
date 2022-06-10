// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ColourSettingsMenu : Menu
    {
        #region Fields
        [SerializeField] private Slider shineSlider;
        [SerializeField] private Slider metallicSlider;
        #endregion

        #region Properties
        public Slider ShineSlider => shineSlider;
        public Slider MetallicSlider => metallicSlider;
        #endregion

        #region Methods
        public void Start()
        {
            shineSlider.onValueChanged.AddListener(UpdateShine);
            metallicSlider.onValueChanged.AddListener(UpdateMetallic);
        }
        public void Reset()
        {
            shineSlider.value = 0f;
            metallicSlider.value = 0f;
        }

        private void UpdateShine(float shine)
        {
            EditorManager.Instance.Player.Creature.Constructor.SetShine(shine);
        }
        private void UpdateMetallic(float metallic)
        {
            EditorManager.Instance.Player.Creature.Constructor.SetMetallic(metallic);
        }
        #endregion
    }
}