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

            BodyPartEditor paintedBodyPart = EditorManager.Instance.Creature.Editor.PaintedBodyPart;
            if (paintedBodyPart != null)
            {
                paintedBodyPart.BodyPartConstructor.ResetPrimaryColour();
                paintedBodyPart.BodyPartConstructor.ResetSecondaryColour();
            }
            else
            {
                EditorManager.Instance.Creature.Constructor.SetPrimaryColour(Color.white);
                EditorManager.Instance.Creature.Constructor.SetSecondaryColour(Color.black);
            }

            EditorManager.Instance.Creature.Editor.UpdatePainted();
        }

        private void UpdateShine(float shine)
        {
            BodyPartEditor paintedBodyPart = EditorManager.Instance.Creature.Editor.PaintedBodyPart;
            if (paintedBodyPart != null)
            {
                paintedBodyPart.BodyPartConstructor.SetShine(shine, true);
            }
            else
            {
                EditorManager.Instance.Creature.Constructor.SetShine(shine);
            }
        }
        private void UpdateMetallic(float metallic)
        {
            BodyPartEditor paintedBodyPart = EditorManager.Instance.Creature.Editor.PaintedBodyPart;
            if (paintedBodyPart != null)
            {
                paintedBodyPart.BodyPartConstructor.SetMetallic(metallic, true);
            }
            else
            {
                EditorManager.Instance.Creature.Constructor.SetMetallic(metallic);
            }
        }
        #endregion
    }
}