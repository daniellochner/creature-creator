// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureCamera))]
    [RequireComponent(typeof(CreatureSpeedEffects))]
    public class CreaturePlayerLocal : CreaturePlayer, ISetupable
    {
        #region Fields
        [SerializeField] private CreatureEditor editor;
        [SerializeField] private CreatureAbilities abilities;
        [SerializeField] private CreatureMover mover;
        [SerializeField] private CreatureInteractor interactor;
        [SerializeField] private new CreatureCamera camera;
        [SerializeField] private CreatureSpeedEffects speedEffects;
        #endregion

        #region Properties
        public CreatureEditor Editor => editor;
        public CreatureAbilities Abilities => abilities;
        public CreatureMover Mover => mover;
        public CreatureInteractor Interactor => interactor;
        public CreatureCamera Camera => camera;
        public CreatureSpeedEffects SpeedEffects => speedEffects;

        public bool IsSetup { get; set; }
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            editor = GetComponent<CreatureEditor>();
            abilities = GetComponent<CreatureAbilities>();
            mover = GetComponent<CreatureMover>();
            interactor = GetComponent<CreatureInteractor>();
            camera = GetComponent<CreatureCamera>();
            speedEffects = GetComponent<CreatureSpeedEffects>();
        }
#endif

        public override void Setup()
        {
            Camera.Setup();
            SpeedEffects.Setup();
            base.Setup();
            Editor.Setup();
            Interactor.Setup();

            IsSetup = true;
        }

        public override void OnDie()
        {
            base.OnDie();

            Constructor.Body.gameObject.SetActive(false);
            Rigidbody.isKinematic = true;

            Animator.enabled = false;

            Abilities.enabled = false;
            Mover.enabled = false;
            Interactor.enabled = false;

            Spawner.Despawn();

            EditorManager.Instance.SetVisibility(false);

            EditorManager.Instance.Invoke(delegate
            {
                string name = Informer.Information.Name.Equals("Unnamed") ? "You" : Informer.Information.Name;
                string age = Informer.Information.FormattedAge;
                InformationDialog.Inform("You Died!", $"{name} died after {age}. Press the button below to respawn at your previous editing platform.", "Respawn", false, Respawn);

            }, 1f);
        }

        private void Respawn()
        {
            ZoneManager.Instance.ExitCurrentZone(Editor.Platform.Position);

            Mover.Teleport(Editor.Platform);

            Constructor.Body.gameObject.SetActive(true);
            Rigidbody.isKinematic = false;
            transform.parent = null;

            Collider.enabled = true;

            Animator.enabled = true;

            Abilities.enabled = true;
            Mover.enabled = true;
            Interactor.enabled = true;

            Spawner.Spawn();

            EditorManager.Instance.SetVisibility(true);
        }
        #endregion
    }
}