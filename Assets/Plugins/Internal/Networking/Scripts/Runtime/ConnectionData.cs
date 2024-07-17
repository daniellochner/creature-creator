using Unity.Netcode;
using System;

namespace DanielLochner.Assets
{
    [Serializable]
    public class ConnectionData : INetworkSerializable
    {
        public string playerId;
        public string username;
        public string password;
        public int level;

        public ConnectionData(string playerId, string username, string password, int level)
        {
            this.playerId = playerId;
            this.username = username;
            this.password = password;
            this.level = level;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref playerId);
            serializer.SerializeValue(ref username);
            serializer.SerializeValue(ref password);
            serializer.SerializeValue(ref level);
        }
    }
}