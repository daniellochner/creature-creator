// Interactions
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
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
        private Interactable prevHighlighted;
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
                else
                {
                    Highlighted = null;
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

                Cursor.SetCursor((highlighted == null) ? defaultCursor : highlighted.Cursor, Vector2.zero, CursorMode.Auto);

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
        private void OnDisable()
        {
            Targeted = null;
        }
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
                if (Highlighted != null && hitInfo.collider == Highlighted.Col)
                {
                    if (Highlighted.CanHighlight(Interactor))
                    {
                        if (Input.GetMouseButtonDown(0) && Highlighted.CanInteract(Interactor))
                        {
                            Highlighted.Interact(Interactor);
                        }
                    }
                    else
                    {
                        Targeted = null;
                    }
                }
                else
                {
                    GameObject t = null;
                    foreach (Interactable interactable in hitInfo.collider.GetComponents<Interactable>())
                    {
                        if (interactable != null && interactable.CanHighlight(Interactor))
                        {
                            t = interactable.gameObject;
                        }
                    }
                    Targeted = t;
                }
            }
            else
            {
                Targeted = null;
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