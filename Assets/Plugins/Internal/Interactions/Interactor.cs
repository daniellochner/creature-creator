// Interactions
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets
{
    public class Interactor : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Camera interactionCamera;
        [SerializeField] private Texture2D defaultCursorIcon;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Interactable highlighted;
        [SerializeField, ReadOnly] private Interactable targeted;

        private Collider prevHitCollider;
        #endregion

        #region Properties
        public Camera InteractionCamera => interactionCamera;

        public Interactable Highlighted
        {
            get => highlighted;
            set
            {
                if (highlighted == value) return;

                highlighted?.Highlight(false);
                highlighted = value;
                highlighted?.Highlight(true);

                if (highlighted == null) prevHitCollider = null;

                Cursor.SetCursor((highlighted != null) ? highlighted.CursorIcon : defaultCursorIcon, Vector2.zero, CursorMode.Auto);
            }
        }
        #endregion

        #region Methods
        private void Update()
        {
            HandleInteractions();
        }

        private void HandleInteractions()
        {
            if (Physics.Raycast(RectTransformUtility.ScreenPointToRay(interactionCamera, Input.mousePosition), out RaycastHit hitInfo))
            {
                if (hitInfo.collider == prevHitCollider)
                {
                    if (Input.GetMouseButtonDown(0) && Highlighted != null && Highlighted.CanInteract(this))
                    {
                        targeted = Highlighted;
                        targeted.Interact();
                    }
                    return; // Returns to prevent having to recheck the same highlighted interactable every frame.
                }
                prevHitCollider = hitInfo.collider;

                Interactable interactable = prevHitCollider.GetComponent<Interactable>();
                if (interactable != null && interactable.CanHighlight(this))
                {
                    Highlighted = interactable;
                }
                else
                {
                    Highlighted = null;
                }
            }
            else
            {
                Highlighted = null;
            }
        }
        #endregion
    }
}