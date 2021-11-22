using Unity.Netcode;
using System;
using UnityEngine;

namespace DanielLochner.Assets
{        
    /// <summary>
    /// No callback exists for OnShutdown in NetworkManager, and so we need to check the connection every frame.
    /// </summary>
    public class NetworkShutdownManager : MonoBehaviourSingleton<NetworkShutdownManager>
    {
        #region Fields        
        private bool isConnected, isControlledShutdown;
        #endregion

        #region Properties
        public Action OnShutdown { get; set; }

        public Action OnControlledShutdown { get; set; }
        public Action OnUncontrolledShutdown { get; set; }
        public Action OnUncontrolledClientShutdown { get; set; }
        public Action OnUncontrolledHostShutdown { get; set; }
        #endregion

        #region Methods
        private void Update()
        {
            HandleConnection();
        }

        private void HandleConnection()
        {
            if (NetworkManager.Singleton.IsListening && !isConnected)
            {
                isConnected = true;
            }
            else
            if (!NetworkManager.Singleton.IsListening && isConnected)
            {
                isConnected = false;
                if (!isControlledShutdown)
                {
                    HandleUncontrolledShutdown();
                }
                isControlledShutdown = false;
            }
        }
        private void HandleUncontrolledShutdown()
        {
            OnShutdown?.Invoke();
            OnUncontrolledShutdown?.Invoke();

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                OnUncontrolledClientShutdown?.Invoke();
            }
            else
            {
                OnUncontrolledHostShutdown?.Invoke();
            }
        }

        public void Shutdown()
        {
            isControlledShutdown = true;
            NetworkManager.Singleton.Shutdown();

            OnShutdown?.Invoke();
            OnControlledShutdown?.Invoke();
        }
        #endregion
    }
}