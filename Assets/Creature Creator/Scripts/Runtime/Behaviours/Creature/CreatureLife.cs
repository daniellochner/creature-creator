// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureMover))]
    public class CreatureLife : CreatureHealth
    {
        #region Properties
        public CreatureMover Mover { get; private set; }
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            Mover = GetComponent<CreatureMover>();
        }

        protected override void OnDie()
        {
            EditorManager.Instance.IsVisible = false;

            base.OnDie();

            string name = Informer.Information.Name.Equals("Unnamed") ? "You" : Informer.Information.Name;
            string age  = Informer.Information.FormattedAge;
            InformationDialog.Inform("You Died!", $"{name} died after {age}. Press the button below to respawn at your previous editing platform.", "Respawn", Respawn);
        }
        protected override void OnRespawn()
        {
            Mover.Teleport(Mover.Platform);

            base.OnRespawn();

            EditorManager.Instance.IsVisible = true;
        }
        #endregion
    }
}