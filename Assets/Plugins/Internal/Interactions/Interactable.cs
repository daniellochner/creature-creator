// Interactions
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class Interactable : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Texture2D cursor;
        [SerializeField] private float interactionRange = Mathf.Infinity;
        #endregion

        #region Properties
        public Texture2D Cursor => cursor;

        public Collider Col { get; set; }

        public virtual bool IsHighlighted { get; set; }
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            Col = GetComponent<Collider>();
        }

        public virtual bool CanInteract(Interactor interactor)
        {
            return !Vector3Utility.SqrDistanceComp(transform.position, interactor.transform.position, interactionRange);
        }
        public virtual bool CanHighlight(Interactor interactor) => CanInteract(interactor);

        public void Interact(Interactor interactor)
        {
            OnInteract(interactor);
        }
        public void Highlight(Interactor interactor, bool isHighlighted)
        {
            OnHighlight(interactor, IsHighlighted = isHighlighted);
        }

        protected virtual void OnInteract(Interactor interactor)
        {

        }
        protected virtual void OnHighlight(Interactor interactor, bool isHighlighted)
        {

        }
        #endregion
    }
}