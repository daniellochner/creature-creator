using System;
using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class LeaderboardUI : MonoBehaviour
    {
        public Map map;
        [Space]
        public TimeUI timePrefab;
        [Space]
        public RectTransform root;
        public GameObject none;
        public GameObject refresh;

        public string LeaderboardId
        {
            get => $"times_{map}".ToLower();
        }

        public async Task Refresh(LeaderboardEntry myTime = null)
        {
            root.DestroyChildren();
            none.SetActive(false);
            refresh.SetActive(true);

            try
            {
                if (myTime == null)
                {
                    LeaderboardsManager.Instance.myTimes.TryGetValue(map, out myTime);
                }

                LeaderboardScoresPage topTimes = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions()
                {
                    Limit = 50
                });

                TimeUI myTimeUI = null;
                foreach (var time in topTimes.Results)
                {
                    var timeUI = Instantiate(timePrefab, root);
                    timeUI.Setup(time.PlayerName, time.Rank, (int)time.Score);

                    if (myTime != null && time.PlayerId == myTime.PlayerId)
                    {
                        myTimeUI = timeUI;
                        myTimeUI.SetMine(true);
                    }
                }
                if (myTime != null && myTimeUI == null)
                {
                    myTimeUI = Instantiate(timePrefab, root);
                    myTimeUI.Setup(myTime.PlayerName, myTime.Rank, (int)myTime.Score, true);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            none.SetActive(root.childCount == 0);
            refresh.SetActive(false);
        }
    }
}