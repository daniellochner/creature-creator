// Interactions
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets
{
    public class Interactor : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Camera interactionCamera;
        #endregion

        #region Properties
        public Camera InteractionCamera => interactionCamera;
        #endregion

        #region Methods
        private void OnEnable()
        {
            InteractionsManager.Instance.Interactor = this;
        }
        private void OnDisable()
        {
            if (InteractionsManager.Instance.Interactor == this) { InteractionsManager.Instance.Interactor = null; }
        }
        #endregion
    }
}