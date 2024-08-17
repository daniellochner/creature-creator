using Cysharp.Threading.Tasks;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LeaderboardsManager : MonoBehaviourSingleton<LeaderboardsManager>
    {
        public SerializableDictionaryBase<Map, LeaderboardEntry> MyTimes { get; set; } = new SerializableDictionaryBase<Map, LeaderboardEntry>();

        public async void Start()
        {
            await UniTask.WaitUntil(() => (UnityServices.Instance.State == ServicesInitializationState.Initialized) && (AuthenticationService.Instance.IsSignedIn));

            foreach (var leaderboard in LeaderboardsMenu.Instance.leaderboards)
            {
                try
                {
                    LeaderboardEntry myTime = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboard.LeaderboardId);
                    MyTimes.Add(leaderboard.map, myTime);
                }
                catch (LeaderboardsException le)
                {
                    if (le.Reason == LeaderboardsExceptionReason.EntryNotFound)
                    {
                        // Ignore when no entry found...
                    }
                    else
                    {
                        Debug.Log(le);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                await leaderboard.Refresh();
            }
        }
    }
}