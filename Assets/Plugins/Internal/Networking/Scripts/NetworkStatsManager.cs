// Credit: Adapted from the Unity "Boss Room" example project.

using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkStatsManager : NetworkSingleton<NetworkStatsManager>
    {
        #region Fields
        [SerializeField] private float m_PingIntervalSeconds = 0.25f;
        [SerializeField] private bool m_UseStats = true;

        private float m_LastPingTime;
        private int m_CurrentRTTPingId;

        private Queue<float> m_MovingWindow = new Queue<float>();
        private Dictionary<int, float> m_PingHistoryStartTimes = new Dictionary<int, float>();

        private const int k_MaxWindowSizeSeconds = 3; // It should take x seconds for the value to react to change.
        #endregion

        #region Properties
        public bool UseStats
        {
            get => m_UseStats;
            set => m_UseStats = value;
        }

        public float LastRTT { get; private set; }

        private float m_MaxWindowSize => k_MaxWindowSizeSeconds / m_PingIntervalSeconds;
        #endregion

        #region Methods
        private void FixedUpdate()
        {
            if (UseStats && !IsServer && Time.realtimeSinceStartup - m_LastPingTime > m_PingIntervalSeconds) RequestRTT();
        }

        private void RequestRTT()
        {
            PingServerRPC(m_CurrentRTTPingId, NetworkManager.Singleton.LocalClientId);

            m_PingHistoryStartTimes[m_CurrentRTTPingId] = Time.realtimeSinceStartup;
            m_CurrentRTTPingId++;
            m_LastPingTime = Time.realtimeSinceStartup;
        }

        private void UpdateRTTSlidingWindowAverage()
        {
            if (m_MovingWindow.Count > m_MaxWindowSize)
            {
                m_MovingWindow.Dequeue();
            }

            float rttSum = 0;
            foreach (var singleRTT in m_MovingWindow)
            {
                rttSum += singleRTT;
            }

            LastRTT = rttSum / m_MaxWindowSize;
        }

        [ServerRpc(RequireOwnership = false)]
        public void PingServerRPC(int pingId, ulong clientId)
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams()
            {
                Send = new ClientRpcSendParams()
                {
                    TargetClientIds = new [] { clientId }
                }
            };
            PongClientRPC(pingId, clientRpcParams);
        }

        [ClientRpc]
        public void PongClientRPC(int pingId, ClientRpcParams clientParams)
        {
            float startTime = m_PingHistoryStartTimes[pingId];
            m_PingHistoryStartTimes.Remove(pingId);
            m_MovingWindow.Enqueue(Time.realtimeSinceStartup - startTime);

            UpdateRTTSlidingWindowAverage();
        }
        #endregion
    }
}