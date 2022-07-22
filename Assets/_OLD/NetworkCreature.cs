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


        //#region Animations
        //[ServerRpc]
        //private void SetTriggerServerRpc(string param)
        //{
        //    SetTriggerClientRpc(param);
        //}
        //[ClientRpc]
        //private void SetTriggerClientRpc(string param)
        //{
        //    if (!IsOwner)
        //    {
        //        Creature.Animator.Animator.SetTrigger(param);
        //    }
        //}

        //[ServerRpc]
        //private void SetBoolServerRpc(string param, bool value)
        //{
        //    SetBoolClientRpc(param, value);
        //}
        //[ClientRpc]
        //private void SetBoolClientRpc(string param, bool value)
        //{
        //    if (!IsOwner)
        //    {
        //        Creature.Animator.Animator.SetBool(param, value);
        //    }
        //}
        //#endregion




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