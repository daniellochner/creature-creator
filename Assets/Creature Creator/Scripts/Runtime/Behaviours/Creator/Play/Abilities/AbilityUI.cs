// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AbilityUI : MonoBehaviour, IPointerDownHandler
    {
        #region Fields
        [SerializeField] private Image iconImage;
        [SerializeField] private Image cooldownImage;
        [SerializeField] private Button performButton;
        [SerializeField] private TextMeshProUGUI performKeyText;

        private Ability ability;
        #endregion

        #region Methods
        private void Update()
        {
            if (ability != null)
            {
                UpdateUI();
            }
        }

        public void Setup(Ability ability)
        {
            this.ability = ability;

            iconImage.sprite = ability.Icon;
            performKeyText.text = $"[{ability.PerformKey.ToString()}]";
            performButton.onClick.AddListener(delegate 
            {
                ability.OnTryPerform();
            });
            cooldownImage.fillAmount = 0f;
        }
        public void UpdateUI()
        {
            cooldownImage.fillAmount = ability.CooldownTimeLeft / ability.Cooldown;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            performButton.interactable = (ability.CooldownTimeLeft == 0f) && ability.CanPerform;
        }
        #endregion
    }
}