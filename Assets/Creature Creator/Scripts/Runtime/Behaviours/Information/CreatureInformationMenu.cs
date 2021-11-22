// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureInformationMenu : Menu
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI nameAgeText;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider energySlider;
        [SerializeField] private Image photoImage;

        private CreatureInformation information = null;
        #endregion

        #region Methods
        private void Update()
        {
            if (information != null)
            {
                UpdateInfo();
            }
        }

        public void Setup(CreatureInformation information)
        {
            this.information = information;
            UpdateInfo();
        }
        private void UpdateInfo()
        {
            nameAgeText.text = $"<u>{information.Name}</u> ({FormatAge(information.Age)})";
            healthSlider.value = information.Health;
            energySlider.value = information.Energy;

            if (information.Photo != null && photoImage.sprite.texture != information.Photo) // Prevents having to recreate the sprite every frame.
            {
                Vector2 position = Vector2.zero;
                Vector2 size = new Vector2(information.Photo.width, information.Photo.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);

                photoImage.sprite = Sprite.Create(information.Photo, new Rect(position, size), pivot);
            }
        }
        private string FormatAge(int age)
        {
            int minutes = age / 60;
            int seconds = age % 60;

            return $"{minutes}m{seconds}s";
        }
        #endregion
    }
}