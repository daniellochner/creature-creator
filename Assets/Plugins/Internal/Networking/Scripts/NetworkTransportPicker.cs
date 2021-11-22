using Unity.Netcode;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class NetworkTransportPicker : MonoBehaviourSingleton<NetworkTransportPicker>
    {
        [SerializeField] private NetworkTransportDictionary networkTransports;

        public T GetTransport<T>(string transport) where T : NetworkTransport
        {
            return networkTransports[transport] as T;
        }
        
        [Serializable]
        public class NetworkTransportDictionary : SerializableDictionaryBase<string, NetworkTransport> { }
    }
}