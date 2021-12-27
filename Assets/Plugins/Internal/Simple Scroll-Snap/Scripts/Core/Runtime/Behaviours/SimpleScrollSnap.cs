// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    [AddComponentMenu("UI/Simple Scroll-Snap")]
    [RequireComponent(typeof(ScrollRect))]
    public class SimpleScrollSnap : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        #region Fields
        // Movement and Layout Settings
        [SerializeField] private MovementType movementType = MovementType.Fixed;
        [SerializeField] private MovementAxis movementAxis = MovementAxis.Horizontal;
        [SerializeField] private bool useAutomaticLayout = true;
        [SerializeField] private float automaticLayoutSpacing = 0.25f;
        [SerializeField] private SizeControl sizeControl = SizeControl.Fit;
        [SerializeField] private Vector2 size = new Vector2(400, 250);
        [SerializeField] private Margins automaticLayoutMargins = new Margins(0);
        [SerializeField] private bool useInfiniteScrolling = false;
        [SerializeField] private float infiniteScrollingSpacing = 0.25f;
        [SerializeField] private bool useOcclusionCulling = false;
        [SerializeField] private int startingPanel = 0;

        // Navigation Settings
        [SerializeField] private bool useSwipeGestures = true;
        [SerializeField] private float minimumSwipeSpeed = 0f;
        [SerializeField] private Button previousButton = null;
        [SerializeField] private Button nextButton = null;
        [SerializeField] private ToggleGroup pagination = null;
        [SerializeField] private bool useToggleNavigation = true;

        // Snap Settings
        [SerializeField] private SnapTarget snapTarget = SnapTarget.Next;
        [SerializeField] private float snapSpeed = 10f;
        [SerializeField] private float thresholdSpeedToSnap = -1f;
        [SerializeField] private bool useHardSnapping = true;
        [SerializeField] private bool useUnscaledTime = false;
        
        // Events
        [SerializeField] private UnityEvent<GameObject, float> onTransitionEffects = new UnityEvent<GameObject, float>();
        [SerializeField] private UnityEvent<int> onPanelSelecting = new UnityEvent<int>();
        [SerializeField] private UnityEvent<int> onPanelSelected = new UnityEvent<int>();
        [SerializeField] private UnityEvent<int, int> onPanelCentering = new UnityEvent<int, int>();
        [SerializeField] private UnityEvent<int, int> onPanelCentered = new UnityEvent<int, int>();

        private ScrollRect scrollRect;
        private Vector2 contentSize, prevAnchoredPosition, velocity;
        private Direction releaseDirection;
        private float releaseSpeed;
        private bool isDragging, isPressing, isSelected = true;
        #endregion

        #region Properties
        public MovementType MovementType
        {
            get => movementType;
            set => movementType = value;
        }
        public MovementAxis MovementAxis
        {
            get => movementAxis;
            set => movementAxis = value;
        }
        public bool UseAutomaticLayout
        {
            get => useAutomaticLayout;
            set => useAutomaticLayout = value;
        }
        public SizeControl SizeControl
        {
            get => sizeControl;
            set => sizeControl = value;
        }
        public Vector2 Size
        {
            get => size;
            set => size = value;
        }
        public float AutomaticLayoutSpacing
        {
            get => automaticLayoutSpacing;
            set => automaticLayoutSpacing = value;
        }
        public Margins AutomaticLayoutMargins
        {
            get => automaticLayoutMargins;
            set => automaticLayoutMargins = value;
        }
        public bool UseInfiniteScrolling
        {
            get => useInfiniteScrolling;
            set => useInfiniteScrolling = value;
        }
        public float InfiniteScrollingSpacing
        {
            get => infiniteScrollingSpacing;
            set => infiniteScrollingSpacing = value;
        }
        public bool UseOcclusionCulling
        {
            get => useOcclusionCulling;
            set => useOcclusionCulling = value;
        }
        public int StartingPanel
        {
            get => startingPanel;
            set => startingPanel = value;
        }
        public bool UseSwipeGestures
        {
            get => useSwipeGestures;
            set => useSwipeGestures = value;
        }
        public float MinimumSwipeSpeed
        {
            get => minimumSwipeSpeed;
            set => minimumSwipeSpeed = value;
        }
        public Button PreviousButton
        {
            get => previousButton;
            set => previousButton = value;
        }
        public Button NextButton
        {
            get => nextButton;
            set => nextButton = value;
        }
        public ToggleGroup Pagination
        {
            get => pagination;
            set => pagination = value;
        }
        public bool ToggleNavigation
        {
            get => useToggleNavigation;
            set => useToggleNavigation = value;
        }
        public SnapTarget SnapTarget
        {
            get => snapTarget;
            set => snapTarget = value;
        }
        public float SnapSpeed
        {
            get => snapSpeed;
            set => snapSpeed = value;
        }
        public float ThresholdSpeedToSnap
        {
            get => thresholdSpeedToSnap;
            set => thresholdSpeedToSnap = value;
        }
        public bool UseHardSnapping
        {
            get => useHardSnapping;
            set => useHardSnapping = value;
        }
        public bool UseUnscaledTime
        {
            get => useUnscaledTime;
            set => useUnscaledTime = value;
        }
        public UnityEvent<GameObject, float> OnTransitionEffects
        {
            get => onTransitionEffects;
        }
        public UnityEvent<int> OnPanelSelecting
        {
            get => onPanelSelecting;
        }
        public UnityEvent<int> OnPanelSelected
        {
            get => onPanelSelected;
        }
        public UnityEvent<int, int> OnPanelCentering
        {
            get => onPanelCentering;
        }
        public UnityEvent<int, int> OnPanelCentered
        {
            get => onPanelCentered;
        }

        public RectTransform Content
        {
            get => ScrollRect.content;
        }
        public RectTransform Viewport
        {
            get => ScrollRect.viewport;
        }
        public RectTransform RectTransform
        {
            get => transform as RectTransform;
        }
        public ScrollRect ScrollRect
        {
            get
            {
                if (scrollRect == null)
                {
                    scrollRect = GetComponent<ScrollRect>();
                }
                return scrollRect;
            }
        }
        public int NumberOfPanels
        {
            get => Content.childCount;
        }
        private bool ValidConfig
        {
            get
            {
                bool valid = true;

                if (pagination != null)
                {
                    int numberOfToggles = pagination.transform.childCount;
                    if (numberOfToggles != NumberOfPanels)
                    {
                        Debug.LogError("<b>[SimpleScrollSnap]</b> The number of Toggles should be equivalent to the number of Panels. There are currently " + numberOfToggles + " Toggles and " + NumberOfPanels + " Panels. If you are adding Panels dynamically during runtime, please update your pagination to reflect the number of Panels you will have before adding.", gameObject);
                        valid = false;
                    }
                }
                if (snapSpeed < 0)
                {
                    Debug.LogError("<b>[SimpleScrollSnap]</b> Snapping speed cannot be negative.", gameObject);
                    valid = false;
                }

                return valid;
            }
        }
        public Vector2 Velocity
        {
            get => velocity;
            set
            {
                ScrollRect.velocity = velocity = value;
                isSelected = false;
            }
        }

        public RectTransform[] Panels
        {
            get;
            private set;
        }
        public Toggle[] Toggles
        {
            get;
            private set;
        }
        public int SelectedPanel
        {
            get;
            private set;
        }
        public int CenteredPanel
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        private void Start()
        {
            if (ValidConfig)
            {
                Setup();
            }
            else
            {
                throw new Exception("Invalid configuration.");
            }
        }
        private void Update()
        {
            if (NumberOfPanels == 0) return;

            HandleOcclusionCulling();
            HandleSelectingAndSnapping();
            HandleInfiniteScrolling();
            HandleTransitionEffects();
            HandleSwipeGestures();

            GetVelocity();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            isPressing = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isPressing = false;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (isDragging && onPanelSelecting.GetPersistentEventCount() > 0)
            {
                onPanelSelecting.Invoke(GetNearestPanel());
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (useHardSnapping)
            {
                ScrollRect.inertia = true;
            }

            isSelected = false;
            isDragging = true;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;

            switch (movementAxis)
            {
                case MovementAxis.Horizontal:
                    releaseDirection = (Velocity.x > 0) ? Direction.Right : Direction.Left;
                    break;
                case MovementAxis.Vertical:
                    releaseDirection = (Velocity.y > 0) ? Direction.Up : Direction.Down;
                    break;
            }
            releaseSpeed = Velocity.magnitude;
        }

        private void Setup()
        {
            if (NumberOfPanels == 0) return;

            // Scroll-Rect
            ScrollRect.horizontal = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Horizontal);
            ScrollRect.vertical =   (movementType == MovementType.Free) || (movementAxis == MovementAxis.Vertical);

            // Panels
            if (sizeControl == SizeControl.Fit)
            {
                size = Viewport.rect.size;
            }
            Panels = new RectTransform[NumberOfPanels];
            for (int i = 0; i < NumberOfPanels; i++)
            {
                Panels[i] = Content.GetChild(i) as RectTransform;
                if (movementType == MovementType.Fixed && useAutomaticLayout)
                {
                    Panels[i].anchorMin = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f);
                    Panels[i].anchorMax = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f);

                    float x = (automaticLayoutMargins.Right + automaticLayoutMargins.Left)   / 2f - automaticLayoutMargins.Left;
                    float y = (automaticLayoutMargins.Top   + automaticLayoutMargins.Bottom) / 2f - automaticLayoutMargins.Bottom;
                    Vector2 marginOffset = new Vector2(x / size.x, y / size.y);
                    Panels[i].pivot = new Vector2(0.5f, 0.5f) + marginOffset;
                    Panels[i].sizeDelta = size - new Vector2(automaticLayoutMargins.Left + automaticLayoutMargins.Right, automaticLayoutMargins.Top + automaticLayoutMargins.Bottom);

                    float panelPosX = (movementAxis == MovementAxis.Horizontal) ? i * (automaticLayoutSpacing + 1f) * size.x + (size.x / 2f) : 0f;
                    float panelPosY = (movementAxis == MovementAxis.Vertical)   ? i * (automaticLayoutSpacing + 1f) * size.y + (size.y / 2f) : 0f;
                    Panels[i].anchoredPosition = new Vector2(panelPosX, panelPosY);
                }
            }

            // Content
            if (movementType == MovementType.Fixed)
            {
                // Automatic Layout
                if (useAutomaticLayout)
                {
                    Content.anchorMin = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f);
                    Content.anchorMax = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f);
                    Content.pivot = new Vector2(movementAxis == MovementAxis.Horizontal     ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f);

                    Vector2 min = Panels[0].anchoredPosition;
                    Vector2 max = Panels[NumberOfPanels - 1].anchoredPosition;

                    float contentWidth  = (movementAxis == MovementAxis.Horizontal) ? (NumberOfPanels * (automaticLayoutSpacing + 1f) * size.x) - (size.x * automaticLayoutSpacing) : size.x;
                    float contentHeight = (movementAxis == MovementAxis.Vertical)   ? (NumberOfPanels * (automaticLayoutSpacing + 1f) * size.y) - (size.y * automaticLayoutSpacing) : size.y;
                    Content.sizeDelta = new Vector2(contentWidth, contentHeight);
                }

                // Infinite Scrolling
                if (useInfiniteScrolling)
                {
                    ScrollRect.movementType = ScrollRect.MovementType.Unrestricted;
                    contentSize = Content.rect.size + (size * infiniteScrollingSpacing);
                    HandleInfiniteScrolling(true);
                }

                // Occlusion Culling
                if (useOcclusionCulling)
                {
                    HandleOcclusionCulling(true);
                }
            }
            else
            {
                useAutomaticLayout = useInfiniteScrolling = useOcclusionCulling = false;
            }

            // Starting Panel
            float xOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Horizontal) ? Viewport.rect.width  / 2f : 0f;
            float yOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Vertical)   ? Viewport.rect.height / 2f : 0f;
            Vector2 offset = new Vector2(xOffset, yOffset);
            prevAnchoredPosition = Content.anchoredPosition = -Panels[startingPanel].anchoredPosition + offset;
            SelectedPanel = CenteredPanel = startingPanel;

            // Buttons
            if (previousButton != null)
            {
                previousButton.onClick.AddListenerOnce(GoToPreviousPanel);
            }
            if (nextButton != null)
            {
                nextButton.onClick.AddListenerOnce(GoToNextPanel);
            }

            // Pagination
            if (pagination != null && NumberOfPanels != 0)
            {
                Toggles = pagination.GetComponentsInChildren<Toggle>();
                Toggles[startingPanel].SetIsOnWithoutNotify(true);
                for (int i = 0; i < Toggles.Length; i++)
                {
                    int panelNumber = i;
                    Toggles[i].onValueChanged.AddListenerOnce(delegate (bool isOn)
                    {
                        if (isOn && useToggleNavigation)
                        {
                            GoToPanel(panelNumber);
                        }
                    });
                }
            }
        }

        private void HandleSelectingAndSnapping()
        {
            if (isSelected)
            {
                if (!((isDragging || isPressing) && useSwipeGestures))
                {
                    SnapToPanel();
                }
            }
            else if (!isDragging && (ScrollRect.velocity.magnitude <= thresholdSpeedToSnap || thresholdSpeedToSnap == -1f))
            {
                SelectPanel();
            }
        }
        private void HandleOcclusionCulling(bool forceUpdate = false)
        {
            if (useOcclusionCulling && (Velocity.magnitude > 0f || forceUpdate))
            {
                for (int i = 0; i < NumberOfPanels; i++)
                {
                    switch (movementAxis)
                    {
                        case MovementAxis.Horizontal:
                            Panels[i].gameObject.SetActive(Mathf.Abs(GetDisplacementFromCenter(i).x) <= Viewport.rect.width  / 2f + size.x);
                            break;
                        case MovementAxis.Vertical:
                            Panels[i].gameObject.SetActive(Mathf.Abs(GetDisplacementFromCenter(i).y) <= Viewport.rect.height / 2f + size.y);
                            break;
                    }
                }
            }
        }
        private void HandleInfiniteScrolling(bool forceUpdate = false)
        {
            if (useInfiniteScrolling && (Velocity.magnitude > 0 || forceUpdate))
            {
                switch (movementAxis)
                {
                    case MovementAxis.Horizontal:
                        for (int i = 0; i < NumberOfPanels; i++)
                        {
                            Vector2 offset = new Vector2(contentSize.x, 0);
                            if (GetDisplacementFromCenter(i).x > Content.rect.width /  2f)
                            {
                                Panels[i].anchoredPosition -= offset;
                            }
                            else
                            if (GetDisplacementFromCenter(i).x < Content.rect.width / -2f)
                            {
                                Panels[i].anchoredPosition += offset;
                            }
                        }
                        break;
                    case MovementAxis.Vertical:
                        for (int i = 0; i < NumberOfPanels; i++)
                        {
                            Vector2 offset = new Vector2(0, contentSize.y);
                            if (GetDisplacementFromCenter(i).y > Content.rect.height /  2f)
                            {
                                Panels[i].anchoredPosition -= offset;
                            }
                            else
                            if (GetDisplacementFromCenter(i).y < Content.rect.height / -2f)
                            {
                                Panels[i].anchoredPosition += offset;
                            }
                        }
                        break;
                }
            }
        }
        private void HandleSwipeGestures()
        {
            if (useSwipeGestures)
            {
                ScrollRect.horizontal = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Horizontal);
                ScrollRect.vertical   = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Vertical);
            }
            else
            {
                ScrollRect.horizontal = ScrollRect.vertical = !isDragging;
            }
        }
        private void HandleTransitionEffects()
        {
            if (onTransitionEffects.GetPersistentEventCount() == 0) return;

            for (int i = 0; i < NumberOfPanels; i++)
            {
                Vector2 displacement = GetDisplacementFromCenter(i);
                float d = (movementType == MovementType.Free) ? displacement.magnitude : ((movementAxis == MovementAxis.Horizontal) ? displacement.x : displacement.y);
                onTransitionEffects.Invoke(Panels[i].gameObject, d);
            }
        }

        private void SelectPanel()
        {
            int nearestPanel = GetNearestPanel();
            Vector2 displacementFromCenter = GetDisplacementFromCenter(nearestPanel);

            if (snapTarget == SnapTarget.Nearest || releaseSpeed <= minimumSwipeSpeed)
            {
                GoToPanel(nearestPanel);
            }
            else 
            if (snapTarget == SnapTarget.Previous)
            {
                if ((releaseDirection == Direction.Right && displacementFromCenter.x < 0f) || (releaseDirection == Direction.Up   && displacementFromCenter.y < 0f))
                {
                    GoToNextPanel();
                }
                else 
                if ((releaseDirection == Direction.Left  && displacementFromCenter.x > 0f) || (releaseDirection == Direction.Down && displacementFromCenter.y > 0f))
                {
                    GoToPreviousPanel();
                }
                else
                {
                    GoToPanel(nearestPanel);
                }
            }
            else 
            if (snapTarget == SnapTarget.Next)
            {
                if ((releaseDirection == Direction.Right && displacementFromCenter.x > 0f) || (releaseDirection == Direction.Up   && displacementFromCenter.y > 0f))
                {
                    GoToPreviousPanel();
                }
                else 
                if ((releaseDirection == Direction.Left  && displacementFromCenter.x < 0f) || (releaseDirection == Direction.Down && displacementFromCenter.y < 0f))
                {
                    GoToNextPanel();
                }
                else
                {
                    GoToPanel(nearestPanel);
                }
            }
        }
        private void SnapToPanel()
        {
            float xOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Horizontal) ? Viewport.rect.width  / 2f : 0f;
            float yOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Vertical)   ? Viewport.rect.height / 2f : 0f;
            Vector2 offset = new Vector2(xOffset, yOffset);

            Vector2 targetPosition = -Panels[CenteredPanel].anchoredPosition + offset;
            Content.anchoredPosition = Vector2.Lerp(Content.anchoredPosition, targetPosition, (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) * snapSpeed);

            if (SelectedPanel != CenteredPanel)
            {
                if (GetDisplacementFromCenter(CenteredPanel).magnitude < (Viewport.rect.width / 10f))
                {
                    onPanelCentered.Invoke(CenteredPanel, SelectedPanel);
                    SelectedPanel = CenteredPanel;
                }
            }
            else
            {
                onPanelCentering.Invoke(CenteredPanel, SelectedPanel);
            }
        }

        public void GoToPanel(int panelNumber)
        {
            CenteredPanel = panelNumber;
            isSelected = true;
            onPanelSelected.Invoke(SelectedPanel);

            if (pagination != null)
            {
                Toggles[panelNumber].isOn = true;
            }
            if (useHardSnapping)
            {
                ScrollRect.inertia = false;
            }
        }
        public void GoToPreviousPanel()
        {
            int nearestPanel = GetNearestPanel();
            if (nearestPanel != 0)
            {
                GoToPanel(nearestPanel - 1);
            }
            else
            {
                if (useInfiniteScrolling)
                {
                    GoToPanel(NumberOfPanels - 1);
                }
                else
                {
                    GoToPanel(nearestPanel);
                }
            }
        }
        public void GoToNextPanel()
        {
            int nearestPanel = GetNearestPanel();
            if (nearestPanel != (NumberOfPanels - 1))
            {
                GoToPanel(nearestPanel + 1);
            }
            else
            {
                if (useInfiniteScrolling)
                {
                    GoToPanel(0);
                }
                else
                {
                    GoToPanel(nearestPanel);
                }
            }
        }

        public void AddToFront(GameObject panel)
        {
            Add(panel, 0);
        }
        public void AddToBack(GameObject panel)
        {
            Add(panel, NumberOfPanels);
        }
        public void Add(GameObject panel, int index)
        {
            if (NumberOfPanels != 0 && (index < 0 || index > NumberOfPanels))
            {
                Debug.LogError("<b>[SimpleScrollSnap]</b> Index must be an integer from 0 to " + NumberOfPanels + ".", gameObject);
                return;
            }
            else if (!useAutomaticLayout)
            {
                Debug.LogError("<b>[SimpleScrollSnap]</b> \"Automatic Layout\" must be enabled for content to be dynamically added during runtime.");
                return;
            }

            panel = Instantiate(panel, Content, false);
            panel.transform.SetSiblingIndex(index);

            if (ValidConfig)
            {
                if (CenteredPanel <= index)
                {
                    startingPanel = CenteredPanel;
                }
                else
                {
                    startingPanel = CenteredPanel + 1;
                }
                Setup();
            }
        }
        public void RemoveFromFront()
        {
            Remove(0);
        }
        public void RemoveFromBack()
        {
            if (NumberOfPanels > 0)
            {
                Remove(NumberOfPanels - 1);
            }
            else
            {
                Remove(0);
            }
        }
        public void Remove(int index)
        {
            if (NumberOfPanels == 0)
            {
                Debug.LogError("<b>[SimpleScrollSnap]</b> There are no panels to remove.", gameObject);
                return;
            }
            else if (index < 0 || index > (NumberOfPanels - 1))
            {
                Debug.LogError("<b>[SimpleScrollSnap]</b> Index must be an integer from 0 to " + (NumberOfPanels - 1) + ".", gameObject);
                return;
            }
            else if (!useAutomaticLayout)
            {
                Debug.LogError("<b>[SimpleScrollSnap]</b> \"Automatic Layout\" must be enabled for content to be dynamically removed during runtime.");
                return;
            }

            DestroyImmediate(Panels[index].gameObject);

            if (ValidConfig)
            {
                if (CenteredPanel == index)
                {
                    if (index == NumberOfPanels)
                    {
                        startingPanel = CenteredPanel - 1;
                    }
                    else
                    {
                        startingPanel = CenteredPanel;
                    }
                }
                else if (CenteredPanel < index)
                {
                    startingPanel = CenteredPanel;
                }
                else
                {
                    startingPanel = CenteredPanel - 1;
                }
                Setup();
            }
        }

        private Vector2 GetDisplacementFromCenter(int index)
        {
            return Panels[index].anchoredPosition + Content.anchoredPosition - new Vector2(Viewport.rect.width * (0.5f - Content.anchorMin.x), Viewport.rect.height * (0.5f - Content.anchorMin.y));
        }
        private int GetNearestPanel()
        {
            float[] distances = new float[NumberOfPanels];
            for (int i = 0; i < Panels.Length; i++)
            {
                distances[i] = GetDisplacementFromCenter(i).magnitude;
            }

            int nearestPanel = 0;
            float minDistance = Mathf.Min(distances);
            for (int i = 0; i < Panels.Length; i++)
            {
                if (minDistance == distances[i])
                {
                    nearestPanel = i;
                    break;
                }
            }
            return nearestPanel;
        }
        private void GetVelocity()
        {
            Vector2 displacement = Content.anchoredPosition - prevAnchoredPosition;
            float time = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            velocity = displacement / time;
            prevAnchoredPosition = Content.anchoredPosition;
        }
        #endregion
    }
}