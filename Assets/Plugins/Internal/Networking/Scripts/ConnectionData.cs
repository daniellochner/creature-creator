using Unity.Netcode;
using System;
using Unity.Collections;

namespace DanielLochner.Assets
{
    [Serializable]
    public class ConnectionData : INetworkSerializable
    {
        public FixedString32Bytes username;
        public FixedString32Bytes password;

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