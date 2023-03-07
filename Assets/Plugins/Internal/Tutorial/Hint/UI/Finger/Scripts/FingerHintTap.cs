using UnityEngine;

namespace DanielLochner.Assets
{
    public class FingerHintTap : MouseHintClick
    {
        #region Methods
        public override void Setup(int hand, Transform pos, bool inWorld, float t1 = 0.5f, float t2 = 0.5f, float t3 = 1f)
        {
            base.Setup((hand == 0) ? 1 : 0, pos, inWorld, t1, t2, t3);
        }
        #endregion
    }
}