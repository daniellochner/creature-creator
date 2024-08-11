using System.Collections;
using Unity.Services.Authentication;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LeaderboardsMenu : Dialog<LeaderboardsMenu>
    {
        public LeaderboardUI[] leaderboards;

        public async void Refresh()
        {
            foreach (var leaderboard in leaderboards)
            {
                await leaderboard.Refresh();
            }
        }
    }
}