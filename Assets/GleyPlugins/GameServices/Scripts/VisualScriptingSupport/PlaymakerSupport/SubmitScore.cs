#if USE_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("http://gleygames.com/documentation/Gley-GameServices-Documentation.pdf")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Submit score into a leaderboard")]
    public class SubmitScore : FsmStateAction
    {
        [Tooltip("Leaderboard to submit your score")]
        public LeaderboardNames leaderboard;

        [UIHint(UIHint.Variable)]
        [Tooltip("Score to sumbit")]
        public FsmInt score;

        public override void OnEnter()
        {
            GameServices.Instance.SubmitScore(score.Value, leaderboard);
            Finish();
        }
    }
}
#endif
