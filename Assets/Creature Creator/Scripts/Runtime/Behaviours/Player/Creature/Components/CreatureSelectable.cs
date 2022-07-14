// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    [RequireComponent(typeof(CreatureInformer))]
    public class CreatureSelectable : CreatureInteractable
    {
        #region Fields
        [SerializeField] private CreatureInformationMenu informationMenuPrefab;
        [SerializeField] private GameObject selectCirclePrefab;

        private CreatureInformationMenu informationMenu;
        private GameObject informationSelectCircle;
        #endregion

        #region Properties
        public CreatureConstructor Constructor { get; private set; }
        public CreatureCollider Collider { get; private set; }
        public CreatureInformer Informer { get; private set; }

        public bool IsBehindPlayer
        {
            get => Player.Instance.Creature.Interactor.InteractionCamera.WorldToViewportPoint(transform.position).z <= 0;
        }
        public bool IsSelected
        {
            get; private set;
        }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }
        private void LateUpdate()
        {
            if (Player.Instance)
            {
                HandlePosition();
                HandleVisibility();
            }
        }
        private void OnDisable()
        {
            if (Application.isPlaying)
            {
                SetSelected(false, true);
            }
        }

        private void Initialize()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Collider = GetComponent<CreatureCollider>();
            Informer = GetComponent<CreatureInformer>();
        }

        public void Setup()
        {
            informationMenu = Instantiate(informationMenuPrefab, Dynamic.OverlayCanvas);
            Informer.Setup(informationMenu);
        }

        private void HandlePosition()
        {
            if (informationMenu != null && informationMenu.IsVisible)
            {
                Vector3 position = transform.position + transform.up * Collider.Height;
                informationMenu.transform.position = RectTransformUtility.WorldToScreenPoint(Player.Instance.Creature.Interactor.InteractionCamera, position);
            }
        }
        private void HandleVisibility()
        {
            if (informationMenu != null && IsSelected)
            {
                if (informationMenu.IsOpen && IsBehindPlayer)
                {
                    informationMenu.Close(true);
                }
                else if (!informationMenu.IsOpen && !IsBehindPlayer)
                {
                    informationMenu.Open(true);
                }
            }
        }

        protected override void OnHighlight(Interactor interactor, bool isHighlighted)
        {
            if (isHighlighted)
            {
                informationMenu.Open();
            }
            else if (!IsSelected)
            {
                informationMenu.Close();
            }
        }
        protected override void OnInteract(Interactor interactor)
        {
            SetSelected(!IsSelected);
        }

        public override bool CanInteract(Interactor interactor)
        {
            return (base.CanInteract(interactor) && !IsBehindPlayer) || IsSelected;
        }

        public void SetSelected(bool isSelected, bool instant = false)
        {
            if (IsSelected != isSelected)
            {
                IsSelected = isSelected;
            }
            else return;

            if (isSelected)
            {
                informationSelectCircle = Instantiate(selectCirclePrefab, transform.position, transform.rotation, transform);
            }
            else
            {
                Destroy(informationSelectCircle);
                if (!IsHighlighted)
                {
                    informationMenu.Close(instant);
                }
            }
        }
        #endregion
    }
}