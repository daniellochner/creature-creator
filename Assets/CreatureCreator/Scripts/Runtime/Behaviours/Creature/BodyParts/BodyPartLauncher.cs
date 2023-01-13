// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BodyPartLauncher : MonoBehaviour
    {
        #region Fields
        [SerializeField] private string projectileId;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private float speed;
        #endregion

        #region Properties
        public string ProjectileId => projectileId;
        public Transform[] SpawnPoints => spawnPoints;
        public float Speed => speed;

        public BodyPartConstructor BodyPartConstructor { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            BodyPartConstructor = GetComponent<BodyPartConstructor>();
        }
        #endregion
    }
}