// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureHider : MonoBehaviour
    {
        #region Properties
        private CreatureConstructor Constructor { get; set; }

        public Action OnHide { get; set; }
        public Action OnShow { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }

        public void Hide()
        {
            SetHidden(true);
            OnHide?.Invoke();
        }
        public void Show()
        {
            SetHidden(false);
            OnShow?.Invoke();
        }
        private void SetHidden(bool isHidden)
        {
            foreach (Renderer renderer in Constructor.Body.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = !isHidden;
            }
        }
        #endregion
    }
}