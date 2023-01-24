using UnityEngine;

namespace DanielLochner.Assets
{
    public class WaitUntilSetup : CustomYieldInstruction
    {
        private ISetupable[] setupable;

        public WaitUntilSetup(params ISetupable[] setupable)
        {
            this.setupable = setupable;
        }

        public override bool keepWaiting
        {
            get
            {
                if (setupable == null || setupable.Length == 0)
                {
                    return true;
                }
                foreach (ISetupable s in setupable)
                {
                    if (s == null || !s.IsSetup)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}