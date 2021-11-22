// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class StatisticsMenu : MenuSingleton<StatisticsMenu>
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI authorText;
        [SerializeField] private TextMeshProUGUI complexityText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI dietText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI abilitiesText;

        private bool hasEntered;
        #endregion

        #region Methods
        public void Setup(BodyPart bodyPart)
        {
            nameText.text = $"{bodyPart.name} (${bodyPart.Price})";
            authorText.text = $"By {bodyPart.Author}";
            complexityText.text = bodyPart.Complexity.ToString();
            healthText.text = bodyPart.Health.ToString();

            // Diet
            bool isMouth = bodyPart is Mouth;
            dietText.transform.parent.gameObject.SetActive(isMouth);
            if (isMouth)
            {
                dietText.text = (bodyPart as Mouth).Diet.ToString();
            }

            // Speed
            bool isLeg = bodyPart is Leg;
            speedText.transform.parent.gameObject.SetActive(isLeg);
            if (isLeg)
            {
                speedText.text = (bodyPart as Leg).Speed.ToString();
            }

            // Abilities
            bool hasAbilities = bodyPart.Abilities.Count > 0;
            abilitiesText.transform.parent.gameObject.SetActive(hasAbilities);
            if (hasAbilities)
            {
                abilitiesText.text = string.Join(", ", bodyPart.Abilities);
            }

            Open();
            hasEntered = true;
        }
        public void Clear()
        {
            hasEntered = false;

            EditorManager.Instance.Invoke(delegate
            {
                if (!hasEntered)
                {
                    Close();
                }
            }, 0.15f);
        }

        public override void Open(bool instant = false)
        {
            gameObject.SetActive(true);
            base.Open(instant);
        }
        public override void OnEndClose()
        {
            base.OnEndClose();
            gameObject.SetActive(false);
        }
        #endregion
    }
}