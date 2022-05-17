using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    [RequireComponent(typeof(CreaturePhotographer))]
    [RequireComponent(typeof(CreatureHealth))]
    [RequireComponent(typeof(CreatureEnergy))]
    [RequireComponent(typeof(CreatureAge))]
    public class CreatureInformer : MonoBehaviour
    {
        #region Fields
        [SerializeField] private CreatureInformationMenu informationMenu;

        [Header("Debug")]
        [SerializeField, ReadOnly] private CreatureInformation information;
        #endregion

        #region Properties
        public CreaturePhotographer Photographer { get; private set; }
        public CreatureConstructor Constructor { get; private set; }
        public CreatureHealth Health { get; private set; }
        public CreatureEnergy Energy { get; private set; }
        public CreatureAge Age { get; private set; }

        public CreatureInformation Information => information;
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Photographer = GetComponent<CreaturePhotographer>();
            Health = GetComponent<CreatureHealth>();
            Energy = GetComponent<CreatureEnergy>();
            Age = GetComponent<CreatureAge>();
        }

        public void Setup(CreatureInformationMenu menu)
        {
            informationMenu = menu;
            informationMenu.Setup(information);

            Health.OnHealthChanged += InformHealth;
            Energy.OnEnergyChanged += InformEnergy;
            Age.OnAgeChanged += InformAge;

            Constructor.OnConstructCreature += Respawn;
        }

        private void InformHealth(float health)
        {
            Information.Health = Mathf.InverseLerp(Health.MinMaxHealth.min, Health.MinMaxHealth.max, health);
        }
        private void InformEnergy(float energy)
        {
            Information.Energy = energy;
        }
        private void InformAge(int age)
        {
            Information.Age = age;
        }

        public void Respawn()
        {
            Information.Reset();

            Photographer.TakePhoto(128, (Texture2D photo) =>
            {
                Information.Photo = photo;
            });
            Information.Name = Constructor.Data.Name;
        }
        #endregion
    }
}