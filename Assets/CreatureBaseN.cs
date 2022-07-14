using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    /// <summary>
    /// Cached components.
    /// </summary>
    public class CreatureBaseN : MonoBehaviour
    {
        #region Fields
        [Header("Base")]
        [SerializeField] private CreatureConstructor constructor;
        [SerializeField] private CreatureAnimator animator;
        [SerializeField] private new CreatureCollider collider;
        [SerializeField] private CreatureCloner cloner;
        [SerializeField] private CreaturePhotographer photographer;
        [SerializeField] private CreatureKiller killer;
        [SerializeField] private CreatureEffector effector;
        [SerializeField] private CreatureScaler scaler;
        [SerializeField] private CreatureHealth health;
        [SerializeField] private CreatureEnergy energy;
        [SerializeField] private CreatureAge age;
        [SerializeField] private CreatureHider hider;
        [SerializeField] private CreatureInformer informer;

        [Header("Remote")]
        [SerializeField] private CreatureSelectable selector;

        [Header("Local")]
        [SerializeField] private CreatureEnergyDepleter energyDepleter;
        [SerializeField] private CreatureAger ager;

        [Header("PLAYER")]
        [SerializeField] private CreatureEditor editor;
        [SerializeField] private CreatureHealthEditor healthEditor;
        [SerializeField] private CreatureAbilities abilities;
        [SerializeField] private CreatureMover mover;
        [SerializeField] private CreatureInteractor interactor;
        [Space]
        [SerializeField] private CameraOrbit cameraOrbit;
        #endregion

        #region Properties
        public CreatureConstructor Constructor => constructor;
        public CreatureAnimator Animator => animator;
        public CreatureCollider Collider => collider;
        public CreatureCloner Cloner => cloner;
        public CreaturePhotographer Photographer => photographer;
        public CreatureKiller Killer => killer;
        public CreatureEffector Effector => effector;
        public CreatureScaler Scaler => scaler;
        public CreatureHealth Health => health;
        public CreatureEnergy Energy => energy;
        public CreatureAge Age => age;
        public CreatureHider Hider => hider;
        public CreatureInformer Informer => informer;

        public CreatureSelectable Selector => selector;

        public CreatureEnergyDepleter EnergyDepleter => energyDepleter;
        public CreatureAger Ager => ager;

        public CreatureEditor Editor => editor;
        public CreatureHealthEditor HealthEditor => healthEditor;
        public CreatureAbilities Abilities => abilities;
        public CreatureMover Mover => mover;
        public CreatureInteractor Interactor => interactor;

        public CameraOrbit CameraOrbit => cameraOrbit;
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
            killer = GetComponent<CreatureKiller>();
            effector = GetComponent<CreatureEffector>();
            scaler = GetComponent<CreatureScaler>();
            health = GetComponent<CreatureHealth>();
            energy = GetComponent<CreatureEnergy>();
            age = GetComponent<CreatureAge>();
            hider = GetComponent<CreatureHider>();
            informer = GetComponent<CreatureInformer>();

            selector = GetComponent<CreatureSelectable>();

            energyDepleter = GetComponent<CreatureEnergyDepleter>();
            ager = GetComponent<CreatureAger>();

            editor = GetComponent<CreatureEditor>();
            healthEditor = GetComponent<CreatureHealthEditor>();
            abilities = GetComponent<CreatureAbilities>();
            mover = GetComponent<CreatureMover>();
            interactor = GetComponent<CreatureInteractor>();
        }
#endif

        public void Setup()
        {
            //Scaler.Setup();
            //Animator.Setup();
            // Selector.Setup();
            //Editor.Setup();

        }
        #endregion
    }
}