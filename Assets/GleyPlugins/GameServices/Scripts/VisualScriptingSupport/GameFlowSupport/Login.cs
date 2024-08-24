#if USE_GAMEFLOW_SUPPORT
namespace GleyGameServices
{
    using GameFlow;
    using UnityEngine;

    [AddComponentMenu("")]
    public class Login : Function
    {
        private bool callbackReceived = false;
        private bool executed;

        public override bool finished
        {
            get
            {
                return callbackReceived;
            }
        }

        // Code implementing any setup required by the action
        protected override void OnSetup()
        {
            callbackReceived = false;
            executed = false;
        }

        // Code implementing the effect of the action
        protected override void OnExecute()
        {
            // Do something with the declared property
            if (executed == false)
            {
                GameServices.Instance.LogIn(LoginComplete);
                executed = true;
            }
        }

        private void LoginComplete(bool success)
        {
            callbackReceived = true;
            _output.SetValue(success);
        }
    }
}
#endif