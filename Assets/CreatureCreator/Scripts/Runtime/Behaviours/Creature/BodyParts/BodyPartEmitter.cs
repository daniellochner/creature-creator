// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BodyPartEmitter : MonoBehaviour
    {
        #region Fields
        [SerializeField] private string emissionId;
        [SerializeField] private Transform spawnPoint;
        #endregion

        #region Properties
        public string EmissionId => emissionId;
        public Transform SpawnPoint => spawnPoint;

        public CreatureEmitter CreatureEmitter { get; private set; }
        public BodyPartConstructor Constructor { get; private set; }
        public BodyPartEmitter Flipped { get; private set; }

        public bool IsFlipped { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<BodyPartConstructor>();
        }

        public void Setup(CreatureEmitter creatureEmitter)
        {
            CreatureEmitter = creatureEmitter;
        }

        public void SetFlipped(BodyPartEmitter main)
        {
            IsFlipped = true;

            Flipped = main;
            main.Flipped = this;
        }
        #endregion
    }
}