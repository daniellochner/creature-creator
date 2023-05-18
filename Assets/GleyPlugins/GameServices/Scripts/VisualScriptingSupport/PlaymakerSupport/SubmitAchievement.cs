#if USE_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("http://gleygames.com/documentation/Gley-GameServices-Documentation.pdf")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Submit an Achievement")]
    public class SubmitAchievement : FsmStateAction
    {
        [Tooltip("Achievement to submit")]
        public AchievementNames achievement;

        public override void OnEnter()
        {
            GameServices.Instance.SubmitAchievement(achievement);
            Finish();
        }
    }
}
#endif
