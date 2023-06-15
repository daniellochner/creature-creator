// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;
using static DanielLochner.Assets.CreatureCreator.Ability;

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
        public CreatureAnimator CreatureAnimator { get; private set; }

        public List<Ability> Abilities => abilities;

        public bool CanInput
        {
            get
            {
                return EditorManager.Instance.IsPlaying && !InputDialog.Instance.IsOpen && !ConfirmationDialog.Instance.IsOpen && !InformationDialog.Instance.IsOpen;
            }
        }
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
                ability.Shutdown();
            }
        }

        private void Update()
        {
            foreach (Ability ability in Abilities)
            {
                ability.Tick(Time.deltaTime);

                if (CanInput)
                {
                    if (ability.Prepare)
                    {
                        if (InputUtility.GetKeyDown(ability.PerformKeybind))
                        {
                            if (abilitiesInfo[ability].isActive && ability.OnTryPrepare())
                            {
                                break;
                            }
                        }
                        else
                        if (InputUtility.GetKeyUp(ability.PerformKeybind))
                        {
                            if (abilitiesInfo[ability].isActive && ability.OnTryPerform())
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (InputUtility.GetKeyDown(ability.PerformKeybind))
                        {
                            if (abilitiesInfo[ability].isActive && ability.OnTryPerform())
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void Initialize()
        {
            CreatureConstructor = GetComponent<CreatureConstructor>();
            CreatureAnimator = GetComponent<CreatureAnimator>();
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

                UpdateActive();
            };
            CreatureConstructor.OnRemoveBodyPartData += delegate (BodyPart bodyPart)
            {
                foreach (Ability ability in bodyPart.Abilities)
                {
                    if (abilitiesInfo.ContainsKey(ability))
                    {
                        abilitiesInfo[ability].count--;

                        if (abilitiesInfo[ability].count == 0)
                        {
                            Remove(ability);
                        }
                    }
                }

                UpdateActive();
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

        private void UpdateActive()
        {
            HashSet<Type> abilityTypes = new HashSet<Type>();
            foreach (Ability ability in Abilities)
            {
                if (!ability.PerformKeybind.Equals(Keybind.None))
                {
                    AbilityInfo info = abilitiesInfo[ability];
                    Type type = ability.GetType();
                    if (abilityTypes.Contains(type))
                    {
                        info.SetActive(false);
                    }
                    else
                    {
                        abilityTypes.Add(type);
                        info.SetActive(true);
                    }
                }
            }
        }

        public bool HasAbility<T>()
        {
            return Abilities.Find(x => x is T);
        }
        #endregion

        #region Inner Classes
        public class AbilityInfo
        {
            public int count;
            public AbilityUI abilityUI;
            public bool isActive;

            public AbilityInfo(int count, AbilityUI abilityUI)
            {
                this.count = count;
                this.abilityUI = abilityUI;
            }

            public void SetActive(bool isActive)
            {
                this.isActive = isActive;
                abilityUI.gameObject.SetActive(isActive);
            }
        }
        #endregion
    }
}