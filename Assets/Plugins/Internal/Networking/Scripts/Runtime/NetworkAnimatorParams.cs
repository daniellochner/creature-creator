using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(AnimatorParams))]
    public class NetworkAnimatorParams : NetworkBehaviour
    {
        private AnimatorParams animatorParams;

        private void Awake()
        {
            animatorParams = GetComponent<AnimatorParams>();
        }

        private void Start()
        {
            if (IsOwner)
            {
                animatorParams.OnSetTrigger += SetTriggerServerRpc;
                animatorParams.OnSetBool += SetBoolServerRpc;
                animatorParams.OnSetFloat += SetFloatServerRpc;
                animatorParams.OnSetInteger += SetIntegerServerRpc;
            }
        }

        [ServerRpc]
        private void SetTriggerServerRpc(string param)
        {
            SetTriggerClientRpc(param);
        }
        [ClientRpc]
        private void SetTriggerClientRpc(string param)
        {
            if (!IsOwner)
            {
                animatorParams.SetTrigger(param);
            }
        }

        [ServerRpc]
        private void SetBoolServerRpc(string param, bool value)
        {
            SetBoolClientRpc(param, value);
        }
        [ClientRpc]
        private void SetBoolClientRpc(string param, bool value)
        {
            if (!IsOwner)
            {
                animatorParams.SetBool(param, value);
            }
        }

        [ServerRpc]
        private void SetFloatServerRpc(string param, float value)
        {
            SetFloatClientRpc(param, value);
        }
        [ClientRpc]
        private void SetFloatClientRpc(string param, float value)
        {
            if (!IsOwner)
            {
                animatorParams.SetFloat(param, value);
            }
        }

        [ServerRpc]
        private void SetIntegerServerRpc(string param, int value)
        {
            SetIntegerClientRpc(param, value);
        }
        [ClientRpc]
        private void SetIntegerClientRpc(string param, int value)
        {
            if (!IsOwner)
            {
                animatorParams.SetInteger(param, value);
            }
        }
    }
}