// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureInformationManager : MonoBehaviourSingleton<CreatureInformationManager>
    {
        #region Fields
        [SerializeField] private CreatureInformationMenu informationMenu;

        [Header("Debug")]
        [SerializeField, ReadOnly] private CreatureInformation information;
        #endregion

        #region Properties
        public CreatureInformation Information => information;

        private CreaturePlayer Creature => Player.Instance.Creature;
        #endregion

        #region Methods
        private void Update()
        {
            HandleInformation();
        }

        public void Setup()
        {
            informationMenu.Setup(Information);
        }
        public void Respawn()
        {
            Information.Reset();

            Creature.Health.HealthPercentage = 1f;
            Creature.Energy.Energy = 1f;
            Creature.Age.Start();

            Creature.Photographer.TakePhoto(128, (Texture2D p) => Information.Photo = p);
            Information.Name = Creature.Constructor.Data.Name;
        }

        private void HandleInformation()
        {
            if (!Player.Instance) return;

            Information.Health = Creature.Health.HealthPercentage;
            Information.Energy = Creature.Energy.Energy;
            Information.Age = Creature.Age.Age;
        }
        #endregion
    }
}