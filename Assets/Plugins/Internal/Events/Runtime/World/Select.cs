// Events
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(Hover), typeof(Click))]
    public class Select : MonoBehaviour
    {
        #region Fields
        [SerializeField] private UnityEvent onSelect = new UnityEvent();
        [SerializeField] private float outlineWidth = 5f;
        [SerializeField] private float threshold = 0f;
        [SerializeField] private string[] ignoredTags;

        private Outline outline;
        private Hover hover;
        private Click click;
        private Camera mainCamera;

        private bool isSelected;
        private Vector2 initialMousePosition;
        #endregion

        #region Properties

        public string[] IgnoredTags
        {
            get => ignoredTags;
            set => ignoredTags = value;
        }

        public bool useOutline = true;

        public UnityEvent OnSelect => onSelect;

        public Outline Outline => outline;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;

                if (useOutline) outline.enabled = isSelected;

                onSelect?.Invoke();
            }
        }

        public static Select GlobalSelected;
        #endregion

        #region Methods
        private void Awake()
        {
            mainCamera = Camera.main;

            hover = GetComponent<Hover>();
            click = GetComponent<Click>();
            click.Threshold = threshold;

            outline = GetComponentInChildren<Outline>();
            outline.OutlineWidth = outlineWidth;
            outline.enabled = false;
        }
        private void Start()
        {
            click.OnClick.AddListener(delegate
            {
                this.InvokeAtEndOfFrame(delegate
                {
                    if (!IsSelected)
                    {
                        IsSelected = true; // Wait until end of frame to allow previous selected object to de-select first.
                    }
                });
            });
        }
        private void Update()
        {
            if (IsSelected)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    initialMousePosition = Input.mousePosition;
                }
                else if (Input.GetMouseButtonUp(0) && Vector2.Distance(initialMousePosition, Input.mousePosition) <= threshold && !CanvasUtility.IsPointerOverUI)
                {
                    if (Physics.Raycast(RectTransformUtility.ScreenPointToRay(mainCamera, Input.mousePosition), out RaycastHit hitInfo))
                    {
                        Select select = hitInfo.collider.GetComponentInParent<Select>();

                        if (select == this)
                        {
                            return;
                        }
                        else
                        {
                            foreach (string tag in ignoredTags)
                            {
                                if (hitInfo.collider.CompareTag(tag))
                                {
                                    return;
                                }
                            }
                        }
                    }
                    IsSelected = false;
                }
            }
        }
        #endregion

        #region Delegates
        public delegate void SelectEvent(bool selected);
        #endregion
    }
}