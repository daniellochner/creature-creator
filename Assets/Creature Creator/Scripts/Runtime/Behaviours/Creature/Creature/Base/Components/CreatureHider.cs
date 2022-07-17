// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureHider : NetworkBehaviour
    {
        #region Properties
        private CreatureConstructor Constructor { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }

        public void Hide()
        {
            SetHidden(true);
        }
        public void Show()
        {
            SetHidden(false);
        }
        private void SetHidden(bool isHidden)
        {
            gameObject.SetActive(isHidden);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetHiddenServerRpc(bool isHidden)
        {
            SetHiddenClientRpc(isHidden);
        }

        [ClientRpc]
        private void SetHiddenClientRpc(bool isHidden)
        {
            if (IsOwner) return;
            SetHidden(isHidden);
        }
        #endregion
    }
}