using UnityEngine;

namespace DanielLochner.Assets
{
    public class WaitUntilSetup : CustomYieldInstruction
    {
        private ISetupable setupable;

        public WaitUntilSetup(ISetupable setupable)
        {
            this.setupable = setupable;
        }

        public override bool keepWaiting
        {
            get => setupable == null || !setupable.IsSetup;
        }
    }
}