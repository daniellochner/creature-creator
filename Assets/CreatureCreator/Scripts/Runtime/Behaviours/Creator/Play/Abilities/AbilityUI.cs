// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AbilityUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image cooldownImage;
        [SerializeField] private Button performButton;
        [SerializeField] private TextMeshProUGUI performKeyText;
        [SerializeField] private CanvasGroup abilityCG;

        private Ability ability;
        #endregion

        #region Properties
        public bool IsInteractable
        {
            set
            {
                abilityCG.alpha = value ? 1f : 0.5f;
                abilityCG.interactable = value;
            }
        }
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

            performButton.onClick.RemoveAllListeners();
            performButton.onClick.AddListener(delegate 
            {
                ability.OnTryPerform();
            });

            nameText.text = ability.name;
            cooldownImage.fillAmount = 0f;
        }
        public void UpdateUI()
        {
            performKeyText.text = $"[{ability.PerformKeybind.ToString()}]";
            cooldownImage.fillAmount = ability.CooldownTimeLeft / ability.Cooldown;

            IsInteractable = (ability.CooldownTimeLeft <= 0f) && ability.CanPerform;
        }
        #endregion
    }
}