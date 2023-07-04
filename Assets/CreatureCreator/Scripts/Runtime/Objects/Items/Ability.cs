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
        [SerializeField] private int level;
        [SerializeField] private new string name;
        [SerializeField] private int priority;
        [SerializeField] private Keybind performKeybind;
        [SerializeField] private float cooldown;
        [SerializeField] private bool prepare;
        #endregion

        #region Properties
        public int Level => level;
        public string Name => name;
        public int Priority => priority;
        public float Cooldown => cooldown;
        public bool Prepare => prepare;
        public Keybind PerformKeybind
        {
            get => performKeybind;
            set => performKeybind = value;
        }

        public CreatureAbilities CreatureAbilities { get; private set; }

        public float CooldownTimeLeft { get; set; }
        public bool IsPrepared { get; set; }

        public virtual bool CanPerform => !CinematicManager.Instance.IsInCinematic && CreatureAbilities.enabled;
        #endregion

        #region Methods
        public virtual void Setup(CreatureAbilities creatureAbilities)
        {
            CreatureAbilities = creatureAbilities;
        }
        public virtual void Shutdown()
        {
            CooldownTimeLeft = 0f;
            IsPrepared = false;
        }

        public bool OnTryPrepare()
        {
            if (CooldownTimeLeft <= 0 && CanPerform)
            {
                OnPrepare();
                IsPrepared = true;

                return true;
            }
            return false;
        }
        public bool OnTryPerform()
        {
            if (CooldownTimeLeft <= 0 && CanPerform && (!Prepare || IsPrepared))
            {
                OnPerform();
                CooldownTimeLeft = Cooldown;
                IsPrepared = false;

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
        public virtual void OnPrepare()
        {
        }
        public virtual void OnPerform()
        {
        }

        public override string ToString()
        {
            return LocalizationUtility.Localize(Name);
        }
        public int CompareTo(Ability other)
        {
            return other.priority.CompareTo(Priority); // Reversed to force descending order when sorted.
        }
        #endregion
    }
}