// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(BodyPartConstructor))]
    public class BodyPartAnimator : MonoBehaviour, IFlippable<BodyPartAnimator>
    {
        #region Properties
        public CreatureAnimator CreatureAnimator { get; private set; }
        public BodyPartConstructor BodyPartConstructor { get; private set; }

        public BodyPartAnimator Flipped { get; set; }
        public bool IsFlipped { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            BodyPartConstructor = GetComponent<BodyPartConstructor>();
        }
        public virtual void Setup(CreatureAnimator creatureAnimator)
        {
            CreatureAnimator = creatureAnimator;
        }

        public void SetFlipped(BodyPartAnimator main)
        {
            IsFlipped = true;

            Flipped = main;
            main.Flipped = this;
        }
        #endregion
    }
}