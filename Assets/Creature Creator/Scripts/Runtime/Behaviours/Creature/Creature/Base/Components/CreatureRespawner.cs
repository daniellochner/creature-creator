// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    [RequireComponent(typeof(CreatureAnimator))]
    [RequireComponent(typeof(CreatureHealth))]
    [RequireComponent(typeof(CreatureInformer))]
    [RequireComponent(typeof(CreatureMover))]
    [RequireComponent(typeof(CreatureSpawner))]
    public class CreatureRespawner : MonoBehaviour
    {
        #region Properties
        public CreatureConstructor Constructor { get; private set; }
        public CreatureAnimator Animator { get; private set; }
        public CreatureHealth Health { get; private set; }
        public CreatureInformer Informer { get; private set; }
        public CreatureMover Mover { get; private set; }
        public CreatureSpawner Spawner { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Animator = GetComponent<CreatureAnimator>();
            Health = GetComponent<CreatureHealth>();
            Informer = GetComponent<CreatureInformer>();
            Mover = GetComponent<CreatureMover>();
            Spawner = GetComponent<CreatureSpawner>();
        }

        private void Start()
        {
            Health.OnDie += OnDie;
        }

        public void OnDie()
        {
            Animator.Animator.enabled = false;
            Spawner.Despawn();

            EditorManager.Instance.IsVisible = false;

            EditorManager.Instance.Invoke(delegate
            {
                string name = Informer.Information.Name.Equals("Unnamed") ? "You" : Informer.Information.Name;
                string age = Informer.Information.FormattedAge;
                InformationDialog.Inform("You Died!", $"{name} died after {age}. Press the button below to respawn at your previous editing platform.", "Respawn", false, Respawn);

            }, 1f);
        }
        private void Respawn()
        {
            Mover.Teleport(Mover.Platform);

            Animator.RestoreDefaults();
            Animator.Restructure(false);
            Animator.IsAnimated = false;

            Animator.Restructure(true);
            Animator.Rebuild();
            Animator.IsAnimated = true;
            Spawner.Spawn();

            EditorManager.Instance.IsVisible = true;
        }
        #endregion
    }
}