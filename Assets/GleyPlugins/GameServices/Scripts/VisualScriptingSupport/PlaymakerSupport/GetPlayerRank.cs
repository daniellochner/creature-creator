#if USE_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("http://gleygames.com/documentation/Gley-GameServices-Documentation.pdf")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Get player rank from leaderboard")]

    public class GetPlayerRank : FsmStateAction
    {
        [Tooltip("Where to send the event.")]
        public FsmEventTarget eventTarget;

        [Tooltip("Leaderboard to get the rank from.")]
        public LeaderboardNames leaderboard;

        [Tooltip("Variable where the rank will be stored.")]
        public FsmInt rank;

        public override void Reset()
        {
            base.Reset();
            eventTarget = FsmEventTarget.Self;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameServices.Instance.GetPlayerRank(leaderboard, CompleteMethod);
        }

        private void CompleteMethod(long rank)
        {
            this.rank.Value = (int)rank;
        }
    }
}
#endif