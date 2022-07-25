using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets
{
    [Serializable]
    public class NetworkDictionary<K, V> : SerializableDictionaryBase<K, V>, INetworkSerializable
    {
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
        }
    }
}