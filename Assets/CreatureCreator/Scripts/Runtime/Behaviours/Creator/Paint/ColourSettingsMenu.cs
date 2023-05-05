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
        [SerializeField] private PressUI shinePressUI;
        [Space]
        [SerializeField] private Slider metallicSlider;
        [SerializeField] private PressUI metallicPressUI;
        #endregion

        #region Properties
        public Slider ShineSlider => shineSlider;
        public Slider MetallicSlider => metallicSlider;
        #endregion

        #region Methods
        private void Start()
        {
            shineSlider.onValueChanged.AddListener(UpdateShine);
            metallicSlider.onValueChanged.AddListener(UpdateMetallic);

            shinePressUI.OnRelease.AddListener(delegate
            {
                EditorManager.Instance.TakeSnapshot(Change.SetShine);
            });
            metallicPressUI.OnRelease.AddListener(delegate
            {
                EditorManager.Instance.TakeSnapshot(Change.SetMetallic);
            });
        }
        public void Reset()
        {
            shineSlider.value = 0f;
            metallicSlider.value = 0f;
        }

        private void UpdateShine(float shine)
        {
            EditorManager.Instance.Creature.Constructor.SetShine(shine);
        }
        private void UpdateMetallic(float metallic)
        {
            EditorManager.Instance.Creature.Constructor.SetMetallic(metallic);
        }
        #endregion
    }
}