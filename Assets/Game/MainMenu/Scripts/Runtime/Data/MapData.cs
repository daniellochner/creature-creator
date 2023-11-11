// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Map")]
    public class MapData : ScriptableObject
    {
        #region Fields
        [SerializeField] private string nameId;
        [SerializeField] private int maxPlayers;
        [SerializeField] private int spawnPoints;
        [SerializeField] private Sprite preview;
        #endregion

        #region Properties
        public string NameId => nameId;
        public int MaxPlayers => maxPlayers;
        public int SpawnPoints => spawnPoints;
        public Sprite Preview => preview;
        #endregion
    }
}