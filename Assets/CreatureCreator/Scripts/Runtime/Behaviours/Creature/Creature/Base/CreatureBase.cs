// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections;
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
    [RequireComponent(typeof(CreatureHider))]
    [RequireComponent(typeof(CreatureInformer))]
    [RequireComponent(typeof(CreatureSpawner))]
    [RequireComponent(typeof(PlayerEffects))]
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
        [SerializeField] private CreatureHider hider;
        [SerializeField] private CreatureInformer informer;
        [SerializeField] private CreatureSpawner spawner;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private CreatureVelocity velocity;
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
        public CreatureHider Hider => hider;
        public CreatureInformer Informer => informer;
        public CreatureSpawner Spawner => spawner;
        public PlayerEffects Effects => effects;
        public Rigidbody Rigidbody => rb;
        public CreatureVelocity Velocity => velocity;
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
            hider = GetComponent<CreatureHider>();
            informer = GetComponent<CreatureInformer>();
            spawner = GetComponent<CreatureSpawner>();
            rb = GetComponent<Rigidbody>();
            velocity = GetComponent<CreatureVelocity>();
        }
#endif

        public virtual void Setup()
        {
            Animator.Setup();

            Spawner.OnSpawn += OnSpawn;
            Spawner.OnDespawn += OnDespawn;
            Hider.OnShow += OnShow;
            Hider.OnHide += OnHide;
            Health.OnDie += OnDie;
        }

        public virtual void OnDie()
        {
            Collider.enabled = false;
        }
        public virtual void OnSpawn()
        {
        }
        public virtual void OnDespawn()
        {
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