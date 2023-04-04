#if USE_BOLT_SUPPORT
namespace GleyGameServices
{
    using Unity.VisualScripting;
    //using Bolt;
    //using Ludiq;
    using UnityEngine;

    [IncludeInSettings(true)]
    public class GameServicesBoltSupport
    {
        static GameObject logInEventTarget;
        static GameObject achievementEventTarget;
        static GameObject leaderboardEventTarget;
        static GameObject playerScoreEventTarget;
        static GameObject playerRankEventTarget;
        public static void LogIn(GameObject _eventTarget)
        {
            logInEventTarget = _eventTarget;
            GameServices.Instance.LogIn(LogInComplete);
        }

        private static void LogInComplete(bool success)
        {
            CustomEvent.Trigger(logInEventTarget, "LogInComplete", false);
        }

        public static void ShowAchievementsUI()
        {
            GameServices.Instance.ShowAchievementsUI();
        }

        public static void ShowLeaderboardsUI()
        {
            GameServices.Instance.ShowLeaderboadsUI();
        }

        public static void SubmitAchievement(AchievementNames achievementName, GameObject _eventTarget)
        {
            achievementEventTarget = _eventTarget;
            GameServices.Instance.SubmitAchievement(achievementName, SubmitAchievementComplete);
        }

        private static void SubmitAchievementComplete(bool success, GameServicesError error)
        {
            CustomEvent.Trigger(achievementEventTarget, "SubmitAchievementComplete", success);
        }

        public static void SubmitScore(long score, LeaderboardNames leaderboardName, GameObject _eventTarget)
        {
            leaderboardEventTarget = _eventTarget;
            GameServices.Instance.SubmitScore(score, leaderboardName, SubmitScoreComplete);
        }

        private static void SubmitScoreComplete(bool success, GameServicesError error)
        {
            CustomEvent.Trigger(leaderboardEventTarget, "SubmitScoreComplete", success);
        }

        public static void GetPlayerScore(LeaderboardNames leaderboardName, GameObject _eventTarget)
        {
            playerScoreEventTarget = _eventTarget;
            GameServices.Instance.GetPlayerScore(leaderboardName, GetScoreComplete);
        }

        private static void GetScoreComplete(long score)
        {
            CustomEvent.Trigger(playerScoreEventTarget, "GetScoreComplete", score);
        }

        public static void GetPlayerRank(LeaderboardNames leaderboardName, GameObject _eventTarget)
        {
            playerRankEventTarget = _eventTarget;
            GameServices.Instance.GetPlayerRank(leaderboardName, GetRankComplete);
        }

        private static void GetRankComplete(long rank)
        {
            CustomEvent.Trigger(playerRankEventTarget, "GetRankComplete", rank);
        }
    }
}
#endif
