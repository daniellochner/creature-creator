// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using System.Text.RegularExpressions;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureLoader : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private int precision = 3;
        [SerializeField] private float showCooldown = 10f;
        [SerializeField] private TextAsset cachedData;

        [SerializeField] private NetworkVariable<bool> isHidden = new NetworkVariable<bool>();

        private float showTimeLeft;
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
        private void Update()
        {
            showTimeLeft = Mathf.Max(showTimeLeft - Time.deltaTime, 0);
        }

        // Show To Me
        public void ShowToMe()
        {
            ShowToSpecificServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        [ServerRpc(RequireOwnership = false)]
        private void ShowToSpecificServerRpc(ulong showToClientId)
        {
            ClientRpcParams p = NetworkUtils.SendTo(showToClientId);
            if (cachedData != null)
            {
                ShowToSpecificCachedClientRpc(p);
            }
            else
            {
                ShowToSpecificClientRpc(GetOptimizedData(), p);
            }
        }
        [ClientRpc]
        private void ShowToSpecificClientRpc(string creatureData, ClientRpcParams clientRpcParams = default)
        {
            Construct(creatureData);
        }
        [ClientRpc]
        private void ShowToSpecificCachedClientRpc(ClientRpcParams clientRpcParams = default)
        {
            Construct(cachedData.text);
        }

        // Show Me To Others
        public void ShowMeToOthers()
        {
            this.Invoke(delegate
            {
                ShowToOthersServerRpc(GetOptimizedData());
            }, 
            showTimeLeft);
            showTimeLeft = showCooldown;
        }
        [ServerRpc(RequireOwnership = false)]
        private void ShowToOthersServerRpc(string creatureData)
        {
            if (cachedData != null)
            {
                ShowToOthersCachedClientRpc();
            }
            else
            {
                ShowToOthersClientRpc(creatureData);
            }
            isHidden.Value = false;
        }
        [ClientRpc]
        private void ShowToOthersClientRpc(string creatureData)
        {
            if (!IsOwner)
            {
                Construct(creatureData);
            }
        }
        [ClientRpc]
        private void ShowToOthersCachedClientRpc()
        {
            if (!IsOwner)
            {
                Construct(cachedData.text);
            }
        }

        // Construct
        private void Construct(string data)
        {
            try
            {
                CreatureData dat = JsonUtility.FromJson<CreatureData>(data);
                Constructor.Demolish();
                Constructor.Body.gameObject.SetActive(true);
                Constructor.Construct(dat);

                OnShow?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        // Hide Me From Others
        public void HideMeFromOthers()
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

        // Optimize
        private string GetOptimizedData()
        {
            return OptimizeData(JsonUtility.ToJson(Constructor.Data));
        }
        private string OptimizeData(string data)
        {
            // Replace all exponential notation with 0
            data = Regex.Replace(data, "[+-]?[0-9]+\\.[0-9]+e[+-][0-9]+", "0");

            // Round all floats to X decimal points
            MatchEvaluator round = new MatchEvaluator(Round);
            data = Regex.Replace(data, "[+-]?[0-9]+\\.[0-9]+", round);

            return data;
        }
        private string Round(Match t)
        {
            string num = t.Value;
            int i = num.IndexOf('.');

            int length = num.Substring(i + 1).Length;

            if (length > precision)
            {
                return num.Substring(0, (i + 1) + precision);
            }
            else
            {
                return num;
            }
        }
        #endregion
    }
}