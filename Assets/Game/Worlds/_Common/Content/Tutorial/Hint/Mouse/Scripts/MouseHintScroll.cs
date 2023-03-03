using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class MouseHintScroll : MouseHint
    {
        #region Fields
        [Header("Desktop")]
        [SerializeField] private Hint scroll;
        [SerializeField] private RectTransform arrow;

        [Header("Handheld")]
        [SerializeField] private Hint pinch;
        #endregion

        #region Methods
        public void Setup(int dir, Transform pos, bool inWorld, float t1 = 2f)
        {
            if (SystemUtility.IsDevice(DeviceType.Desktop))
            {
                Setup(scroll);
                arrow.localScale = new Vector3(1, dir, 1);

                StartCoroutine(AnimateRoutine(dir, pos, inWorld, t1));
            }
            else
            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                Setup(pinch);
            }

        }

        private IEnumerator AnimateRoutine(int dir, Transform pos, bool inWorld, float t1)
        {
            while (true)
            {
                if (pos == null) yield break;

                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
                {
                    transform.position = GetPosition(pos, inWorld);
                    arrow.localPosition = new Vector3(0, 15f + (10f * (0.5f + Mathf.Sin(Mathf.PI * p * 2f) / 2f)), 0);
                },
                t1);
            }
        }
        #endregion
    }
}