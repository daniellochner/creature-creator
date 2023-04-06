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
        [SerializeField] private bool useOutline = true;

        private bool isSelected;
        private Vector2 initialMousePosition;
        #endregion

        #region Properties
        public UnityEvent OnSelect
        {
            get => onSelect;
        }
        public float OutlineWidth
        {
            get => outlineWidth;
            set => outlineWidth = value;
        }
        public float Threshold
        {
            get => threshold;
            set => threshold = value;
        }
        public string[] IgnoredTags
        {
            get => ignoredTags;
            set => ignoredTags = value;
        }
        public bool UseOutline
        {
            get => useOutline;
            set => useOutline = value;
        }

        public Outline Outline { get; private set; }
        public Hover Hover { get; private set; }
        public Click Click { get; private set; }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;

                if (useOutline) Outline.enabled = isSelected;

                onSelect?.Invoke();
            }
        }

        public static Select GlobalSelected;
        #endregion

        #region Methods
        private void Awake()
        {
            Hover = GetComponent<Hover>();
            Click = GetComponent<Click>();
            Click.Threshold = threshold;

            Outline = GetComponentInChildren<Outline>();
            Outline.OutlineWidth = outlineWidth;
            Outline.enabled = false;
        }
        private void Start()
        {
            Click.OnLeftClick.AddListener(delegate
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
                    if (Physics.Raycast(RectTransformUtility.ScreenPointToRay(Camera.main, Input.mousePosition), out RaycastHit hitInfo))
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
    }
}