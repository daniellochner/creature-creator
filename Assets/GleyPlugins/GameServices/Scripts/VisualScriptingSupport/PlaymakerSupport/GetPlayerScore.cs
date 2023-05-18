#if USE_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("http://gleygames.com/documentation/Gley-GameServices-Documentation.pdf")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Get player highscore from leaderboard")]

    public class GetPlayerScore : FsmStateAction
    {
        [Tooltip("Where to send the event.")]
        public FsmEventTarget eventTarget;

        [Tooltip("Leaderboard to get the score from.")]
        public LeaderboardNames leaderboard;

        [Tooltip("Variable where the score will be stored.")]
        public FsmInt score;

        public override void Reset()
        {
            base.Reset();
            eventTarget = FsmEventTarget.Self;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameServices.Instance.GetPlayerScore(leaderboard, CompleteMethod);
        }

        private void CompleteMethod(long arg0)
        {
            score.Value = (int)arg0;
        }
    }
}
#endif