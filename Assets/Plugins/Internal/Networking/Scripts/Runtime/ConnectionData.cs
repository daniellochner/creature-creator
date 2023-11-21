using Unity.Netcode;
using System;
using Unity.Collections;

namespace DanielLochner.Assets
{
    [Serializable]
    public class ConnectionData : INetworkSerializable
    {
        public string playerId;
        public string username;
        public string password;
        public bool isVIP;

        public ConnectionData(string playerId, string username, string password, bool isVIP)
        {
            this.playerId = playerId;
            this.username = username;
            this.password = password;
            this.isVIP = isVIP;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref playerId);
            serializer.SerializeValue(ref username);
            serializer.SerializeValue(ref password);
            serializer.SerializeValue(ref isVIP);
        }
    }
}