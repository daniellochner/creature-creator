// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureHealth))]
    [RequireComponent(typeof(CreatureInformer))]
    [RequireComponent(typeof(CreatureMover))]
    [RequireComponent(typeof(CreatureHungerDepleter))]
    [RequireComponent(typeof(CreatureAger))]
    public class CreatureHealthEditor : MonoBehaviour
    {
        #region Properties
        public CreatureHealth Health { get; private set; }
        public CreatureInformer Informer { get; private set; }
        public CreatureMover Mover { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Health = GetComponent<CreatureHealth>();
            Informer = GetComponent<CreatureInformer>();
            Mover = GetComponent<CreatureMover>();
        }

        private void Start()
        {
            Health.OnDie += Die;
            Health.OnRespawn += Respawn;
        }

        public void Die()
        {
            EditorManager.Instance.IsVisible = false;

            EditorManager.Instance.Invoke(delegate
            {
                string name = Informer.Information.Name.Equals("Unnamed") ? "You" : Informer.Information.Name;
                string age = Informer.Information.FormattedAge;
                InformationDialog.Inform("You Died!", $"{name} died after {age}. Press the button below to respawn at your previous editing platform.", "Respawn", false, Health.Respawn);
            }, 1f);
        }
        public void Respawn()
        {
            Mover.Teleport(Mover.Platform);

            EditorManager.Instance.IsVisible = true;
        }
        #endregion
    }
}