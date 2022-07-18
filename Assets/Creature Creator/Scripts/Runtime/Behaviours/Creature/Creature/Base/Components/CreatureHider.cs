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
        #region Fields
        [SerializeField] private NetworkVariable<bool> isHidden = new NetworkVariable<bool>();
        #endregion

        #region Properties
        private CreatureConstructor Constructor { get; set; }
        private CreatureNamer Namer { get; set; }

        public Action OnShow { get; set; }
        public Action OnHide { get; set; }

        public bool IsHidden => isHidden.Value;
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Namer = GetComponent<CreatureNamer>();
        }

        public void Setup()
        {
            if (!IsOwner)
            {
                if (!IsHidden)
                {
                    RequestShow();
                }
            }
        }
        
        public void RequestShow()
        {
            RequestShowServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        [ServerRpc(RequireOwnership = false)]
        public void RequestShowServerRpc(ulong showToClientId)
        {
            ShowClientRpc(JsonUtility.ToJson(Constructor.Data), NetworkUtils.SendTo(showToClientId));
        }

        public void Show()
        {
            ShowServerRpc(JsonUtility.ToJson(Constructor.Data));
        }
        [ServerRpc(RequireOwnership = false)]
        private void ShowServerRpc(string creatureData)
        {
            ShowClientRpc(creatureData);
            isHidden.Value = false;
        }
        [ClientRpc]
        private void ShowClientRpc(string creatureData, ClientRpcParams clientRpcParams = default)
        {
            if (!IsOwner)
            {
                Constructor.Demolish();
                Constructor.Body.gameObject.SetActive(true);
                Constructor.Construct(JsonUtility.FromJson<CreatureData>(creatureData));
            }
            OnShow?.Invoke();
        }

        public void Hide()
        {
            HideServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void HideServerRpc()
        {
            HideClientRpc();
            isHidden.Value = true;
        }
        [ClientRpc]
        private void HideClientRpc()
        {
            if (!IsOwner)
            {
                Constructor.Body.gameObject.SetActive(false);
            }
            OnHide?.Invoke();
        }
        #endregion
    }
}