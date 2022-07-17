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

        public void Show()
        {
            ShowServerRpc(JsonUtility.ToJson(Constructor.Data));
        }
        [ServerRpc(RequireOwnership = false)]
        private void ShowServerRpc(string creatureData)
        {
            ShowClientRpc(creatureData);
        }
        [ClientRpc]
        private void ShowClientRpc(string creatureData)
        {
            if (IsOwner) return;

            Constructor.Demolish();
            gameObject.SetActive(true);
            CreatureData data = JsonUtility.FromJson<CreatureData>(creatureData);
            Constructor.Construct(data);

            if (NetworkCreaturesMenu.Instance)
            {
                NetworkCreaturesMenu.Instance.SetName(OwnerClientId, data.Name);
            }
        }

        public void Hide()
        {
            HideServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void HideServerRpc()
        {
            HideClientRpc();
        }
        [ClientRpc]
        private void HideClientRpc()
        {
            if (IsOwner) return;

            gameObject.SetActive(false);

            if (NetworkCreaturesMenu.Instance)
            {
                NetworkCreaturesMenu.Instance.SetName(OwnerClientId, "...");
            }
        }
        #endregion
    }
}