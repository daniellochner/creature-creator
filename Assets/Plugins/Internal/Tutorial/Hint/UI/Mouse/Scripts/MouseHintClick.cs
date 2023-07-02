using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class MouseHintClick : MouseHint
    {
        #region Fields
        [SerializeField] protected Sprite left;
        [SerializeField] protected Sprite right;

        private IEnumerator anim;
        #endregion

        #region Methods
        private void OnEnable()
        {
            if (anim != null)
            {
                StartCoroutine(anim);
            }
        }

        public virtual void Setup(int button, Transform pos, bool inWorld, float t1 = 0.5f, float t2 = 0.5f, float t3 = 1f)
        {
            icon.sprite = (button == 0) ? left : right;

            anim = AnimateRoutine(pos, inWorld, t1, t2, t3);
            StartCoroutine(anim);
        }

        private IEnumerator AnimateRoutine(Transform pos, bool inWorld, float t1, float t2, float t3)
        {
            while (true)
            {
                if (pos == null) yield break;

                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
                {
                    transform.position = GetPosition(pos, inWorld);
                    transform.localScale = Vector3.Lerp(Vector3.one, 0.9f * Vector3.one, p);
                }, 
                t1);
                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
                {
                    transform.position = GetPosition(pos, inWorld);
                    transform.localScale = Vector3.Lerp(0.9f * Vector3.one, Vector3.one, p);
                },
                t2);
                yield return InvokeUtility.InvokeOverTimeRoutine(delegate
                {
                    transform.position = GetPosition(pos, inWorld);
                },
                t3);
            }
        }
        #endregion
    }
}