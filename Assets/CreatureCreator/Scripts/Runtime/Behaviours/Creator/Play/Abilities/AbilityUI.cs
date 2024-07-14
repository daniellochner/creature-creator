// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AbilityUI : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerExitHandler
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image cooldownImage;
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

        public bool CanAct => Player.Instance.Abilities.enabled;
        #endregion

        #region Methods
        private void Update()
        {
            if (ability != null && Player.Instance)
            {
                UpdateUI();
            }
        }
        private void OnDestroy()
        {
            LocalizationSettings.Instance.OnSelectedLocaleChanged -= UpdateName;
        }

        public void Setup(Ability ability)
        {
            this.ability = ability;

            LocalizationSettings.Instance.OnSelectedLocaleChanged += UpdateName;
            UpdateName();

            cooldownImage.fillAmount = 0f;

            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                performKeyText.gameObject.SetActive(false);
                nameText.rectTransform.anchorMin = Vector2.zero;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (CanAct)
            {
                ability.OnTryPerform();
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (CanAct)
            {
                ability.OnTryPrepare();
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (CanAct && ability.IsPrepared)
            {
                ability.OnTryPerform();
            }
        }

        public void UpdateUI()
        {
            performKeyText.text = $"[{ability.PerformKeybind.ToString()}]";
            cooldownImage.fillAmount = ability.CooldownTimeLeft / ability.Cooldown;

            IsInteractable = (ability.CooldownTimeLeft <= 0f) && ability.CanPerform;
        }
        private void UpdateName(Locale locale = default)
        {
            nameText.text = LocalizationUtility.Localize(ability.Name);
        }
        #endregion
    }
}