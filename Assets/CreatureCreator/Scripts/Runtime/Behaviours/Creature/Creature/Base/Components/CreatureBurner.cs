// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureBurner : NetworkBehaviour
    {
        #region Properties
        public CreatureHealth Health { get; set; }

        public NetworkVariable<bool> IsBurning { get; set; } = new NetworkVariable<bool>();
        #endregion

        #region Methods
        private void Awake()
        {
            Health = GetComponent<CreatureHealth>();
        }
        private void Start()
        {
            if (IsServer)
            {
                Health.OnDie += delegate { IsBurning.Value = false; };
            }
        }
        #endregion
    }
}