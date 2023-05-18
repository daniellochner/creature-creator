#if USE_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("http://gleygames.com/documentation/Gley-GameServices-Documentation.pdf")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Show all Leaderboards in the game")]
    public class ShowAllLeaderboards : FsmStateAction
    {
        public override void OnEnter()
        {
            GameServices.Instance.ShowLeaderboadsUI();
            Finish();
        }
    }
}
#endif
