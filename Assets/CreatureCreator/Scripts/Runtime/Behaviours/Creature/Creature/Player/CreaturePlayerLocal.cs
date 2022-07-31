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
        [SerializeField] private CreatureAbilities abilities;
        [SerializeField] private CreatureMover mover;
        [SerializeField] private CreatureInteractor interactor;
        [SerializeField] private new CreatureCamera camera;
        #endregion

        #region Properties
        public CreatureEditor Editor => editor;
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

            //Animator.SetDamping(false);
            //Animator.IsAnimated = false;

            Spawner.Despawn();

            EditorManager.Instance.IsVisible = false;

            EditorManager.Instance.Invoke(delegate
            {
                string name = Informer.Information.Name.Equals("Unnamed") ? "You" : Informer.Information.Name;
                string age = Informer.Information.FormattedAge;
                InformationDialog.Inform("You Died!", $"{name} died after {age}. Press the button below to respawn at your previous editing platform.", "Respawn", false, Respawn);

            }, 1f);
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


        private void Respawn()
        {
            Mover.Teleport(Editor.Platform);


            // set these in creature player local rather...
            //Animator.RestoreDefaults();
            //Animator.Restructure(false);
            //Animator.IsAnimated = false;
            //Animator.SetDamping(false);


            //Animator.Restructure(true);
            //Animator.Rebuild();
            //Animator.IsAnimated = true;
            //Animator.SetDamping(true);
            Spawner.Spawn();

            EditorManager.Instance.IsVisible = true;
        }
        #endregion
    }
}