using System.Text;
using Unity.Services.Friends.Models;

namespace Unity.Services.Samples.Friends
{
    [System.Serializable]
    public struct FriendsEntryData
    {
        public string Name;
        public string Id;
        public Availability Availability;
        public string Activity;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("FriendEntryData: \n");
            sb.Append(Name);
            sb.Append(" : ");
            sb.AppendLine(Id);
            sb.Append(Availability);
            sb.Append(" : ");
            sb.AppendLine(Activity);
            return sb.ToString();
        }
    }
}
