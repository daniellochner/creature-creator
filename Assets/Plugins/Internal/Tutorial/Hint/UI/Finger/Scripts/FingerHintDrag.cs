using UnityEngine;

namespace DanielLochner.Assets
{
    public class FingerHintDrag : MouseHintDrag
    {
        #region Methods
        public override void Setup(int hand, Transform sT, Transform eT, bool sInWorld, bool eInWorld, float t1 = 1f, float t2 = 2f, float t3 = 1f)
        {
            base.Setup((hand == 0) ? 1 : 0, sT, eT, sInWorld, eInWorld, t1, t2, t3);
        }
        #endregion
    }
}