// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BodyPartLauncher : MonoBehaviour
    {
        #region Fields
        [SerializeField] private string projectileId;
        [SerializeField] private Trajectory[] spawnPoints;
        [SerializeField] private float speed;
        #endregion

        #region Properties
        public string ProjectileId => projectileId;
        public Trajectory[] SpawnPoints => spawnPoints;
        public float Speed => speed;

        public CreatureLauncher CreatureLauncher { get; private set; }
        public BodyPartConstructor Constructor { get; private set; }
        public BodyPartLauncher Flipped { get; private set; }

        public bool IsFlipped { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<BodyPartConstructor>();
        }

        public void Setup(CreatureLauncher creatureLauncher)
        {
            CreatureLauncher = creatureLauncher;
        }

        public void SetFlipped(BodyPartLauncher main)
        {
            IsFlipped = true;

            Flipped = main;
            main.Flipped = this;
        }
        #endregion
    }
}