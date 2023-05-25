// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    [RequireComponent(typeof(CreatureAnimator))]
    [RequireComponent(typeof(CreatureCollider))]
    [RequireComponent(typeof(CreatureCloner))]
    [RequireComponent(typeof(CreaturePhotographer))]
    [RequireComponent(typeof(CreatureCorpse))]
    [RequireComponent(typeof(CreatureScaler))]
    [RequireComponent(typeof(CreatureHealth))]
    [RequireComponent(typeof(CreatureHunger))]
    [RequireComponent(typeof(CreatureAge))]
    [RequireComponent(typeof(CreatureLoader))]
    [RequireComponent(typeof(CreatureInformer))]
    [RequireComponent(typeof(CreatureSpawner))]
    [RequireComponent(typeof(PlayerEffects))]
    [RequireComponent(typeof(CreatureHolder))]
    [RequireComponent(typeof(CreatureComparer))]
    [RequireComponent(typeof(CreatureUnderwater))]
    [RequireComponent(typeof(MinimapIcon))]
    [RequireComponent(typeof(CreatureLauncher))]
    [RequireComponent(typeof(CreatureOptimizer))]
    public class CreatureBase : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private CreatureConstructor constructor;
        [SerializeField] private CreatureAnimator animator;
        [SerializeField] private new CreatureCollider collider;
        [SerializeField] private CreatureCloner cloner;
        [SerializeField] private CreaturePhotographer photographer;
        [SerializeField] private CreatureCorpse corpse;
        [SerializeField] private PlayerEffects effects;
        [SerializeField] private CreatureScaler scaler;
        [SerializeField] private CreatureHealth health;
        [SerializeField] private CreatureHunger hunger;
        [SerializeField] private CreatureAge age;
        [SerializeField] private CreatureLoader loader;
        [SerializeField] private CreatureInformer informer;
        [SerializeField] private CreatureSpawner spawner;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private CreatureVelocity velocity;
        [SerializeField] private CreatureHolder holder;
        [SerializeField] private CreatureComparer comparer;
        [SerializeField] private CreatureUnderwater underwater;
        [SerializeField] private CreatureSpeedup speedUp;
        [SerializeField] private CreatureGrounded grounded;
        [SerializeField] private MinimapIcon minimapIcon;
        [SerializeField] private CreatureLauncher launcher;
        [SerializeField] private CreatureOptimizer optimizer;
        #endregion

        #region Properties
        public CreatureConstructor Constructor => constructor;
        public CreatureAnimator Animator => animator;
        public CreatureCollider Collider => collider;
        public CreatureCloner Cloner => cloner;
        public CreaturePhotographer Photographer => photographer;
        public CreatureCorpse Corpse => corpse;
        public CreatureScaler Scaler => scaler;
        public CreatureHealth Health => health;
        public CreatureHunger Hunger => hunger;
        public CreatureAge Age => age;
        public CreatureLoader Loader => loader;
        public CreatureInformer Informer => informer;
        public CreatureSpawner Spawner => spawner;
        public PlayerEffects Effects => effects;
        public Rigidbody Rigidbody => rb;
        public CreatureVelocity Velocity => velocity;
        public CreatureHolder Holder => holder;
        public CreatureComparer Comparer => comparer;
        public CreatureUnderwater Underwater => underwater;
        public CreatureSpeedup SpeedUp => speedUp;
        public CreatureGrounded Grounded => grounded;
        public MinimapIcon MinimapIcon => minimapIcon;
        public CreatureLauncher Launcher => launcher;
        public CreatureOptimizer Optimizer => optimizer;

        public static List<CreatureBase> Creatures { get; set; } = new List<CreatureBase>();
        #endregion

        #region Methods
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            constructor = GetComponent<CreatureConstructor>();
            animator = GetComponent<CreatureAnimator>();
            collider = GetComponent<CreatureCollider>();
            photographer = GetComponent<CreaturePhotographer>();
            cloner = GetComponent<CreatureCloner>();
            corpse = GetComponent<CreatureCorpse>();
            effects = GetComponent<PlayerEffects>();
            scaler = GetComponent<CreatureScaler>();
            health = GetComponent<CreatureHealth>();
            hunger = GetComponent<CreatureHunger>();
            age = GetComponent<CreatureAge>();
            loader = GetComponent<CreatureLoader>();
            informer = GetComponent<CreatureInformer>();
            spawner = GetComponent<CreatureSpawner>();
            rb = GetComponent<Rigidbody>();
            velocity = GetComponent<CreatureVelocity>();
            holder = GetComponent<CreatureHolder>();
            comparer = GetComponent<CreatureComparer>();
            underwater = GetComponent<CreatureUnderwater>();
            speedUp = GetComponent<CreatureSpeedup>();
            grounded = GetComponent<CreatureGrounded>();
            minimapIcon = GetComponent<MinimapIcon>();
            launcher = GetComponent<CreatureLauncher>();
            optimizer = GetComponent<CreatureOptimizer>();
        }
#endif
        private void OnEnable()
        {
            Creatures.Add(this);
        }
        private void OnDisable()
        {
            Creatures.Remove(this);
        }

        public virtual void Setup()
        {
            Animator.Setup();
            Launcher.Setup();

            MinimapIcon.enabled = true;

            Spawner.OnSpawn += OnSpawn;
            Spawner.OnDespawn += OnDespawn;
            Loader.OnShow += OnShow;
            Loader.OnHide += OnHide;
            Health.OnDie += OnDie;
        }

        public virtual void OnDie()
        {
            Collider.enabled = false;
            MinimapIcon.enabled = false;
        }
        public virtual void OnSpawn()
        {
            MinimapIcon.enabled = true;
            Launcher.enabled = true;
        }
        public virtual void OnDespawn()
        {
            Launcher.enabled = false;
        }
        public virtual void OnHide()
        {
        }
        public virtual void OnShow()
        {
        }
        #endregion
    }
}