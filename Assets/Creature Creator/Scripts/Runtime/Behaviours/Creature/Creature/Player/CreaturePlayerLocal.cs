// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureCamera))]
    public class CreaturePlayerLocal : CreaturePlayer, ISetupable
    {
        #region Fields
        [SerializeField] private CreatureEditor editor;
        [SerializeField] private CreatureRespawner respawner;
        [SerializeField] private CreatureAbilities abilities;
        [SerializeField] private CreatureMover mover;
        [SerializeField] private CreatureInteractor interactor;
        [SerializeField] private new CreatureCamera camera;
        #endregion

        #region Properties
        public CreatureEditor Editor => editor;
        public CreatureRespawner Respawner => respawner;
        public CreatureAbilities Abilities => abilities;
        public CreatureMover Mover => mover;
        public CreatureInteractor Interactor => interactor;
        public CreatureCamera Camera => camera;

        public bool IsSetup { get; set; }
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            editor = GetComponent<CreatureEditor>();
            respawner = GetComponent<CreatureRespawner>();
            abilities = GetComponent<CreatureAbilities>();
            mover = GetComponent<CreatureMover>();
            interactor = GetComponent<CreatureInteractor>();
            camera = GetComponent<CreatureCamera>();
        }
#endif

        public override void Setup()
        {
            Camera.Setup();
            base.Setup();
            Editor.Setup();
            Interactor.Setup();

            IsSetup = true;
        }

        public override void OnDie()
        {
            base.OnDie();

            Abilities.enabled = false;
            Mover.enabled = false;
            Interactor.enabled = false;
            Rigidbody.isKinematic = true;
        }
        public override void OnSpawn()
        {
            base.OnSpawn();

            Constructor.Body.gameObject.SetActive(true);

            Abilities.enabled = true;
            Mover.enabled = true;
            Interactor.enabled = true;

            Collider.enabled = true;
            Rigidbody.isKinematic = false;
        }
        public override void OnDespawn()
        {
            base.OnDespawn();
        }
        #endregion
    }
}