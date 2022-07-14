using Unity.Netcode;
using System;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class NetworkCreatureInformation : INetworkSerializable
    {
        public float health;
        public float energy;
        public int age;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref health);
            serializer.SerializeValue(ref energy);
            serializer.SerializeValue(ref age);
        }
    }
}