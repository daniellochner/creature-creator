// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class NetworkCreatureNonPlayer : NetworkCreature
    {
        #region Fields
        private bool despawn;
        #endregion

        #region Methods

        public override void Setup()
        {
            base.Setup();
            
            if (NetworkConnectionManager.IsConnected)
            {
                if (IsOwner)
                {
                    source.Setup();
                }
                else
                {
                    target.Setup();
                }
            }
            else
            {
                source.Setup();
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (despawn)
            {
                Despawn();
            }
        }
        public void Despawn()
        {
            if (NetworkObject.IsSpawned)
            {
                NetworkObject.Despawn(true);
            }
            else
            {
                despawn = true;
            }
        }
        #endregion
    }
}