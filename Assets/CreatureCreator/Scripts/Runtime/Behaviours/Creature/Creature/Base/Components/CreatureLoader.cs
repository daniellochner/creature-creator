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
            ShowMeToOthersServerRpc(Constructor.Data, NetworkManager.Singleton.LocalClientId);
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

        //private string GetOptimizedData()
        //{
        //    return OptimizeData(JsonUtility.ToJson(Constructor.Data));
        //}
        //private string OptimizeData(string data)
        //{
        //    // Replace all exponential notation with 0
        //    data = Regex.Replace(data, "[+-]?[0-9]+\\.[0-9]+e[+-][0-9]+", "0");

        //    // Round all floats to X decimal points
        //    MatchEvaluator round = new MatchEvaluator(Round);
        //    data = Regex.Replace(data, "[+-]?[0-9]+\\.[0-9]+", round);

        //    return data;
        //}
        //private string Round(Match t)
        //{
        //    string num = t.Value;
        //    int i = num.IndexOf('.');

        //    int length = num.Substring(i + 1).Length;

        //    if (length > precision)
        //    {
        //        return num.Substring(0, (i + 1) + precision);
        //    }
        //    else
        //    {
        //        return num;
        //    }
        //}
        #endregion
    }
}