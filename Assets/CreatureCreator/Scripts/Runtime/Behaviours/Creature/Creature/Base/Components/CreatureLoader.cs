// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureLoader : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private bool rateLimit;
        [SerializeField, DrawIf("rateLimit", true)] private float loadCooldown;
        [SerializeField, DrawIf("rateLimit", true)] private int warnAt;
        [SerializeField, DrawIf("rateLimit", true)] private int kickAt;
        [SerializeField] private TextAsset cachedData;

        private float loadTimeLeft;
        private int counter;
        private string prevData;
        #endregion

        #region Properties
        private CreatureConstructor Constructor { get; set; }
        private CreatureOptimizer Optimizer { get; set; }

        public Action OnShow { get; set; }
        public Action OnHide { get; set; }

        private bool RateLimit => rateLimit && WorldManager.Instance.IsMultiplayer;

        public NetworkVariable<bool> IsHidden { get; set; } = new NetworkVariable<bool>(true);
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }
        private void Update()
        {
            if (RateLimit) { loadTimeLeft = Mathf.Max(loadTimeLeft - Time.deltaTime, 0); }
        }

        #region Show
        public void ShowToMe()
        {
            ShowToMeServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        public void ShowMeToOthers()
        {
            if (!IsHidden.Value) return;

            string nextData = JsonUtility.ToJson(Constructor.Data);
            if (nextData != prevData)
            {
                ShowMeToOthersServerRpc(Constructor.Data, NetworkManager.Singleton.LocalClientId);
                if (RateLimit)
                {
                    if (loadTimeLeft > 0)
                    {
                        counter++;
                        if (counter >= kickAt)
                        {
                            NetworkConnectionManager.Instance.ForceDisconnect(LocalizationUtility.Localize("disconnect_message_construct-spam"));
                        }
                        else
                        if (counter >= warnAt)
                        {
                            InformationDialog.Inform(LocalizationUtility.Localize("cc_load-cooldown_title"), LocalizationUtility.Localize("cc_load-cooldown_message", loadCooldown, (counter - warnAt) + 1));
                        }
                    }
                    loadTimeLeft = loadCooldown;
                }

                prevData = nextData;
            }
            else
            {
                ShowMeToOthersServerRpc(null, NetworkManager.Singleton.LocalClientId);
            }

            OnShow?.Invoke();
        }

        [ServerRpc(RequireOwnership = false)]
        private void ShowToMeServerRpc(ulong clientId)
        {
            if (cachedData != null)
            {
                ShowCachedClientRpc(NetworkUtils.SendTo(clientId));
            }
            else
            {
                ShowClientRpc(Constructor.Data, NetworkUtils.SendTo(clientId));
            }
        }
        [ServerRpc]
        private void ShowMeToOthersServerRpc(CreatureData data, ulong clientId)
        {
            List<ulong> clientIds = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);
            clientIds.Remove(clientId); // Don't show me to me!
            ShowClientRpc(data, NetworkUtils.SendTo(clientIds.ToArray()));
            IsHidden.Value = false;
        }

        [ClientRpc]
        private void ShowClientRpc(CreatureData data, ClientRpcParams clientRpcParams = default)
        {
            Show(data);
        }
        [ClientRpc]
        private void ShowCachedClientRpc(ClientRpcParams clientRpcParams = default)
        {
            Show(JsonUtility.FromJson<CreatureData>(cachedData.text));
        }

        private void Show(CreatureData data)
        {
            if (data != null)
            {
                Constructor.Demolish();
                Constructor.Body.gameObject.SetActive(true);
                Constructor.Construct(data);
            }
            else
            {
                Constructor.Body.gameObject.SetActive(true);
            }

            OnShow?.Invoke();
        }
        #endregion

        #region Hide
        public void HideFromOthers()
        {
            if (IsHidden.Value) return;

            HideFromOthersServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void HideFromOthersServerRpc()
        {
            HideFromOthersClientRpc();
            IsHidden.Value = true;
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