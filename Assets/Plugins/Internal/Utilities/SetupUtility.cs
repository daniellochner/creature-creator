using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public static class SetupUtility
    {
        public static bool IsSetup(params ISetupable[] setupable)
        {
            foreach (ISetupable s in setupable)
            {
                if (s == null || !s.IsSetup)
                {
                    return false;
                }
            }
            return true;
        }
    }
}