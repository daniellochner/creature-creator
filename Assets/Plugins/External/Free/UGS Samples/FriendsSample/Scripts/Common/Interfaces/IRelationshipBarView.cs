using System;

namespace Unity.Services.Samples.Friends
{
    public interface IRelationshipBarView
    {
        Action onShowAddFriend { get; set; }
        void Refresh();
    }


}

