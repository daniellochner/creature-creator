// Interactions
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class Interactable : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Texture2D cursorIcon;
        [SerializeField] private MinMax minMaxInteractionRange = new MinMax(0f, Mathf.Infinity);
        #endregion

        #region Properties
        public Texture2D CursorIcon => cursorIcon;

        public virtual bool IsHighlighted { get; set; }
        #endregion

        #region Methods
        public virtual bool CanInteract(Interactor interactor)
        {
            float distance = Vector3.Distance(transform.position, interactor.transform.position);
            return distance > minMaxInteractionRange.min && distance < minMaxInteractionRange.max;
        }
        public virtual bool CanHighlight(Interactor interactor) => CanInteract(interactor);

        public void Interact()
        {
            OnInteract();
        }
        public void Highlight(bool isHighlighted)
        {
            OnHighlight(IsHighlighted = isHighlighted);
        }

        protected virtual void OnInteract()
        {

        }
        protected virtual void OnHighlight(bool isHighlighted)
        {

        }
        #endregion
    }
}