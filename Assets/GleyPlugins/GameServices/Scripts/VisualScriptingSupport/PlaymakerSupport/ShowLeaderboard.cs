#if USE_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("http://gleygames.com/documentation/Gley-GameServices-Documentation.pdf")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Show a specific Leaderboard")]
    public class ShowLeaderboard : FsmStateAction
    {
        [Tooltip("Show a leaderboard")]
        public LeaderboardNames leaderboard;

        public override void OnEnter()
        {
            GameServices.Instance.ShowSpecificLeaderboard(leaderboard);
            Finish();
        }
    }
}
#endif
