using System;
using System.Collections.Generic;

namespace Unity.Services.Samples.Friends
{

    public interface IFriendsListView : IListView
    {
        Action<string> onRemove { get; set; }
        Action<string> onBlock { get; set; }
        void BindList(List<FriendsEntryData> friendEntryDatas);
    }

}

