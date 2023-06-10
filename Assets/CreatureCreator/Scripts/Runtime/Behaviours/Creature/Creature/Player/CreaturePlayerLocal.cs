// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureCamera))]
    [RequireComponent(typeof(CreatureSpeedEffects))]
    [RequireComponent(typeof(CreatureBuoyancy))]
    public class CreaturePlayerLocal : CreaturePlayer, ISetupable
    {
        #region Fields
        [SerializeField] private CreatureEditor editor;
        [SerializeField] private CreatureAbilities abilities;
        [SerializeField] private CreatureMover mover;
        [SerializeField] private CreatureInteractor interactor;
        [SerializeField] private new CreatureCamera camera;
        [SerializeField] private CreatureSpeedEffects speedEffects;
        [SerializeField] private CreatureBuoyancy buoyancy;
        #endregion

        #region Properties
        public CreatureEditor Editor => editor;
        public CreatureAbilities Abilities => abilities;
        public CreatureMover Mover => mover;
        public CreatureInteractor Interactor => interactor;
        public CreatureCamera Camera => camera;
        public CreatureSpeedEffects SpeedEffects => speedEffects;
        public CreatureBuoyancy Buoyancy => buoyancy;

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
            buoyancy = GetComponent<CreatureBuoyancy>();
        }
#endif

        public override void Setup()
        {
            Camera.Setup();
            SpeedEffects.Setup();
            base.Setup();
            Editor.Setup();
            Interactor.Setup();

            Age.OnAgeChanged += OnAge;

            IsSetup = true;
        }

        public override void OnDie(DamageReason reason)
        {
            base.OnDie(reason);

            Constructor.Body.gameObject.SetActive(false);
            Rigidbody.isKinematic = true;

            Animator.enabled = false;

            Abilities.enabled = false;
            Mover.enabled = false;
            Interactor.enabled = false;

            Holder.DropAll();
            Holder.enabled = false;

            Loader.HideFromOthers();

            Spawner.Despawn();

            EditorManager.Instance.SetVisibility(false);

            float time = Time.time;
            EditorManager.Instance.InvokeUntil(() => !Input.GetMouseButton(0) && !InputUtility.GetKey(KeybindingsManager.Data.Respawn) && (Time.time > (time + 1f)), delegate
            {
                string name = Informer.Information.Name.Equals(LocalizationUtility.Localize("creature-unnamed")) ? LocalizationUtility.Localize("you-died_you") : Informer.Information.Name;
                string age  = Informer.Information.FormattedAge;
                InformationDialog.Inform(LocalizationUtility.Localize("you-died_title"), LocalizationUtility.Localize("you-died_message", name, age), LocalizationUtility.Localize("you-died_okay"), false, Respawn, Respawn, Respawn);
            });

#if USE_STATS
            StatsManager.Instance.Deaths++;
#endif
        }

        private void OnAge(int age)
        {
#if USE_STATS
            if (age > 3600)
            {
                StatsManager.Instance.UnlockAchievement("ACH_GRAY_HAIRS");
            }
#endif
        }

        private void Respawn()
        {
            if (MinigameManager.Instance.CurrentMinigame != null)
            {
                Transform spawnPoint = MinigameManager.Instance.CurrentMinigame.GetSpawnPoint();
                Mover.Teleport(spawnPoint.position, spawnPoint.rotation, true);

                Player.Instance.Loader.ShowMeToOthers();
            }
            else
            {
                ZoneManager.Instance.ExitCurrentZone(Editor.Platform.Position);
                Editor.Platform.TeleportTo(false, true);
            }


            Constructor.Body.gameObject.SetActive(true);
            Rigidbody.isKinematic = false;
            transform.parent = null;

            Collider.enabled = true;

            Animator.enabled = true;

            Abilities.enabled = true;
            Mover.enabled = true;
            Interactor.enabled = true;
            Holder.enabled = true;

            Spawner.Spawn();

            EditorManager.Instance.SetVisibility(true);
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            buoyancy.enabled = false;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            buoyancy.enabled = true;
        }
        #endregion
    }
}