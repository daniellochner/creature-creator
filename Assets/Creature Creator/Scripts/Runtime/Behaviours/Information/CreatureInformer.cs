// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    [RequireComponent(typeof(CreaturePhotographer))]
    public class CreatureInformer : CreatureInteractable
    {
        #region Fields
        [SerializeField] private CreatureInformationMenu informationMenuPrefab;
        [SerializeField] private GameObject informationSelectCirclePrefab;

        [Header("Debug")]
        [SerializeField, ReadOnly] private CreatureInformation information;

        private CreatureInformationMenu informationMenu;
        private GameObject informationSelectCircle;
        private bool isSelected = false;
        #endregion

        #region Properties
        public CreatureConstructor CreatureConstructor { get; private set; }
        public CreatureCollider CreatureCollider { get; private set; }
        public CreaturePhotographer CreaturePhotographer { get; private set; }

        public CreatureInformation Information => information;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                }
                else return;

                if (isSelected)
                {
                    informationSelectCircle = Instantiate(informationSelectCirclePrefab, transform.position, transform.rotation * Quaternion.Euler(90f, 0, 0), transform);
                }
                else
                {
                    Destroy(informationSelectCircle);
                    if (!IsHighlighted) informationMenu.Close();
                }
            }
        }
        public bool IsBehindPlayer
        {
            get => Player.Instance.Creature.Interactor.InteractionCamera.WorldToViewportPoint(transform.position).z <= 0;
        }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
            Setup();
        }
        private void Update()
        {
            if (!Player.Instance) return;

            HandlePosition();
            HandleVisibility();
        }
        private void OnDisable()
        {
            IsSelected = false;
        }

        private void Initialize()
        {
            CreatureConstructor = GetComponent<CreatureConstructor>();
            CreatureCollider = GetComponent<CreatureCollider>();
            CreaturePhotographer = GetComponent<CreaturePhotographer>();
        }
        private void Setup()
        {
            CreatureConstructor.OnConstructCreature += delegate
            {
                CreaturePhotographer.TakePhoto(128, (Texture2D p) => Information.Photo = p);
                Information.Name = CreatureConstructor.Data.Name;
            };

            informationMenu = Instantiate(informationMenuPrefab, Dynamic.OverlayCanvas);
            informationMenu.Setup(Information);
        }

        private void HandlePosition()
        {
            if (!IsBehindPlayer)
            {
                Vector3 position = transform.position + transform.up * CreatureCollider.Height;
                informationMenu.transform.position = RectTransformUtility.WorldToScreenPoint(Player.Instance.Creature.Interactor.InteractionCamera, position);
            }
        }
        private void HandleVisibility()
        {
            if (!CanInteract(Player.Instance.Creature.Interactor) && !IsBehindPlayer && IsSelected)
            {
                IsSelected = false;
            }
            else
            {
                if (IsBehindPlayer && informationMenu.IsOpen)
                {
                    informationMenu.Close(true);
                }
                else
                if (!IsBehindPlayer && !informationMenu.IsOpen && IsSelected)
                {
                    informationMenu.Open(true);
                }
            }
        }

        protected override void OnHighlight(bool isHighlighted)
        {
            if (isHighlighted)
            {
                informationMenu.Open();
            }
            else if (!isSelected)
            {
                informationMenu.Close();
            }
        }
        protected override void OnInteract()
        {
            IsSelected = !IsSelected;
        }

        public override bool CanInteract(CreatureInteractor interactor)
        {
            return base.CanInteract(interactor) && !IsBehindPlayer;
        }
        #endregion
    }
}