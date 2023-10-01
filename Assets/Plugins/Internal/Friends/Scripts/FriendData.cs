using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace DanielLochner.Assets
{
    [Preserve]
    [DataContract]
    public class FriendData
    {
        [Preserve]
        [DataMember(Name = "Lobby Id", IsRequired = true, EmitDefaultValue = true)]
        public string LobbyId { get; set; }
    }
}