// Interactions
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class Interactor : NetworkBehaviour
    {
        #region Fields
        [SerializeField] protected Camera interactionCamera;
        #endregion

        #region Properties
        public Camera InteractionCamera => interactionCamera;
        #endregion

        #region Methods
        private void OnEnable()
        {
            InteractionsManager.Instance.enabled = true;
        }
        private void OnDisable()
        {
            InteractionsManager.Instance.enabled = false;
        }

        public virtual void Setup()
        {
            InteractionsManager.Instance.Interactor = this;
        }
        private void Destroy()
        {
            if (InteractionsManager.Instance.Interactor == this) { InteractionsManager.Instance.Interactor = null; }
        }
        #endregion
    }
}