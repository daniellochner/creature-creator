// Interactions
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class InteractionsManager : MonoBehaviourSingleton<InteractionsManager>
    {
        #region Fields
        [SerializeField] private Texture2D defaultCursor;
        [SerializeField] private InteractionUI interactionPrefab;
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private ToggleGroup toggleGroup;
        [Header("Debug")]
        [SerializeField, ReadOnly] private GameObject targeted;
        [SerializeField, ReadOnly] private Interactable highlighted;

        private List<InteractionUI> interactions = new List<InteractionUI>();
        private Collider prevHitCollider;
        private int highlightedIndex;

        #endregion

        #region Properties
        public Interactor Interactor { get; set; }

        public Action<Interactable> OnHighlight { get; set; }
        public Action<GameObject> OnTarget { get; set; }

        public GameObject Targeted
        {
            get => targeted;
            set
            {
                if (targeted == value) return;
                targeted = value;

                Clear();
                if (targeted != null)
                {
                    Setup(targeted);
                }

                OnTarget?.Invoke(targeted);
            }
        }
        public Interactable Highlighted
        {
            get => highlighted;
            set
            {
                if (highlighted == value) return;

                highlighted?.Highlight(Interactor, false);
                highlighted = value;
                highlighted?.Highlight(Interactor, true);

                if (highlighted == null) prevHitCollider = null;

                if (highlighted == null)
                {
                    Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
                    Cursor.visible = true;
                }
                else
                {
                    if (interactions.Count == 1)
                    {
                        Cursor.SetCursor(highlighted.Cursor, Vector2.zero, CursorMode.Auto);
                        Cursor.visible = true;
                    }
                    else
                    {
                        Cursor.visible = false;
                    }
                }

                OnHighlight?.Invoke(highlighted);
            }
        }
        public int HighlightedIndex
        {
            get => highlightedIndex;
            set
            {
                highlightedIndex = value;

                int max = interactions.Count - 1;
                if (highlightedIndex < 0)
                {
                    highlightedIndex = max;
                }
                else 
                if (highlightedIndex > max)
                {
                    highlightedIndex = 0;
                }

                InteractionUI interaction = interactions[highlightedIndex];
                Highlighted = interaction.Interactable;
                interaction.Select();
            }
        }
        #endregion

        #region Methods
        private void Update()
        {
            HandleUI();
            HandleInteractions();
        }

        private void HandleUI()
        {
            if (grid.transform.childCount > 0)
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    HighlightedIndex++;
                }
                else
                if (Input.mouseScrollDelta.y < 0)
                {
                    HighlightedIndex--;
                }
            }
        }
        private void HandleInteractions()
        {
            if (Interactor == null) return;

            if (Physics.Raycast(RectTransformUtility.ScreenPointToRay(Interactor.InteractionCamera, Input.mousePosition), out RaycastHit hitInfo))
            {
                if (hitInfo.collider == prevHitCollider)
                {
                    if (Input.GetMouseButtonDown(0) && Highlighted != null && Highlighted.CanInteract(Interactor))
                    {
                        Highlighted.Interact(Interactor);
                    }
                    return; // Returns to prevent having to recheck the same highlighted interactable every frame.
                }
                prevHitCollider = hitInfo.collider;

                Interactable interactable = prevHitCollider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    Targeted = prevHitCollider.gameObject;
                }
                else
                {
                    Targeted = null;
                    Highlighted = null;
                }
            }
            else
            {
                Targeted = null;
                Highlighted = null;
            }
        }

        public void Setup(GameObject obj)
        {
            foreach (Interactable interactable in obj.GetComponents<Interactable>())
            {
                if (interactable.CanHighlight(Interactor))
                {
                    InteractionUI interaction = Instantiate(interactionPrefab, grid.transform);
                    interaction.Setup(interactable, toggleGroup);

                    interactions.Add(interaction);
                }
            }
            if (interactions.Count > 0)
            {
                HighlightedIndex = 0;
            }

            grid.gameObject.SetActive(interactions.Count > 1);
        }
        public void Clear()
        {
            interactions.Clear();
            grid.transform.DestroyChildren();
        }
        #endregion
    }
}