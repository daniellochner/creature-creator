using System;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class FactoryItem : IEquatable<FactoryItem>
    {
        public ulong id;
        public string name;
        public ulong creatorId;
        public string description;
        public uint upVotes;
        public uint downVotes;
        public string previewURL;
        public FactoryTagType tag;
        public uint timeCreated;

        public bool Equals(FactoryItem other)
        {
            return id == other.id;
        }
    }
}