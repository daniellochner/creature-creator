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
        //#region Properties
        //public NetworkVariable<float> Health { get; private set; } = new NetworkVariable<float>();
        //public NetworkVariable<float> Energy { get; private set; } = new NetworkVariable<float>();
        //public NetworkVariable<int> Age { get; private set; } = new NetworkVariable<int>();
        //

        //public virtual CreatureBase Creature => local;
        //#endregion

        //#region Methods
        //private void Awake()
        //{
        //    Initialize();
        //}
        //private void Start()
        //{
        //    //Setup();
        //}

        //private void Initialize()
        //{
        //    networkObject = GetComponent<NetworkObject>();
        //    networkTransform = GetComponent<NetworkTransform>();
        //}
        //public virtual void Setup()
        //{
        //    if (NetworkConnectionManager.IsConnected)
        //    {
        //        if (IsOwner)
        //        {
        //            Creature.Health.OnHealthChanged += SetHealthServerRpc;
        //            Creature.Energy.OnEnergyChanged += SetEnergyServerRpc;
        //            Creature.Age.OnAgeChanged += SetAgeServerRpc;

        //            Creature.Animator.OnSetTrigger += SetTriggerServerRpc;
        //            Creature.Animator.OnSetBool += SetBoolServerRpc;
        //        }
        //        else
        //        {
        //            Health.OnValueChanged += UpdateHealth;
        //            Energy.OnValueChanged += UpdateEnergy;
        //            Age.OnValueChanged += UpdateAge;
        //        }
        //    }
        //}

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
        //        Destroy(Creature.Killer.Corpse);
        //    }
        //}
        //#endregion
        

        //#region Information
        //[ServerRpc]
        //private void SetHealthServerRpc(float health)
        //{
        //    Health.Value = health;
        //}
        //[ServerRpc]
        //private void SetEnergyServerRpc(float energy)
        //{
        //    Energy.Value = energy;
        //}
        //[ServerRpc]
        //private void SetAgeServerRpc(int age)
        //{
        //    Age.Value = age;
        //}

        //private void UpdateHealth(float oldHealth, float newHealth)
        //{
        //    Creature.Health.Health = newHealth;
        //}
        //private void UpdateEnergy(float oldEnergy, float newEnergy)
        //{
        //    Creature.Energy.Energy = newEnergy;
        //}
        //private void UpdateAge(int oldAge, int newAge)
        //{
        //    Creature.Age.Age = newAge;
        //}
        //#endregion
        //#endregion
    }
}