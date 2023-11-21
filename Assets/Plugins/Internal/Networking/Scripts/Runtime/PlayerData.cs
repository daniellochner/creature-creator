using Unity.Netcode;
using System;

namespace DanielLochner.Assets
{
    [Serializable]
    public class PlayerData : INetworkSerializable
    {
        public ulong clientId;
        public string playerId;
        public string username;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref playerId);
            serializer.SerializeValue(ref username);
        }
    }
}