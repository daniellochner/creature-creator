using Unity.Netcode;
using System;

namespace DanielLochner.Assets
{
    [Serializable]
    public class PlayerData : INetworkSerializable, IEquatable<PlayerData>
    {
        public ulong clientId;
        public string playerId;
        public string username;
        public int level;

        public bool Equals(PlayerData other)
        {
            return clientId.Equals(other.clientId) && playerId.Equals(other.playerId);
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref playerId);
            serializer.SerializeValue(ref username);
            serializer.SerializeValue(ref level);
        }
    }
}