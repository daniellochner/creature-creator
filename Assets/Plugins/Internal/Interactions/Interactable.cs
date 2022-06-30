// Interactions
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets
{
    public abstract class Interactable : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Sprite icon;
        [SerializeField] private Texture2D cursorIcon;
        [SerializeField] private float interactionRange = Mathf.Infinity;
        #endregion

        #region Properties
        public Sprite Icon => icon;
        public Texture2D CursorIcon => cursorIcon;

        public virtual bool IsHighlighted { get; set; }
        #endregion

        #region Methods
        public virtual bool CanInteract(Interactor interactor)
        {
            return !Vector3Utility.SqrDistanceComp(transform.position, interactor.transform.position, interactionRange);
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