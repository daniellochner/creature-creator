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
        [SerializeField, ReadOnly] private NetworkVariable<bool> isHidden = new NetworkVariable<bool>();

        private float loadTimeLeft;
        private int counter;
        #endregion

        #region Properties
        private CreatureConstructor Constructor { get; set; }
        private CreatureOptimizer Optimizer { get; set; }

        public Action OnShow { get; set; }
        public Action OnHide { get; set; }

        public bool IsHidden => isHidden.Value;
        private bool RateLimit => rateLimit && GameSetup.Instance.IsMultiplayer;
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
            if (!IsHidden) return;

            ShowMeToOthersServerRpc(Constructor.Data, NetworkManager.Singleton.LocalClientId);
            OnShow?.Invoke();

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
        }

        [ServerRpc(RequireOwnership = false)]
        private void ShowToMeServerRpc(ulong clientId)
        {
            ShowClientRpc(Constructor.Data, NetworkUtils.SendTo(clientId));
        }
        [ServerRpc]
        private void ShowMeToOthersServerRpc(CreatureData data, ulong clientId)
        {
            List<ulong> clientIds = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);
            clientIds.Remove(clientId); // Don't show me to me!
            ShowClientRpc(data, NetworkUtils.SendTo(clientIds.ToArray()));
            isHidden.Value = false;
        }

        [ClientRpc]
        private void ShowClientRpc(CreatureData data, ClientRpcParams clientRpcParams = default)
        {
            Show(data);
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
            if (IsHidden) return;
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