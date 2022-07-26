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
                animatorParams.OnSetTriggerWithValue += SetTriggerWithValueServerRpc;
            }
        }

        [ServerRpc]
        private void SetTriggerServerRpc(string triggerParam)
        {
            SetTriggerClientRpc(triggerParam);
        }
        [ClientRpc]
        private void SetTriggerClientRpc(string triggerParam)
        {
            if (!IsOwner)
            {
                animatorParams.SetTrigger(triggerParam);
            }
        }

        [ServerRpc]
        private void SetBoolServerRpc(string boolParam, bool boolValue)
        {
            SetBoolClientRpc(boolParam, boolValue);
        }
        [ClientRpc]
        private void SetBoolClientRpc(string boolParam, bool boolValue)
        {
            if (!IsOwner)
            {
                animatorParams.SetBool(boolParam, boolValue);
            }
        }

        [ServerRpc]
        private void SetFloatServerRpc(string floatParam, float floatValue)
        {
            SetFloatClientRpc(floatParam, floatValue);
        }
        [ClientRpc]
        private void SetFloatClientRpc(string floatParam, float floatValue)
        {
            if (!IsOwner)
            {
                animatorParams.SetFloat(floatParam, floatValue);
            }
        }

        [ServerRpc]
        private void SetIntegerServerRpc(string intParam, int intValue)
        {
            SetIntegerClientRpc(intParam, intValue);
        }
        [ClientRpc]
        private void SetIntegerClientRpc(string intParam, int intValue)
        {
            if (!IsOwner)
            {
                animatorParams.SetInteger(intParam, intValue);
            }
        }

        [ServerRpc]
        private void SetTriggerWithValueServerRpc(string triggerParam, string floatParam, float floatValue)
        {
            SetTriggerWithValueClientRpc(triggerParam, floatParam, floatValue);
        }
        [ClientRpc]
        private void SetTriggerWithValueClientRpc(string triggerParam, string floatParam, float floatValue)
        {
            if (!IsOwner)
            {
                animatorParams.SetTriggerWithValue(triggerParam, floatParam, floatValue);
            }
        }
    }
}