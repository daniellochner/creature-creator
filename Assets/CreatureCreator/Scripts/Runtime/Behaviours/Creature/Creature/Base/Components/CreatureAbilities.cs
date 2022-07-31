// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor), typeof(CreatureMover))]
    public class CreatureAbilities : MonoBehaviour
    {
        #region Fields
        [Header("Setup")]
        [SerializeField] private AbilityUI abilityUIPrefab;

        [Header("Debug")]
        [SerializeField, ReadOnly] private List<Ability> abilities = new List<Ability>();

        private Dictionary<Ability, AbilityInfo> abilitiesInfo = new Dictionary<Ability, AbilityInfo>();
        #endregion

        #region Properties
        public CreatureConstructor CreatureConstructor { get; private set; }

        public List<Ability> Abilities => abilities;
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
            Setup();
        }
        private void OnDisable()
        {
            foreach (Ability ability in Abilities)
            {
                ability.CooldownTimeLeft = 0f;
            }
        }

        private void Update()
        {
            foreach (Ability ability in Abilities)
            {
                if (InputUtility.GetKeyDown(ability.PerformKeybind))
                {
                    if (ability.OnTryPerform())
                    {
                        break;
                    }
                }
                ability.Tick(Time.deltaTime);
            }
        }

        private void Initialize()
        {
            CreatureConstructor = GetComponent<CreatureConstructor>();
        }

        private void Setup()
        {
            SetupConstructor();
        }
        private void SetupConstructor()
        {
            CreatureConstructor.OnAddBodyPartData += delegate (BodyPart bodyPart)
            {
                foreach (Ability ability in bodyPart.Abilities)
                {
                    if (abilitiesInfo.ContainsKey(ability))
                    {
                        abilitiesInfo[ability].count++;
                    }
                    else
                    {
                        Add(ability);
                    }
                }
                Abilities.Sort();
            };
            CreatureConstructor.OnRemoveBodyPartData += delegate (BodyPart bodyPart)
            {
                foreach (Ability ability in bodyPart.Abilities)
                {
                    abilitiesInfo[ability].count--;

                    if (abilitiesInfo[ability].count == 0)
                    {
                        Remove(ability);
                    }
                }
            };
        }

        private void Add(Ability ability)
        {
            ability.Setup(this);

            AbilityUI abilityUI = default;
            if (!ability.PerformKeybind.Equals(Keybind.None))
            {
                abilityUI = Instantiate(abilityUIPrefab, AbilitiesManager.Instance.AbilitiesGrid.transform);
                abilityUI.Setup(ability);
            }

            abilitiesInfo.Add(ability, new AbilityInfo(1, abilityUI));
            abilities.Add(ability);
            ability.OnAdd(); 
        }
        private void Remove(Ability ability)
        {
            if (!ability.PerformKeybind.Equals(Keybind.None))
            {
                Destroy(abilitiesInfo[ability].abilityUI.gameObject);
            }

            abilitiesInfo.Remove(ability);
            abilities.Remove(ability);
            ability.OnRemove();
        }
        #endregion

        #region Inner Classes
        public class AbilityInfo
        {
            public int count;
            public AbilityUI abilityUI;

            public AbilityInfo(int count, AbilityUI abilityUI)
            {
                this.count = count;
                this.abilityUI = abilityUI;
            }
        }
        #endregion
    }
}