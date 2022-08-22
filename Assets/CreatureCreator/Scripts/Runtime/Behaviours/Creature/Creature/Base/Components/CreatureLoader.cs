// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureLoader : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private TextAsset cachedData;
        [SerializeField, ReadOnly] private NetworkVariable<bool> isHidden = new NetworkVariable<bool>();
        #endregion

        #region Properties
        private CreatureConstructor Constructor { get; set; }

        public Action OnShow { get; set; }
        public Action OnHide { get; set; }

        public bool IsHidden => isHidden.Value;
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }

        #region Show
        public void ShowToMe()
        {
            if (cachedData != null)
            {
                Show(JsonUtility.FromJson<CreatureData>(cachedData.text));
            }
            else
            {
                ShowToMeServerRpc(NetworkManager.Singleton.LocalClientId);
            }
        }
        public void ShowMeToOthers()
        {
            ShowMeToOthersServerRpc(new NetworkCreatureData(Constructor.Data), NetworkManager.Singleton.LocalClientId);
            OnShow?.Invoke();
        }

        [ServerRpc(RequireOwnership = false)]
        private void ShowToMeServerRpc(ulong clientId)
        {
            ShowClientRpc(new NetworkCreatureData(Constructor.Data), NetworkUtils.SendTo(clientId));
        }
        [ServerRpc]
        private void ShowMeToOthersServerRpc(NetworkCreatureData data, ulong clientId)
        {
            List<ulong> clientIds = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);
            clientIds.Remove(clientId); // Don't show me to me!
            ShowClientRpc(data, NetworkUtils.SendTo(clientIds.ToArray()));
            isHidden.Value = false;
        }

        [ClientRpc]
        private void ShowClientRpc(NetworkCreatureData data, ClientRpcParams clientRpcParams = default)
        {
            Show(data.Data);
        }

        private void Show(CreatureData data)
        {
            Constructor.Demolish();
            Constructor.Body.gameObject.SetActive(true);
            Constructor.Construct(data);
            OnShow?.Invoke();
        }
        #endregion

        #region Hide
        public void HideFromOthers()
        {
            HideFromOthersServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void HideFromOthersServerRpc()
        {
            HideFromOthersClientRpc();
            isHidden.Value = true;
        }
        [ClientRpc]
        private void HideFromOthersClientRpc()
        {
            if (!IsOwner)
            {
                Constructor.Body.gameObject.SetActive(false);
            }
            OnHide?.Invoke();
        }
        #endregion
        #endregion
    }
}