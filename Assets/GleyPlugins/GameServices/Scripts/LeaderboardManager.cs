namespace GleyGameServices
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Events;


#if UseGameCenterPlugin
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
#endif
#if UseGooglePlayGamesPlugin
    using GooglePlayGames.BasicApi;
#endif

    public class LeaderboardManager
    {
        //load the list of all game leaderboards form the Settings Window
        private List<Leaderboard> gameLeaderboards;


        /// <summary>
        /// Constructor, loads settings data
        /// </summary>
        public LeaderboardManager()
        {
            try
            {
                gameLeaderboards = Resources.Load<GameServicesSettings>("GameServicesData").allGameLeaderboards;
            }
            catch
            {
                Debug.LogError("Game Services Data not found -> Go to Window->Gley->Game Services to setup the plugin");
                ScreenWriter.Write("Game Services Data not found -> Go to Window->Gley->Game Services to setup the plugin");
            }
        }


        /// <summary>
        /// Submit a score for both ANdroid and iOS using the Unity Social interface
        /// </summary>
        /// <param name="score">value of the score</param>
        /// <param name="leaderboardName">leaderboard to submit score in</param>
        /// <param name="SubmitComplete">callback -> submit result</param>
        public void SubmitScore(long score, string leaderboardName, UnityAction<bool, GameServicesError> SubmitComplete)
        {
            string leaderboardId = null;
#if UseGooglePlayGamesPlugin
            leaderboardId = gameLeaderboards.FirstOrDefault(cond => cond.name == leaderboardName).idGoogle;
#endif
#if UseGameCenterPlugin
            leaderboardId = gameLeaderboards.FirstOrDefault(cond => cond.name == leaderboardName).idIos;
#endif
            Social.ReportScore(score, leaderboardId, (bool success) =>
            {
                if (success)
                {
                    if (SubmitComplete != null)
                    {
                        SubmitComplete(true, GameServicesError.Success);
                    }
                }
                else
                {
                    if (SubmitComplete != null)
                    {
                        SubmitComplete(false, GameServicesError.ScoreSubmitFailed);
                    }
                }
            });
        }


        /// <summary>
        /// Shows all game leaderboards
        /// </summary>
        public void ShowLeaderboards()
        {
            Social.ShowLeaderboardUI();
        }


        /// <summary>
        /// Displays a specific leaderboard on screen. Available only for Google Play
        /// </summary>
        /// <param name="leaderboardName">the name of the leaderboard to display</param>
        public void ShowSingleLeaderboard(LeaderboardNames leaderboardName)
        {
#if UseGooglePlayGamesPlugin
            string leaderboardId = gameLeaderboards.FirstOrDefault(cond => cond.name == leaderboardName.ToString()).idGoogle;
            ((GooglePlayGames.PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboardId);
            return;
#endif
#if UseGameCenterPlugin
            string leaderboardId = gameLeaderboards.FirstOrDefault(cond => cond.name == leaderboardName.ToString()).idIos;
            GameCenterPlatform.ShowLeaderboardUI(leaderboardId, UnityEngine.SocialPlatforms.TimeScope.AllTime);
            return;
#endif
        }

        /// <summary>
        /// Retrieves the highest score from leaderboard of the current player
        /// </summary>
        /// <param name="leaderboardName">the name of the leaderboard</param>
        /// <param name="CompleteMethod">a complete method called after score is loaded</param>
        public void GetPlayerScore(LeaderboardNames leaderboardName, UnityAction<long> CompleteMethod)
        {
#if UseGooglePlayGamesPlugin
            string leaderboardId = gameLeaderboards.FirstOrDefault(cond => cond.name == leaderboardName.ToString()).idGoogle;
            ((GooglePlayGames.PlayGamesPlatform)Social.Active).LoadScores(
                leaderboardId,
                LeaderboardStart.PlayerCentered,
                1,
                LeaderboardCollection.Public,
                LeaderboardTimeSpan.AllTime,
                (LeaderboardScoreData data) =>
                {
                    if (CompleteMethod != null)
                    {
                        CompleteMethod(data.PlayerScore.value);
                    }
                }
            );
#endif
#if UseGameCenterPlugin
            ILeaderboard Leaderboard = Social.CreateLeaderboard();
            Leaderboard.id = gameLeaderboards.FirstOrDefault(cond => cond.name == leaderboardName.ToString()).idIos;
            Leaderboard.timeScope = TimeScope.AllTime;
            Leaderboard.LoadScores(success =>
            {
                if (success)
                {
                    IScore[] scores = Leaderboard.scores;
                    if (scores.Length > 0)
                    {
                        if (CompleteMethod != null)
                        {
                            CompleteMethod(Leaderboard.localUserScore.value);
                        }
                    }
                    else
                    {
                        if (CompleteMethod != null)
                        {
                            CompleteMethod(0);
                        }
                    }
                }
                else
                {
                    ScreenWriter.Write("Load Scores failed");
                    if (CompleteMethod != null)
                    {
                        CompleteMethod(0);
                    }
                }
            });
#endif
        }


        /// <summary>
        /// Retrieves the highest score from leaderboard of the current player
        /// </summary>
        /// <param name="leaderboardName">the name of the leaderboard</param>
        /// <param name="CompleteMethod">a complete method called after score is loaded</param>
        public void GetPlayerRank(LeaderboardNames leaderboardName, UnityAction<long> CompleteMethod)
        {
#if UseGooglePlayGamesPlugin
            string leaderboardId = gameLeaderboards.FirstOrDefault(cond => cond.name == leaderboardName.ToString()).idGoogle;
            ((GooglePlayGames.PlayGamesPlatform)Social.Active).LoadScores(
                leaderboardId,
                LeaderboardStart.PlayerCentered,
                1,
                LeaderboardCollection.Public,
                LeaderboardTimeSpan.AllTime,
                (LeaderboardScoreData data) =>
                {
                    if (CompleteMethod != null)
                    {
                        CompleteMethod(data.PlayerScore.rank);
                    }
                }
            );
#endif
#if UseGameCenterPlugin
            ILeaderboard Leaderboard = Social.CreateLeaderboard();
            Leaderboard.id = gameLeaderboards.FirstOrDefault(cond => cond.name == leaderboardName.ToString()).idIos;
            Leaderboard.timeScope = TimeScope.AllTime;
            Leaderboard.LoadScores(success =>
            {
                if (success)
                {
                    IScore[] scores = Leaderboard.scores;
                    if (scores.Length > 0)
                    {
                        if (CompleteMethod != null)
                        {
                            CompleteMethod(Leaderboard.localUserScore.rank);
                        }
                    }
                    else
                    {
                        if (CompleteMethod != null)
                        {
                            CompleteMethod(0);
                        }
                    }
                }
                else
                {
                    ScreenWriter.Write("Load Scores failed");
                    if (CompleteMethod != null)
                    {
                        CompleteMethod(0);
                    }
                }
            });
#endif
        }
    }
}
