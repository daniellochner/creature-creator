#if USE_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("http://gleygames.com/documentation/Gley-GameServices-Documentation.pdf")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Login to GameServices")]
    public class Login : FsmStateAction
    {
        [Tooltip("Where to send the event.")]
        public FsmEventTarget eventTarget;

        [UIHint(UIHint.FsmEvent)]
        [Tooltip("Event sent when Login was successful")]
        public FsmEvent loginSuccess;

        [UIHint(UIHint.FsmEvent)]
        [Tooltip("Event sent when Login failed")]
        public FsmEvent loginFailed;


        public override void Reset()
        {
            base.Reset();
            loginSuccess = null;
            loginFailed = null;
            eventTarget = FsmEventTarget.Self;
        }

        public override void OnEnter()
        {
            if (!GameServices.Instance.IsLoggedIn())
            {
                GameServices.Instance.LogIn(LoginComplete);
            }
            else
            {
                Fsm.Event(eventTarget, loginSuccess);
                Finish();
            }
        }

        private void LoginComplete(bool success)
        {
            if(success)
            {
                Fsm.Event(eventTarget, loginSuccess);
            }
            else
            {
                Fsm.Event(eventTarget, loginFailed);
            }
            Finish();
        }
    }
}
#endif
