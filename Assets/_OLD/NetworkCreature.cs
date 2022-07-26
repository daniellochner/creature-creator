// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkCreature : NetworkBehaviour
    {
        

        //#region Respawn
        //[ServerRpc]
        //private void RespawnServerRpc()
        //{
        //    RespawnClientRpc();
        //}
        //[ClientRpc]
        //private void RespawnClientRpc()
        //{
        //    if (!IsOwner)
        //    {
        //        Destroy(Creature.Killer.Corpse);//
        //    }
        //}
        //#endregion
    }
}