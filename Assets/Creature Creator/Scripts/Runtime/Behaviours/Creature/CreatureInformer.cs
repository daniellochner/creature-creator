using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    [RequireComponent(typeof(CreaturePhotographer))]
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

        public CreatureInformation Information => information;

        public Action OnRespawn { get; set; }
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
        }

        public void Setup(CreatureInformationMenu menu)
        {
            informationMenu = menu;
            informationMenu.Setup(information);

            Constructor.OnConstructCreature += Respawn;
        }

        public void Respawn()
        {
            Information.Reset();

            Photographer.TakePhoto(128, (Texture2D p) => Information.Photo = p);
            Information.Name = Constructor.Data.Name;
            OnRespawn?.Invoke();
        }
        #endregion
    }
}