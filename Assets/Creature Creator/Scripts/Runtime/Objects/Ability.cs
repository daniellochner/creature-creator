// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public abstract class Ability : Item, IComparable<Ability>
    {
        #region Fields
        [Header("Ability")]
        [SerializeField] private int priority;
        [SerializeField] private KeyCode performKey;
        [SerializeField] private float cooldown;
        #endregion

        #region Properties
        public int Priority => priority;
        public KeyCode PerformKey => performKey;
        public float Cooldown => cooldown;

        public CreatureAbilities CreatureAbilities { get; private set; }

        public float CooldownTimeLeft { get; private set; }
        public virtual bool CanPerform => true;
        #endregion

        #region Methods
        public virtual void Setup(CreatureAbilities creatureAbilities)
        {
            CreatureAbilities = creatureAbilities;
        }

        public bool OnTryPerform()
        {
            if (CooldownTimeLeft <= 0 && CanPerform)
            {
                OnPerform();
                CooldownTimeLeft = Cooldown;

                return true;
            }
            return false;
        }
        public void Tick(float deltaTime)
        {
            if (CooldownTimeLeft > 0)
            {
                CooldownTimeLeft -= deltaTime;
            }
            else
            {
                CooldownTimeLeft = 0f;
            }
        }

        public virtual void OnAdd()
        {
        }
        public virtual void OnRemove()
        {
        }
        public virtual void OnPerform()
        {
        }

        public override string ToString()
        {
            return name;
        }
        public int CompareTo(Ability other)
        {
            return other.priority.CompareTo(Priority); // Reversed to force descending order when sorted.
        }
        #endregion
    }
}