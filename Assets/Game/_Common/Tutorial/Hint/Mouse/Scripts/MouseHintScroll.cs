using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class MouseHintScroll : MouseHint
    {
        #region Fields
        [SerializeField] private RectTransform arrow;
        #endregion

        #region Methods
        public void Setup(int dir, Transform pos, bool inWorld)
        {
            arrow.localScale = new Vector3(1, dir, 1);
            StartCoroutine(AnimateRoutine(dir, pos, inWorld));
        }

        private IEnumerator AnimateRoutine(int dir, Transform pos, bool inWorld)
        {
            while (true)
            {
                if (pos == null) yield break;

                transform.position = GetPosition(pos, inWorld);

                yield return null;
            }
        }
        #endregion
    }
}