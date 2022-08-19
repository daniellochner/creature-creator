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

        private readonly string end = ",\"0000001D\":{\"type\":{\"class\":\"Terminus\",\"ns\":\"UnityEngine.DMAT\",\"asm\":\"FAKE_ASM\"},\"data\":{}}}}";
        private Coroutine showCoroutine;
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
            
        }


        // Request to show this creature to me
        public void RequestShow()
        {

            RequestShowServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        [ServerRpc(RequireOwnership = false)]
        public void RequestShowServerRpc(ulong showToClientId)
        {
            ShowClientRpc(OptimizedData, NetworkUtils.SendTo(showToClientId));
        }

        // Show me to everyone else
        public void Show()
        {
            ShowServerRpc(OptimizedData);
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
            ShowOptimized(creatureData);
        }
        [ClientRpc]
        private void ShowCachedClientRpc(ClientRpcParams clientRpcParams = default)
        {
            ShowOptimized(cachedData.text);
        }
        private void ShowOptimized(string data)
        {
            if (!IsOwner)
            {
                try
                {
                    data = ParseOptimizedData(data);

                    CreatureData dat = JsonUtility.FromJson<CreatureData>(data);
                    Constructor.Demolish();
                    Constructor.Body.gameObject.SetActive(true);
                    Constructor.Construct(dat);
                }
                catch
                {
                    Debug.Log($"Parse error: {data}");
                    return;
                }
            }
            OnShow?.Invoke();
        }

        // Hide me from everyone else
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






        
        public string OptimizedData
        {
            get => OptimizeData(JsonUtility.ToJson(Constructor.Data));
        }

        public string ParseOptimizedData(string data)
        {
            return data + end;
        }
        public string OptimizeData(string data)
        {
            Debug.Log(data);
            // Remove common end portion
            data = data.Substring(0, data.Length - end.Length);

            Debug.Log(data);

            // Replace all exponential notation with 0
            data = Regex.Replace(data, "[+-]?[0-9]+\\.[0-9]+e[+-][0-9]+", "0");
            Debug.Log(data);

            // Round all floats to X decimal points
            MatchEvaluator round = new MatchEvaluator(Round);
            data = Regex.Replace(data, "[+-]?[0-9]+\\.[0-9]+", round);
            Debug.Log(data);

            return data;
        }

        private string Round(Match t)
        {
            string num = t.Value;
            int i = num.IndexOf('.');

            int length = num.Substring(i + 1).Length;

            return num.Substring(0, (i + 1) + precision);
        }
        #endregion
    }
}