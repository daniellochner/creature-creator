using Unity.Netcode;
using System;
using Unity.Collections;

namespace DanielLochner.Assets
{
    [Serializable]
    public class ConnectionData : INetworkSerializable
    {
        public string username;
        public string password;

        public ConnectionData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref username);
            serializer.SerializeValue(ref password);
        }
    }
}