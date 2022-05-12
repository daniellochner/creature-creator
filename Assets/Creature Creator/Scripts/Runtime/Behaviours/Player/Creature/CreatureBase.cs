// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    [RequireComponent(typeof(CreatureAnimator))]
    [RequireComponent(typeof(CreatureCollider))]
    [RequireComponent(typeof(CreatureCloner))]
    [RequireComponent(typeof(CreaturePhotographer))]
    [RequireComponent(typeof(CreatureKiller))]
    [RequireComponent(typeof(CreatureEffector))]
    public class CreatureBase : MonoBehaviour
    {
        #region Fields
        [SerializeField] private CreatureConstructor constructor;
        [SerializeField] private CreatureAnimator animator;
        [SerializeField] private new CreatureCollider collider;
        [SerializeField] private CreatureCloner cloner;
        [SerializeField] private CreaturePhotographer photographer;
        [SerializeField] private CreatureKiller killer;
        [SerializeField] private CreatureEffector effector;
        #endregion

        #region Properties
        public CreatureConstructor Constructor => constructor;
        public CreatureAnimator Animator => animator;
        public CreatureCollider Collider => collider;
        public CreatureCloner Cloner => cloner;
        public CreaturePhotographer Photographer => photographer;
        public CreatureKiller Killer => killer;
        public CreatureEffector Effector => effector;
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
        }
#endif

        public virtual void Setup()
        {
            Animator.Setup();
        }
        #endregion
    }
}